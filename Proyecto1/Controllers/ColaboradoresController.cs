using Microsoft.AspNetCore.Mvc;
using Proyecto1.Models;
using Proyecto1.Services;

namespace Proyecto1.Controllers
{
    public class ColaboradoresController : Controller
    {
        private readonly ColaboradoresService _service;

        public ColaboradoresController(ColaboradoresService service)
        {
            _service = service;
        }


        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Colaborador c)
        {
            if (ModelState.IsValid)
            {
                await _service.Insertar(c);
                TempData["Success"] = "Colaborador registrado correctamente.";
                return RedirectToAction("Index");
            }
            return View(c);
        }

        public async Task<IActionResult> Details(int id)
        {
            var col = await _service.ObtenerPorId(id);
            return View(col);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var col = await _service.ObtenerPorId(id);
            return View(col);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Colaborador c)
        {
            if (ModelState.IsValid)
            {
                await _service.Actualizar(c);
                TempData["Success"] = "Colaborador actualizado correctamente.";
                return RedirectToAction("Index");
            }
            return View(c);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var col = await _service.ObtenerPorId(id);
            if (col == null)
                return NotFound();

            return View(col);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int IdColaborador)
        {
            await _service.Eliminar(IdColaborador);
            TempData["Warning"] = "Colaborador eliminado.";
            return RedirectToAction(nameof(Index));
        }



        // LISTADO + BUSCADOR
        public async Task<IActionResult> Index(int? buscar)
        {
            var lista = await _service.ObtenerColaboradores();

            if (buscar.HasValue)
            {
                lista = lista
                    .Where(c => c.IdColaborador == buscar.Value)
                    .ToList();

                if (!lista.Any())
                {
                    TempData["Error"] = "El colaborador no existe.";
                }
            }

            ViewBag.Buscar = buscar;
            return View(lista);
        }

        // ENDPOINT AJAX PARA BUSCAR
        [HttpGet]
        public async Task<IActionResult> BuscarColaborador(int id)
        {
            var c = await _service.ObtenerInfoAsync(id);

            if (c == null)
                return NotFound();

            return Json(new
            {
                nombre = $"{c.Nombre} {c.Apellidos}",
                correo = c.Correo,
                puesto = c.Puesto
            });
        }





    }
}
