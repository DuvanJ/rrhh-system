using Microsoft.AspNetCore.Mvc;
using Proyecto1.Models;
using Proyecto1.Services;

namespace Proyecto1.Controllers
{
    public class AsistenciaController : Controller
    {
        private readonly AsistenciaService _service;

        public AsistenciaController(AsistenciaService service)
        {
            _service = service;
        }

        // 📌 LISTADO GENERAL (RRHH / ADMIN)
        public async Task<IActionResult> Index(int? idColaborador)
        {
            var lista = await _service.ListarAsync(); // siempre cargamos todo

            if (idColaborador.HasValue)
            {
                bool existe = await _service.ColaboradorExisteAsync(idColaborador.Value);

                if (!existe)
                {
                    TempData["Error"] = $"No existe ningún colaborador con ID {idColaborador.Value}.";
                    return View(lista); // ✅ mostramos tabla completa
                }

                var filtrada = lista
                    .Where(a => a.IdColaborador == idColaborador.Value)
                    .ToList();

                if (!filtrada.Any())
                {
                    TempData["Warning"] = "El colaborador existe, pero no tiene registros de asistencia.";
                    return View(lista); // ✅ tabla completa
                }

                TempData["Success"] = "Asistencias encontradas correctamente.";
                return View(filtrada); // ✅ solo cuando sí hay resultados
            }

            return View(lista);
        }



        // 📌 EDITAR (AJUSTES RRHH)
        public async Task<IActionResult> Edit(int id)
        {
            var asistencia = await _service.ObtenerPorIdAsync(id);

            if (asistencia == null)
            {
                TempData["Error"] = "El registro de asistencia no existe.";
                return RedirectToAction(nameof(Index));
            }

            return View(asistencia);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Asistencia a)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Debe completar correctamente todos los campos.";
                return View(a);
            }

            try
            {
                string usuario = User.Identity?.Name ?? "Administrador";

                await _service.AjustarAsistenciaAsync(
                    a,
                    usuario,
                    a.Motivo
                );

                TempData["Success"] = "Asistencia ajustada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(a);
            }
        }

        public async Task<IActionResult> MiAsistencia()
        {
            int? idColaborador = HttpContext.Session.GetInt32("IdColaborador");

            if (!idColaborador.HasValue)
                return RedirectToAction("Index", "Login");

            var lista = await _service.ListarAsync();

            var misAsistencias = lista
                .Where(a => a.IdColaborador == idColaborador.Value)
                .ToList();

            return View("MiAsistencia", misAsistencias);
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(string tipo)
        {
            int? idColaborador = HttpContext.Session.GetInt32("IdColaborador");

            if (!idColaborador.HasValue)
                return RedirectToAction("Index", "Login");

            DateTime fecha = DateTime.Today;
            TimeSpan horaActual = DateTime.Now.TimeOfDay;

            TimeSpan? horaEntrada = null;
            TimeSpan? horaSalida = null;

            if (tipo == "Entrada")
            {
                horaEntrada = horaActual;
            }
            else if (tipo == "Salida")
            {
                horaSalida = horaActual;
            }
            else
            {
                TempData["Error"] = "Tipo de registro inválido.";
                return RedirectToAction(nameof(MiAsistencia));
            }

            try
            {
                await _service.RegistrarAsync(
                    idColaborador.Value,
                    fecha,
                    horaEntrada,
                    horaSalida
                );

                TempData["Success"] = $"Registro de {tipo.ToLower()} realizado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(MiAsistencia));
        }





    }
}
