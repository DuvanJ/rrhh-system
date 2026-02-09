using Microsoft.AspNetCore.Mvc;
using Proyecto1.BLL;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto1.Controllers
{
    public class LiquidacionesController : Controller
    {
        private readonly LiquidacionService _service;

        public LiquidacionesController(LiquidacionService service)
        {
            _service = service;
        }

        // ================= LISTADO =================
        public async Task<IActionResult> Index(int? idColaborador)
        {
            var lista = await _service.ListarAsync();

            if (idColaborador.HasValue)
            {
                bool existe = await _service.ColaboradorExisteAsync(idColaborador.Value);
                if (!existe)
                {
                    TempData["Error"] = $"No existe ningún colaborador con ID {idColaborador.Value}. Mostrando todas las liquidaciones.";
                    return View(lista);
                }

                var filtrada = lista.Where(l => l.IdColaborador == idColaborador.Value).ToList();
                if (!filtrada.Any())
                {
                    TempData["Warning"] = "El colaborador existe, pero no tiene liquidaciones registradas.";
                    return View(lista);
                }

                TempData["Success"] = "Liquidaciones encontradas correctamente.";
                return View(filtrada);
            }

            return View(lista);
        }

        // ================= FORMULARIO CREAR =================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.TiposSalida = await _service.ListarTiposSalidaAsync();
            return View("Create"); // Apunta explícitamente a Create.cshtml
        }

        // ================= CALCULAR / GENERAR =================
        [HttpPost]
        public async Task<IActionResult> Create(int idColaborador, int idTipoSalida, DateTime fechaSalida)
        {
            try
            {
                // Validar colaborador activo
                if (!await _service.ColaboradorActivoAsync(idColaborador))
                {
                    TempData["Error"] = $"No se puede generar liquidación. El colaborador con ID {idColaborador} no está activo.";
                    return RedirectToAction("Create");
                }

                // Validar si ya existe una liquidación pendiente
                if (await _service.LiquidacionPendienteAsync(idColaborador))
                {
                    TempData["Warning"] = "El colaborador ya tiene una liquidación pendiente. No se puede generar otra hasta aprobar o cancelar la existente.";
                    return RedirectToAction("Create");
                }

                // Generar liquidación
                string usuarioRegistro = User.Identity?.Name ?? "Sistema";
                await _service.CalcularAsync(idColaborador, idTipoSalida, fechaSalida, usuarioRegistro);

                TempData["Success"] = "Liquidación generada correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ocurrió un error al generar la liquidación: {ex.Message}";
                return RedirectToAction("Create");
            }
        }

        // ================= DETALLE =================
        public async Task<IActionResult> Detalle(int id)
        {
            var detalle = await _service.ObtenerDetalleAsync(id);
            if (!detalle.Any())
            {
                TempData["Warning"] = "No existen rubros para esta liquidación.";
                return RedirectToAction("Index");
            }

            ViewBag.IdLiquidacion = id;
            return View(detalle);
        }

        // ================= APROBAR =================
        public async Task<IActionResult> Aprobar(int id)
        {
            await _service.AprobarAsync(id);
            TempData["Success"] = "Liquidación aprobada.";
            return RedirectToAction("Index");
        }

        // ================= PAGAR =================
        public async Task<IActionResult> Pagar(int id)
        {
            await _service.PagarAsync(id);
            TempData["Success"] = "Liquidación marcada como pagada.";
            return RedirectToAction("Index");
        }

        // ================= BUSCAR COLABORADOR ACTIVO =================
        [HttpGet]
        public async Task<IActionResult> BuscarColaborador(int id)
        {
            var c = await _service.ObtenerInfoAsync(id);

            if (c == null)
                return Json(new { nombre = "" });

            return Json(new { nombre = $"{c.Nombre} {c.Apellidos}" });
        }
    }
}


