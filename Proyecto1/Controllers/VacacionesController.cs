using Microsoft.AspNetCore.Mvc;
using Proyecto1.BLL;
using Proyecto1.Models;

namespace Proyecto1.Controllers
{
    public class VacacionesController : Controller
    {
        private readonly VacacionesService _service;
        private readonly EmailService _emailService;
        private readonly IConfiguration _config;

        public VacacionesController(VacacionesService service, EmailService emailService, IConfiguration config)
        {
            _service = service;
            _emailService = emailService;
            _config = config;
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

            int diasSolicitados =
                (int)(model.FechaFin - model.FechaInicio).TotalDays + 1;

            // 🔴 VALIDACIÓN 1: fechas inválidas (negativo)
            if (diasSolicitados < 1)
            {
                ModelState.AddModelError(
                    "",
                    "La fecha final no puede ser menor que la fecha inicial."
                );

                ViewBag.DiasDisponibles = diasDisponibles;
                return View(model);
            }

            // 🔴 VALIDACIÓN 2: no exceder días disponibles
            if (diasSolicitados > diasDisponibles)
            {
                ModelState.AddModelError(
                    "",
                    $"No tiene suficientes días disponibles. Días disponibles: {diasDisponibles}"
                );

                ViewBag.DiasDisponibles = diasDisponibles;
                return View(model);
            }

            model.IdColaborador = idColaborador.Value;
            model.FechaSolicitud = DateTime.Now;
            model.Estado = "Pendiente";
            model.DiasSolicitados = diasSolicitados;
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

            // 3️⃣ Enviar correo al colaborador
            if (!string.IsNullOrWhiteSpace(datos.CorreoColaborador))
            {
                try
                {
                    await _emailService.EnviarAsync(
                        datos.CorreoColaborador,
                        asunto,
                        mensaje
                    );
                }
                catch
                {
                    TempData["Warning"] =
                        "La solicitud fue procesada, pero el correo del colaborador no existe.";
                }
            }
            else
            {
                TempData["Warning"] =
                    "La solicitud fue procesada, pero el colaborador no tiene correo registrado.";
            }

            // 4️⃣ Enviar correo a RRHH (SIEMPRE)
            await _emailService.EnviarAsync(
                _config["SendGrid:CorreoRRHH"],
                $"Solicitud de vacaciones {estado}",
                mensaje
            );

            // 5️⃣ Mensaje final
            TempData["Success"] = $"Vacaciones {estado.ToLower()} correctamente.";

            return RedirectToAction(nameof(Index));
        }



    }
}
