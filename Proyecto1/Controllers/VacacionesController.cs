using Microsoft.AspNetCore.Mvc;
using Proyecto1.BLL;
using Proyecto1.Models;
using Proyecto1.Services;

namespace Proyecto1.Controllers
{
    public class VacacionesController : Controller
    {
        private readonly VacacionesService _service;
        private readonly EmailService _emailService;
        private readonly IConfiguration _config;
        private readonly ColaboradoresService _colaboradoresService;

        public VacacionesController (VacacionesService service, EmailService emailService, IConfiguration config, ColaboradoresService colaboradoresService)
        {
            _service = service;
            _emailService = emailService;
            _config = config;
            _colaboradoresService = colaboradoresService;
        }

        // ================= ADMIN =================
        public async Task<IActionResult> Index(int? idColaborador)
        {
            var lista = await _service.ListarAdminAsync();

            if (idColaborador.HasValue)
            {
                bool existe = await _service.ColaboradorExisteAsync(idColaborador.Value);

                if (!existe)
                {
                    TempData["Error"] = $"No existe ningún colaborador con ID {idColaborador.Value}.";
                    return View(lista);
                }

                var filtrada = lista
                    .Where(v => v.IdColaborador == idColaborador.Value)
                    .ToList();

                if (!filtrada.Any())
                {
                    TempData["Warning"] = "El colaborador no tiene vacaciones registradas.";
                    return View(lista);
                }

                TempData["Success"] = "Vacaciones encontradas correctamente.";
                return View(filtrada);
            }

            return View(lista);
        }


        // ================= COLABORADOR =================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            int? idColaborador = HttpContext.Session.GetInt32("IdColaborador");
            if (!idColaborador.HasValue)
                return RedirectToAction("Index", "Login");

            ViewBag.DiasDisponibles =
                await _service.CalcularDiasDisponiblesAsync(idColaborador.Value);

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vacacion model)
        {
            int? idColaborador = HttpContext.Session.GetInt32("IdColaborador");
            if (!idColaborador.HasValue)
                return RedirectToAction("Index", "Login");

            int diasDisponibles =
                await _service.CalcularDiasDisponiblesAsync(idColaborador.Value);

            // ✅ CALCULAR DÍAS DE VACACIONES DESCONTANDO DÍAS LIBRES
            int diasSolicitados = (int)(model.FechaFin - model.FechaInicio).TotalDays + 1;
            int diasLibres = 0;

            // Contar domingos (días libres semanales) en el rango
            for (DateTime fecha = model.FechaInicio; fecha <= model.FechaFin; fecha = fecha.AddDays(1))
            {
                if (fecha.DayOfWeek == DayOfWeek.Sunday)
                {
                    diasLibres++;
                }
            }

            int diasVacacionesReales = diasSolicitados - diasLibres;

            // 🔴 VALIDACIÓN 1: fechas inválidas
            if (diasSolicitados < 1)
            {
                ModelState.AddModelError(
                    "",
                    "La fecha final no puede ser menor que la fecha inicial."
                );

                ViewBag.DiasDisponibles = diasDisponibles;
                return View(model);
            }

            // 🔴 VALIDACIÓN 2: no exceder días disponibles (DESCONTANDO DÍAS LIBRES)
            if (diasVacacionesReales > diasDisponibles)
            {
                ModelState.AddModelError(
                    "",
                    $"No tiene suficientes días disponibles. Solicitó {diasVacacionesReales} días de vacaciones (descontando {diasLibres} día(s) libre(s)). Días disponibles: {diasDisponibles}"
                );

                ViewBag.DiasDisponibles = diasDisponibles;
                return View(model);
            }

            model.IdColaborador = idColaborador.Value;
            model.FechaSolicitud = DateTime.Now;
            model.Estado = "Pendiente";
            model.DiasSolicitados = diasVacacionesReales; // 👈 Guardar días REALES de vacaciones
            model.DiasDisponibles = diasDisponibles;

            await _service.RegistrarAsync(model);

            TempData["Success"] = "Solicitud de vacaciones enviada correctamente.";
            return RedirectToAction(nameof(MisVacaciones));
        }




        public async Task<IActionResult> MisVacaciones()
        {
            int? idColaborador = HttpContext.Session.GetInt32("IdColaborador");

            if (!idColaborador.HasValue)
                return RedirectToAction("Index", "Login");

            var lista = await _service.ListarPorColaboradorAsync(idColaborador.Value);
            return View(lista);
        }


