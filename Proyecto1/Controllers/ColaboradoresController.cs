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

        public async Task<IActionResult> Index()
        {
            var lista = await _service.ObtenerColaboradores();
            return View(lista);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Colaborador c)
        {
            if (ModelState.IsValid)
            {
                await _service.Insertar(c);
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
                return RedirectToAction("Index");
            }
            return View(c);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var col = await _service.ObtenerPorId(id);
            return View(col);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}
