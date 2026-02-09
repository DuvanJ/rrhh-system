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
            // Trae todas las evaluaciones
            var lista = await _service.ListarEvaluacionesAsync();

            if (idColaborador.HasValue)
            {
                var colaborador = await _service.ObtenerColaboradorActivoAsync(idColaborador.Value);

                // 1. Colaborador no existe
                if (colaborador == null)
                {
                    TempData["Error"] = $"No existe ningún colaborador activo con ID {idColaborador.Value}. Mostrando todos los registros.";
                    return View(lista);
                }

                // 2. Filtrar por colaborador
                var filtrada = lista
                    .Where(l => l.IdColaborador == idColaborador.Value)
                    .ToList();

                // 3. Existe pero sin evaluaciones
                if (!filtrada.Any())
                {
                    TempData["Warning"] = "El colaborador existe, pero no tiene evaluaciones registradas.";
                    return View(lista);
                }

                // 4. Tiene evaluaciones
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EvaluacionVM vm)
        {
            // VALIDACIÓN MANUAL CLARA
            if (vm.IdColaborador <= 0)
                ModelState.AddModelError("IdColaborador", "Debe seleccionar un colaborador.");

            if (vm.Detalles == null || !vm.Detalles.Any())
                ModelState.AddModelError("", "Debe ingresar al menos un criterio de evaluación.");

            if (vm.Detalles.Any(d => d.Puntaje == null))
                ModelState.AddModelError("", "Todos los criterios deben tener un puntaje.");

            if (!ModelState.IsValid)
            {
                // Mostrar errores reales
                TempData["Error"] = string.Join(" | ",
                    ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                );

                vm.Criterios = await _service.ListarCriteriosAsync();
                return View(vm);
            }

            try
            {
                // Convertir detalles
                var detalles = vm.Detalles.Select(d => new EvaluacionDetalle
                {
                    IdCriterio = d.IdCriterio,
                    Puntaje = d.Puntaje!.Value,
                    Observacion = d.Observacion
                }).ToList();

                // Crear evaluación
                var evaluacion = new Evaluacion
                {
                    IdColaborador = vm.IdColaborador,
                    FechaEvaluacion = vm.FechaEvaluacion,
                    ComentariosGenerales = vm.ComentariosGenerales,
                    PuntajeTotal = detalles.Average(d => d.Puntaje)
                };

                await _service.CrearEvaluacionAsync(evaluacion, detalles);

                TempData["Success"] = "Evaluación registrada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al registrar la evaluación: " + ex.Message;
                vm.Criterios = await _service.ListarCriteriosAsync();
                return View(vm);
            }
        }


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


        [HttpPost]
        public async Task<IActionResult> Edit(EvaluacionVM vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Datos inválidos.";
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
                return View(vm);
            }
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





