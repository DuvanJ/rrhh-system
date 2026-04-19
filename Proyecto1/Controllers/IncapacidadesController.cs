using Microsoft.AspNetCore.Mvc;
using Proyecto1.BLL;
using Proyecto1.Models;

namespace Proyecto1.Controllers
{
    public class IncapacidadesController : Controller
    {
        private readonly IncapacidadesService _service;

        public IncapacidadesController(IncapacidadesService service)
        {
            _service = service;
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
                    TempData["Error"] =
                        $"No existe ningún colaborador con ID {idColaborador.Value}.";
                    return View(lista);
                }

                var filtrada = lista
                    .Where(i => i.IdColaborador == idColaborador.Value)
                    .ToList();

                if (!filtrada.Any())
                {
                    TempData["Warning"] =
                        "El colaborador no tiene incapacidades registradas.";
                    return View(lista);
                }

                TempData["Success"] = "Incapacidades encontradas correctamente.";
                return View(filtrada);
            }

            return View(lista);
        }

        // ================= COLABORADOR =================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var tipos = await _service.ObtenerTiposActivosAsync();
            ViewBag.TiposIncapacidad = tipos;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Incapacidad model, string accion)
        {
            var tipos = await _service.ObtenerTiposActivosAsync();
            ViewBag.TiposIncapacidad = tipos;

            // ================= BUSCAR =================
            if (accion == "buscar")
            {
                ModelState.Clear(); // 🔥 evita que valide todo

                var colab = await _service.ObtenerInfoColaboradorAsync(model.IdColaborador);

                if (colab == null)
                {
                    ModelState.AddModelError(nameof(model.IdColaborador),
                        "No existe un colaborador activo con ese ID.");
                    return View(model);
                }

                ViewBag.NombreColaborador = $"{colab.Nombre} {colab.Apellidos}";
                return View(model);
            }

            // ================= GUARDAR =================

            var colaborador = await _service.ObtenerInfoColaboradorAsync(model.IdColaborador);

            if (colaborador == null)
            {
                ModelState.AddModelError(nameof(model.IdColaborador),
                    "Debe buscar y validar un colaborador válido.");
            }
            else
            {
                ViewBag.NombreColaborador = $"{colaborador.Nombre} {colaborador.Apellidos}";
            }

            if (model.FechaFin < model.FechaInicio)
            {
                ModelState.AddModelError("",
                    "La fecha final no puede ser menor que la fecha inicial.");
            }

            if (!ModelState.IsValid)
                return View(model);

            model.Estado = "Activa";

            await _service.RegistrarAsync(model);

            TempData["Success"] = "Incapacidad registrada correctamente.";

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> MisIncapacidades()
        {
            int? idColaborador = HttpContext.Session.GetInt32("IdColaborador");

            if (!idColaborador.HasValue)
                return RedirectToAction("Index", "Login");

            var lista =
                await _service.ListarPorColaboradorAsync(idColaborador.Value);

            return View(lista);
        }

        // ================= ADMIN =================
        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, string estado)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(estado))
            {
                TempData["Error"] = "Parámetros inválidos.";
                return RedirectToAction(nameof(Index));
            }

            await _service.CambiarEstadoAsync(id, estado);

            TempData["Success"] =
                $"Incapacidad {estado.ToLower()} correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // ================= DETALLE =================
        public async Task<IActionResult> Detalle(int id)
        {
            if (id <= 0)
                return RedirectToAction(nameof(Index));

            var detalle = await _service.ObtenerDetalleAsync(id);

            if (detalle == null)
            {
                TempData["Error"] = "No se encontró la incapacidad.";
                return RedirectToAction(nameof(Index));
            }

            return View(detalle);
        }


        // ================= BUSCAR COLABORADOR =================
        [HttpPost]
        public async Task<IActionResult> BuscarColaborador(int IdColaborador)
        {
            var existe = await _service.ColaboradorExisteAsync(IdColaborador);

            var model = new Incapacidad
            {
                IdColaborador = IdColaborador
            };

            if (!existe)
            {
                ModelState.AddModelError(
                    nameof(model.IdColaborador),
                    $"No existe ningún colaborador activo con ID {IdColaborador}"
                );

                return View("Create", model);
            }

            var colab = await _service.ObtenerInfoColaboradorAsync(IdColaborador);

            ViewBag.NombreColaborador = $"{colab.Nombre} {colab.Apellidos}";
            TempData["Success"] = "Colaborador encontrado correctamente.";

            return View("Create", model);
        }




    }
}
