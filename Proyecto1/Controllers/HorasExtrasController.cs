using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto1.Models;
using Proyecto1.Services;

namespace Proyecto1.Controllers
{
    public class HorasExtrasController : Controller
    {
        private readonly HorasExtrasService _service;
        private readonly EmailService _emailService;
        private readonly ColaboradoresService _colaboradoresService;
        private readonly IConfiguration _config;

        public HorasExtrasController(
            HorasExtrasService service,
            EmailService emailService,
            ColaboradoresService colaboradoresService,
            IConfiguration config)
        {
            _service = service;
            _emailService = emailService;
            _colaboradoresService = colaboradoresService;
            _config = config;
        }

        // 📌 Vista principal (Admin / RRHH)
        public async Task<IActionResult> Index(int? idColaborador)
        {
            var lista = await _service.ListarAsync();

            if (idColaborador.HasValue)
            {
                bool existe = await _service.ColaboradorExisteAsync(idColaborador.Value);

                if (!existe)
                {
                    TempData["Error"] = $"No existe ningún colaborador con ID {idColaborador.Value}.";
                    return View(lista);
                }

                var filtrada = lista
                    .Where(h => h.IdColaborador == idColaborador.Value)
                    .ToList();

                if (!filtrada.Any())
                {
                    TempData["Warning"] = "El colaborador existe, pero no tiene horas extras registradas.";
                    return View(lista);
                }

                TempData["Success"] = "Horas extras encontradas correctamente.";
                return View(filtrada);
            }

            return View(lista);
        }

        // 📌 Crear solicitud (Colaborador)
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(HoraExtra model)
        {
            int? idColaborador = HttpContext.Session.GetInt32("IdColaborador");

            if (!idColaborador.HasValue)
                return RedirectToAction("Index", "Login");

            if (model.CantidadHoras <= 0)
            {
                TempData["Error"] = "Las horas extras deben ser mayor que 0";
                return RedirectToAction("Create");
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Debe completar todos los campos obligatorios";
                return RedirectToAction("Create");
            }

            model.IdColaborador = idColaborador.Value;
            model.Estado = "Pendiente";

            await _service.RegistrarAsync(model);

            TempData["Success"] = "Solicitud de horas extras enviada correctamente";
            return RedirectToAction("MisHorasExtras");
        }

        // 📌 Aprobar / Rechazar (Admin)
        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, string estado)
        {
            try
            {
                // ✅ VALIDACIÓN: El usuario logueado NO puede aprobar sus propias solicitudes
                int? idUsuarioLogueado = HttpContext.Session.GetInt32("IdColaborador");
                var solicitud = await _service.ObtenerDetalleAsync(id);

                if (solicitud == null)
                {
                    TempData["Error"] = "No se encontró la solicitud.";
                    return RedirectToAction(nameof(Index));
                }

                // Si el usuario logueado es el que hizo la solicitud, NO PUEDE aprobarla
                if (idUsuarioLogueado.HasValue && solicitud.IdColaborador == idUsuarioLogueado.Value)
                {
                    TempData["Error"] = "No puedes aprobar o rechazar tus propias solicitudes. Otro miembro de RRHH debe hacerlo.";
                    return RedirectToAction(nameof(Index));
                }

                // 1️⃣ Cambiar estado en BD
                await _service.CambiarEstadoAsync(id, estado);

                // 2️⃣ Obtener datos para el correo
                var datos = await _service.ObtenerDetalleAsync(id);

                if (datos == null)
                {
                    TempData["Error"] = "No se pudieron obtener los datos de la solicitud.";
                    return RedirectToAction(nameof(Index));
                }

                string asunto = "Estado de su solicitud de horas extra";
                string mensaje = $@"
Hola {datos.NombreColaborador},

Su solicitud de horas extra del {datos.Fecha:dd/MM/yyyy} ha sido {estado}.

Cantidad de horas: {datos.CantidadHoras}

Atentamente,
Departamento de RRHH
";

                // 3️⃣ OBTENER CORREO: primero del SP, si no, de Colaboradores
                string correoColaborador = !string.IsNullOrWhiteSpace(datos.CorreoColaborador)
                    ? datos.CorreoColaborador
                    : null;

                if (string.IsNullOrWhiteSpace(correoColaborador) && datos.IdColaborador > 0)
                {
                    var colaborador = await _colaboradoresService.ObtenerPorId(datos.IdColaborador);
                    correoColaborador = colaborador?.Correo;
                }

                // 4️⃣ Enviar correo al colaborador
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
                    catch
                    {
                        TempData["Warning"] =
                            "La solicitud fue procesada, pero el correo del colaborador no es válido o no existe.";
                    }
                }
                else
                {
                    TempData["Warning"] =
                        "La solicitud fue procesada, pero el colaborador no tiene correo registrado.";
                }

                // 5️⃣ ENVIAR CORREO A RRHH
                string correoRRHH = _config["SendGrid:CorreoRRHH"];

                if (!string.IsNullOrWhiteSpace(correoRRHH))
                {
                    try
                    {
                        await _emailService.EnviarAsync(
                            correoRRHH,
                            $"Solicitud {estado}",
                            mensaje
                        );
                    }
                    catch
                    {
                        // Log silenciosamente
                    }
                }

                // 6️⃣ Mensaje final
                TempData["Success"] = $"Solicitud {estado.ToLower()} correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al procesar la solicitud: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }


        public async Task<IActionResult> MisHorasExtras()
        {
            int? idColaborador = HttpContext.Session.GetInt32("IdColaborador");

            if (!idColaborador.HasValue)
                return RedirectToAction("Index", "Login");

            return View(await _service.ListarPorColaboradorAsync(idColaborador.Value));
        }
    }
}
