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
        private readonly IConfiguration _config;

        public HorasExtrasController(HorasExtrasService service, EmailService emailService,
        IConfiguration config)
        {
            _service = service;
            _emailService = emailService;
            _config = config;
        }

        // 📌 Vista principal (Admin / RRHH)
        public async Task<IActionResult> Index(int? idColaborador)
        {
            var lista = await _service.ListarAsync(); // 🔹 siempre cargamos todo

            if (idColaborador.HasValue)
            {
                bool existe = await _service.ColaboradorExisteAsync(idColaborador.Value);

                if (!existe)
                {
                    TempData["Error"] = $"No existe ningún colaborador con ID {idColaborador.Value}.";
                    return View(lista); // ✔ tabla completa
                }

                var filtrada = lista
                    .Where(h => h.IdColaborador == idColaborador.Value)
                    .ToList();

                if (!filtrada.Any())
                {
                    TempData["Warning"] = "El colaborador existe, pero no tiene horas extras registradas.";
                    return View(lista); // ✔ tabla completa
                }

                TempData["Success"] = "Horas extras encontradas correctamente.";
                return View(filtrada); // ✔ solo resultados
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
            if (!ModelState.IsValid)
                return View(model);

            int? idColaborador = HttpContext.Session.GetInt32("IdColaborador");

            if (!idColaborador.HasValue)
                return RedirectToAction("Index", "Login");

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

            // 3️⃣ Enviar correo al colaborador (con validación + control de error)
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
                        "La solicitud fue procesada, pero el correo del colaborador no es válido o no existe.";
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
                $"Solicitud {estado}",
                mensaje
            );

            // 5️⃣ Mensaje final
            TempData["Success"] = $"Solicitud {estado.ToLower()} correctamente.";

            return RedirectToAction(nameof(Index));
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