        // ================= ADMIN =================
        // 📌 Aprobar / Rechazar Vacaciones (Admin)
        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, string estado)
        {
            try
            {
                // ✅ VALIDACIÓN: El usuario logueado NO puede aprobar sus propias vacaciones
                int? idUsuarioLogueado = HttpContext.Session.GetInt32("IdColaborador");
                var solicitud = await _service.ObtenerDetalleAsync(id);

                if (solicitud == null)
                {
                    TempData["Error"] = "No se encontró la solicitud de vacaciones.";
                    return RedirectToAction(nameof(Index));
                }

                // Si el usuario logueado es el que hizo la solicitud, NO PUEDE aprobarla
                if (idUsuarioLogueado.HasValue && solicitud.IdColaborador == idUsuarioLogueado.Value)
                {
                    TempData["Error"] = "No puedes aprobar o rechazar tus propias solicitudes de vacaciones. Otro miembro de RRHH debe hacerlo.";
                    return RedirectToAction(nameof(Index));
                }

                // ✅ Si pasa la validación, continúa normalmente
                // 1️⃣ Cambiar estado en BD
                await _service.CambiarEstadoAsync(id, estado);

                // 2️⃣ Obtener datos para el correo
                var datos = await _service.ObtenerDetalleAsync(id);

                if (datos == null)
                {
                    TempData["Error"] = "No se pudieron obtener los datos de la solicitud.";
                    return RedirectToAction(nameof(Index));
                }

                string asunto = "Estado de su solicitud de vacaciones";

                string mensaje = $@"
Hola {datos.NombreColaborador},

Su solicitud de vacaciones ha sido <b>{estado}</b>.

📅 Fecha inicio: {datos.FechaInicio:dd/MM/yyyy}
📅 Fecha fin: {datos.FechaFin:dd/MM/yyyy}
🧮 Días solicitados: {datos.DiasSolicitados}

Atentamente,
Departamento de Recursos Humanos
";

                // 3️⃣ Obtener correo: primero del SP, si no, de Colaboradores
                string correoColaborador = !string.IsNullOrWhiteSpace(datos.CorreoColaborador)
                    ? datos.CorreoColaborador
                    : null;

                if (string.IsNullOrWhiteSpace(correoColaborador) && datos.IdColaborador > 0)
                {
                    var colaborador = await _colaboradoresService.ObtenerPorId(datos.IdColaborador);
                    correoColaborador = colaborador?.Correo;
                }

                // 4️⃣ Enviar correo al colaborador (si existe)
                if (!string.IsNullOrWhiteSpace(correoColaborador))
                {
                    try
                    {
                        await _emailService.EnviarAsync(
                            correoColaborador,
                            asunto,
                            mensaje
                        );
                    }
                    catch (Exception ex)
                    {
                        TempData["Warning"] = "La solicitud fue procesada, pero hubo un error al enviar el correo al colaborador.";
                    }
                }
                else
                {
                    TempData["Warning"] = "La solicitud fue procesada, pero el colaborador no tiene correo registrado.";
                }

                // 5️⃣ Enviar correo a RRHH
                string correoRRHH = _config["SendGrid:CorreoRRHH"];
                if (!string.IsNullOrWhiteSpace(correoRRHH))
                {
                    try
                    {
                        await _emailService.EnviarAsync(
                            correoRRHH,
                            $"Solicitud de vacaciones {estado}",
                            mensaje
                        );
                    }
                    catch { }
                }

                TempData["Success"] = $"Vacaciones {estado.ToLower()} correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al procesar la solicitud: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpGet]
        public async Task<IActionResult> HistoricoColaborador(int idColaborador)
        {
            // Verificar que el colaborador existe
            bool existe = await _service.ColaboradorExisteAsync(idColaborador);

            if (!existe)
            {
                TempData["Error"] = $"No existe ningún colaborador con ID {idColaborador}.";
                return RedirectToAction(nameof(Index));
            }

            var colaborador = await _colaboradoresService.ObtenerPorId(idColaborador);
            var historico = await _service.ObtenerHistoricoColaboradorAsync(idColaborador);

            // ✅ Calcular iniciales en el controller
            var nombres = colaborador.Nombre.Split(' ');
            string iniciales = nombres.Take(2)
                .Select(n => n[0])
                .Aggregate("", (a, c) => a + c)
                .ToUpper();

            ViewBag.NombreColaborador = $"{colaborador.Nombre} {colaborador.Apellidos}";
            ViewBag.Iniciales = iniciales;
            ViewBag.IdColaborador = idColaborador;
            ViewBag.DiasDisponibles = await _service.CalcularDiasDisponiblesAsync(idColaborador);

            // Estadísticas
            ViewBag.TotalSolicitudes = historico.Count;
            ViewBag.TotalAprobadas = historico.Count(v => v.Estado == "Aprobada");
            ViewBag.TotalPendientes = historico.Count(v => v.Estado == "Pendiente");
            ViewBag.TotalRechazadas = historico.Count(v => v.Estado == "Rechazada");
            ViewBag.TotalDiasUsados = historico.Where(v => v.Estado == "Aprobada").Sum(v => v.DiasSolicitados);

            return View(historico);
        }



    }
}
