using Microsoft.AspNetCore.Mvc;
using Proyecto1.Services;

namespace Proyecto1.Controllers
{
    public class MiAsistenciaController : Controller
    {
        private readonly AsistenciaService _service;

        public MiAsistenciaController(AsistenciaService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            int? idColaborador = HttpContext.Session.GetInt32("IdColaborador");

            if (idColaborador == null)
                return RedirectToAction("Index", "Login");

            var asistencias = await _service.ObtenerPorColaboradorAsync(idColaborador.Value);
            return View(asistencias);
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(string tipo)
        {
            try
            {
                int? idColaborador = HttpContext.Session.GetInt32("IdColaborador");

                if (idColaborador == null)
                {
                    TempData["Error"] = "No se pudo identificar el colaborador.";
                    return RedirectToAction(nameof(Index));
                }

                var ahora = DateTime.Now;

                if (tipo == "Entrada")
                {
                    await _service.RegistrarAsync(
                        idColaborador.Value,
                        ahora.Date,
                        ahora.TimeOfDay,
                        null
                    );
                }
                else if (tipo == "Salida")
                {
                    await _service.RegistrarAsync(
                        idColaborador.Value,
                        ahora.Date,
                        null,
                        ahora.TimeOfDay
                    );
                }

                TempData["Success"] = "Asistencia registrada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

