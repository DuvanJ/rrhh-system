using Microsoft.AspNetCore.Mvc;
using Proyecto1.BLL;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Proyecto1.Controllers
{
    public class PlanillasController : Controller
    {
        private readonly PlanillasService _service;

        public PlanillasController(PlanillasService service)
        {
            _service = service;
        }

        // ================= INDEX =================
        public async Task<IActionResult> Index()
        {
            var lista = await _service.ListarAsync();
            return View(lista);
        }

        // ================= GENERAR =================
        [HttpPost]
        public async Task<IActionResult> Generar(int mes, int año)
        {
            try
            {
                await _service.GenerarAsync(mes, año);
                TempData["Success"] = "Planilla generada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // ================= DETALLE =================
        public async Task<IActionResult> Detalle(int id)
        {
            var detalle = await _service.ObtenerDetalleAsync(id);
            ViewBag.IdPlanilla = id;
            return View(detalle);
        }


        // ================= EXPORTAR PDF GENERAL =================
        public async Task<IActionResult> ExportarPdf(int idPlanilla)
        {
            var datos = await _service.ObtenerDetallePdfAsync(idPlanilla);
            var planilla = await _service.ObtenerPlanillaAsync(idPlanilla);

            if (planilla == null)
                return NotFound();

            string periodo = $"{planilla.Mes}/{planilla.Año}";

            var document = new PlanillaEmpresaPdf(
                datos,
                "Ferretería Novex",
                periodo
            );

            var pdf = document.GeneratePdf();

            return File(pdf, "application/pdf", $"Planilla_{periodo}.pdf");
        }



        public async Task<IActionResult> ExportarPdfColaborador(int idPlanilla, int idColaborador)
        {
            var detalle = await _service.ObtenerDetalleColaboradorAsync(idPlanilla, idColaborador);

            if (detalle == null)
                return NotFound();

            var document = new PlanillaColaboradorPdf(detalle);

            var pdf = document.GeneratePdf();

            return File(pdf, "application/pdf",
                $"Colilla_{detalle.NombreColaborador}_{idPlanilla}.pdf");
        }



        // ================= ANULAR =================
        [HttpPost]
        public async Task<IActionResult> Anular(int idPlanilla)
        {
            await _service.AnularAsync(idPlanilla);
            TempData["Warning"] = "Planilla anulada correctamente.";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> DetalleColaborador(int idPlanilla, int idColaborador)
        {
            var detalle = await _service.ObtenerDetalleColaboradorAsync(idPlanilla, idColaborador);
            return View(detalle);
        }





    }
}
