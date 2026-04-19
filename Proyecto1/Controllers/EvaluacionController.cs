using Microsoft.AspNetCore.Mvc;
using Proyecto1.BLL;
using Proyecto1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto1.Controllers
{
    public class EvaluacionController : Controller
    {
        private readonly EvaluacionService _service;

        public EvaluacionController(EvaluacionService service)
        {
            _service = service;
        }

        // ================= INDEX =================
        public async Task<IActionResult> Index(int? idColaborador)
        {
            var lista = await _service.ListarEvaluacionesAsync();

            if (idColaborador.HasValue)
            {
                var colaborador = await _service.ObtenerColaboradorActivoAsync(idColaborador.Value);

                if (colaborador == null)
                {
                    TempData["Error"] = $"No existe ningún colaborador activo con ID {idColaborador.Value}. Mostrando todos los registros.";
                    return View(lista);
                }

                var filtrada = lista
                    .Where(l => l.IdColaborador == idColaborador.Value)
                    .ToList();

                if (!filtrada.Any())
                {
                    TempData["Warning"] = "El colaborador existe, pero no tiene evaluaciones registradas.";
                    return View(lista);
                }

                TempData["Success"] = "Evaluaciones encontradas correctamente.";
                return View(filtrada);
            }

            return View(lista);
        }

        // ================= CREATE GET =================
        public async Task<IActionResult> Create()
        {
            var criterios = await _service.ListarCriteriosAsync();

            var vm = new EvaluacionVM
            {
                FechaEvaluacion = DateTime.Today,
                Criterios = criterios,
                Detalles = criterios.Select(c => new DetalleEvaluacionVM
                {
                    IdCriterio = c.IdCriterio
                }).ToList()
            };

            return View(vm);
        }

        // ================= CREATE POST =================
        // ================= CREATE POST =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EvaluacionVM vm)
        {
            // ========== LIMPIAR ERRORES DE CAMPOS OPCIONALES ==========
            for (int i = 0; i < vm.Detalles.Count; i++)
            {
                ModelState.Remove($"Detalles[{i}].NombreCriterio");
                ModelState.Remove($"Detalles[{i}].Observacion");  // ✅ AGREGAR ESTA LÍNEA
            }

            // Validar colaborador
            if (vm.IdColaborador <= 0)
                ModelState.AddModelError("IdColaborador", "Debe seleccionar un colaborador.");

            // Validar detalles
            if (vm.Detalles == null || !vm.Detalles.Any())
                ModelState.AddModelError("Detalles", "Debe ingresar al menos un criterio de evaluación.");

            if (vm.Detalles != null && vm.Detalles.Any(d => d.Puntaje == null || d.Puntaje < 0 || d.Puntaje > 100))
                ModelState.AddModelError("Detalles", "Todos los criterios deben tener un puntaje entre 0 y 100.");

            // Si hay errores, recargar la vista con datos
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .Where(msg => !string.IsNullOrEmpty(msg))
                    .ToList();

                TempData["Error"] = string.Join(" | ", errores);

                vm.Criterios = await _service.ListarCriteriosAsync();

                if (vm.Detalles == null || vm.Detalles.Count != vm.Criterios.Count)
                {
                    vm.Detalles = vm.Criterios.Select(c => new DetalleEvaluacionVM
                    {
                        IdCriterio = c.IdCriterio,
                        NombreCriterio = c.NombreCriterio,
                        Puntaje = null,
                        Observacion = ""
                    }).ToList();
                }

                return View(vm);
            }

            try
            {
                var detalles = vm.Detalles.Select(d => new EvaluacionDetalle
                {
                    IdCriterio = d.IdCriterio,
                    Puntaje = d.Puntaje!.Value,
                    Observacion = d.Observacion ?? ""
                }).ToList();

                var evaluacion = new Evaluacion
                {
                    IdColaborador = vm.IdColaborador,
                    FechaEvaluacion = vm.FechaEvaluacion,
                    ComentariosGenerales = vm.ComentariosGenerales ?? "",
                    PuntajeTotal = (decimal)detalles.Average(d => d.Puntaje)
                };

                await _service.CrearEvaluacionAsync(evaluacion, detalles);

                TempData["Success"] = "Evaluación registrada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al registrar la evaluación: " + ex.Message;

                vm.Criterios = await _service.ListarCriteriosAsync();
                vm.Detalles = vm.Criterios.Select(c => new DetalleEvaluacionVM
                {
                    IdCriterio = c.IdCriterio,
                    NombreCriterio = c.NombreCriterio,
                    Puntaje = null,
                    Observacion = ""
                }).ToList();

                return View(vm);
            }
        }

        // ================= EDITAR (GET) =================
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _service.ObtenerEvaluacionPorIdAsync(id);

            if (vm == null)
            {
                TempData["Error"] = "Evaluación no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        // ================= EDITAR (POST) =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EvaluacionVM vm)
        {
            for (int i = 0; i < vm.Detalles.Count; i++)
            {
                ModelState.Remove($"Detalles[{i}].NombreCriterio");
                ModelState.Remove($"Detalles[{i}].Observacion");
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Datos inválidos.";
                vm.Criterios = await _service.ListarCriteriosAsync();
                return View(vm);
            }

            try
            {
                await _service.ActualizarEvaluacionAsync(vm);
                TempData["Success"] = "Evaluación actualizada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                vm.Criterios = await _service.ListarCriteriosAsync();
                return View(vm);
            }
        }

        // ================= DETAILS =================
        public async Task<IActionResult> Details(int id)
        {
            var vm = await _service.ObtenerEvaluacionPorIdAsync(id);

            if (vm == null)
            {
                TempData["Error"] = "Evaluación no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }



        // GET
        public async Task<IActionResult> Delete(int id)
        {
            var vm = await _service.ObtenerEvaluacionPorIdAsync(id);

            if (vm == null)
            {
                TempData["Error"] = "Evaluación no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _service.EliminarEvaluacionAsync(id);
                TempData["Success"] = "Evaluación eliminada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }



        // ================= BUSCAR COLABORADOR =================
        [HttpGet]
        public async Task<IActionResult> VerificarColaborador(int id)
        {
            var c = await _service.ObtenerColaboradorActivoAsync(id);

            if (c == null)
                return Json(new
                {
                    encontrado = false,
                    mensaje = "No se encontró un colaborador activo con ese ID."
                });

            return Json(new
            {
                encontrado = true,
                nombre = c.Nombre
            });
        }
    }
}





