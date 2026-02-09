using Microsoft.AspNetCore.Mvc;
using Proyecto1.BLL;

namespace Proyecto1.Controllers
{
    public class AguinaldoController : Controller
    {
        private readonly AguinaldoService _service;

        public AguinaldoController(AguinaldoService service)
        {
            _service = service;
        }

        // ===============================
        // Vista principal (filtrar por año)
        // ===============================
        public async Task<IActionResult> Index(int? anio, int? idColaborador)
        {
            int year = anio ?? DateTime.Now.Year;

            var lista = await _service.ListarPorAnioAsync(year);

            bool filtroCol = idColaborador.HasValue;

            if (filtroCol)
            {
                lista = lista.Where(x => x.IdColaborador == idColaborador.Value).ToList();

                if (!lista.Any())
                {
                    TempData["Warning"] = $"No se encontraron registros del aguinaldo para el colaborador ID {idColaborador.Value} en {year}.";
                }
            }
            else
            {
                if (!lista.Any())
                    TempData["Info"] = $"No existen datos de aguinaldo para la temporada {year}.";
                else
                    TempData["Success"] = $"Mostrando aguinaldo de la temporada {year}.";
            }

            ViewBag.Anio = year;
            ViewBag.Total = lista.Sum(x => x.MontoAguinaldo);
            ViewBag.IdColaborador = idColaborador;

            return View(lista);
        }




        // ===============================
        // Calcular aguinaldo
        // ===============================
        [HttpPost]
        public async Task<IActionResult> Calcular(int anio, bool recalcular = false)
        {
            // 1️⃣ Validar salarios
            bool hayDatos = await _service.HaySalariosAsync(anio);
            if (!hayDatos)
            {
                TempData["Warning"] = $"No existen salarios registrados en {anio}, no se puede calcular el aguinaldo.";
                return RedirectToAction("Index", new { anio });
            }

            // 2️⃣ Validar si ya existe
            bool existe = await _service.ExisteAguinaldoAsync(anio);

            if (existe && !recalcular)
            {
                TempData["Warning"] = $"El aguinaldo de la temporada {anio} ya fue calculado. Para reflejar ajustes en planilla, puede recalcular el proceso.";
                TempData["ConfirmRecalcular"] = anio;
                return RedirectToAction("Index", new { anio });
            }

            // 3️⃣ Calcular
            await _service.CalcularAsync(anio);

            // 4️⃣ Validar si generó registros
            var lista = await _service.ListarPorAnioAsync(anio);
            if (!lista.Any())
            {
                TempData["Warning"] = $"Se realizaron los calculos, pero no se generaron registros de aguinaldo.";
                return RedirectToAction("Index", new { anio });
            }

            TempData["Success"] = $"Aguinaldo de la temporada {anio} generado correctamente.";
            return RedirectToAction("Index", new { anio });
        }



        // ===============================
        // Aprobar aguinaldo
        // ===============================
        [HttpPost]
        public async Task<IActionResult> Aprobar(int id, int anio)
        {
            await _service.AprobarAsync(id);
            TempData["Success"] = "Aguinaldo aprobado";

            return RedirectToAction("Index", new { anio });
        }

        // ===============================
        // Pagar aguinaldo
        // ===============================
        [HttpPost]
        public async Task<IActionResult> Pagar(int id, int anio)
        {
            await _service.PagarAsync(id);
            TempData["Success"] = "Aguinaldo pagado";

            return RedirectToAction("Index", new { anio });
        }
    }
}
