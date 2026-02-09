using Microsoft.AspNetCore.Mvc;
using Proyecto1.BLL;
using Proyecto1.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto1.Controllers
{
    public class UniformesController : Controller
    {
        private readonly UniformeService _service;

        public UniformesController(UniformeService service)
        {
            _service = service;
        }

        // ================= LISTAR HISTORIAL =================
        public async Task<IActionResult> Index(int? idColaborador)
        {
            var lista = await _service.ListarHistorialAsync(null);

            if (idColaborador.HasValue)
            {
                bool existe = await _service.ColaboradorExisteAsync(idColaborador.Value);

                if (!existe)
                {
                    TempData["Error"] = $"No existe ningún colaborador con ID {idColaborador.Value}. Mostrando todos los registros.";
                    return View(lista);
                }

                var filtrada = lista
                    .Where(l => l.IdColaborador == idColaborador.Value)
                    .ToList();

                if (!filtrada.Any())
                {
                    TempData["Warning"] = "El colaborador existe, pero no tiene uniformes registrados.";
                    return View(lista);
                }

                TempData["Success"] = "Uniformes encontrados correctamente.";
                return View(filtrada);
            }

            return View(lista);
        }



        // ================= CREAR =================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new EntregaUniformeVM
            {
                IdColaborador = 0,
                NombreColaborador = "",

                TiposUniforme = await _service.ListarTiposAsync(),

                Tallas = new List<string>
        {
            "XS", "S", "M", "L", "XL", "XXL"
        },

                Uniformes = new List<EntregaUniforme>()
            };

            return View(vm);
        }





        [HttpPost]
        public async Task<IActionResult> Create(EntregaUniformeVM vm)
        {
            // Validar que se haya agregado al menos un uniforme
            if (vm.Uniformes == null || vm.Uniformes.Count == 0)
            {
                TempData["Error"] = "Debe agregar al menos un uniforme.";
                vm.TiposUniforme = await _service.ListarTiposAsync();
                vm.Tallas = new List<string> { "XS", "S", "M", "L", "XL", "XXL" };
                return View(vm);
            }

            // ===== Validación de duplicados dentro del formulario =====
            var combinaciones = new HashSet<string>();
            foreach (var u in vm.Uniformes)
            {
                var key = $"{u.IdTipoUniforme}-{u.Talla}";
                if (!combinaciones.Add(key))
                {
                    TempData["Error"] = "No se permiten uniformes duplicados (mismo tipo y talla). Ajuste la cantidad si desea más.";
                    vm.TiposUniforme = await _service.ListarTiposAsync();
                    vm.Tallas = new List<string> { "XS", "S", "M", "L", "XL", "XXL" };
                    return View(vm);
                }

                // Asignar datos generales
                u.IdColaborador = vm.IdColaborador;
                u.FechaEntrega = vm.FechaEntrega;
                u.Observaciones = vm.Observaciones;
            }

            // Guardar uniformes
            await _service.CrearMultiplesAsync(vm.Uniformes);

            TempData["Success"] = "Uniformes registrados correctamente.";
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EntregaUniformeVM vm)
        {
            if (vm.Uniformes == null || vm.Uniformes.Count == 0)
            {
                TempData["Error"] = "Debe existir al menos un uniforme.";
                vm.TiposUniforme = await _service.ListarTiposAsync();
                vm.Tallas = new List<string> { "XS", "S", "M", "L", "XL", "XXL" };
                return View(vm);
            }

            foreach (var u in vm.Uniformes)
            {
                u.IdColaborador = vm.IdColaborador;
                u.FechaEntrega = vm.FechaEntrega;
                u.Observaciones = vm.Observaciones;

                await _service.ActualizarAsync(u);
            }

            TempData["Success"] = "Entrega actualizada correctamente.";
            return RedirectToAction(nameof(Index));
        }


        // ================= ELIMINAR =================
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.EliminarAsync(id);
            TempData["Success"] = "Entrega eliminada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // ================= BUSCAR COLABORADOR =================
        [HttpGet]
        public async Task<IActionResult> VerificarColaborador(int id)
        {
            var c = await _service.ObtenerColaboradorActivoAsync(id);

            if (c == null)
                return Json(new { encontrado = false, mensaje = "No se encontró un colaborador activo con ese ID." });

            return Json(new { encontrado = true, nombre = c.Nombre });
        }
    }
}
