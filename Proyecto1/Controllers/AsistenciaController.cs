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
        public async Task<IActionResult> Index(int? idColaborador, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var listaRaw = await _service.ListarAsync();

            // Convertir a ViewModel con Estado calculado
            var lista = listaRaw.Select(a => new AsistenciaReporteVM
            {
                IdAsistencia = a.IdAsistencia,
                IdColaborador = a.IdColaborador,
                NombreColaborador = a.NombreColaborador,  // ← Ya viene lleno del Service
                Fecha = a.Fecha,
                HoraEntrada = a.HoraEntrada,
                HoraSalida = a.HoraSalida,
                Estado = a.HoraEntrada == null ? "Falta" :
                         a.HoraEntrada > new TimeSpan(8, 0, 0) ? "Tardanza" :
                         "Presente"
            }).ToList();

            // 🔹 FILTRO POR COLABORADOR
            if (idColaborador.HasValue)
            {
                // ❌ ID inválido (0 o negativo)
                if (idColaborador.Value <= 0)
                {
                    TempData["Error"] = "Debe ingresar un ID de colaborador válido.";
                    return View(lista);
                }

                // 🔎 Validar existencia
                bool existe = await _service.ColaboradorExisteAsync(idColaborador.Value);

                if (!existe)
                {
                    TempData["Error"] = $"No existe ningún colaborador con ID {idColaborador.Value}.";
                    return View(lista);
                }

                // 🔍 Filtrar
                lista = lista
                    .Where(a => a.IdColaborador == idColaborador.Value)
                    .ToList();

                if (!lista.Any())
                {
                    TempData["Warning"] = "El colaborador existe, pero no tiene registros.";
                }
                else
                {
                    TempData["Success"] = $"Mostrando asistencia del colaborador ID {idColaborador.Value}.";
                }
            }

            // 🔹 FILTRO POR FECHAS
            if (fechaInicio.HasValue)
                lista = lista.Where(a => a.Fecha >= fechaInicio.Value).ToList();

            if (fechaFin.HasValue)
                lista = lista.Where(a => a.Fecha <= fechaFin.Value).ToList();

            // 🔹 KPIs
            ViewBag.Total = lista.Count;
            ViewBag.Puntuales = lista.Count(a => a.Estado == "Presente");
            ViewBag.Tardias = lista.Count(a => a.Estado == "Tardanza");

            // 🔹 mantener filtros
            ViewBag.IdColaborador = idColaborador;
            ViewBag.FechaInicio = fechaInicio?.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechaFin?.ToString("yyyy-MM-dd");

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
            // 🔥 VALIDAR MOTIVO PRIMERO (ANTES DE ModelState)
            if (string.IsNullOrWhiteSpace(a.Motivo))
            {
                TempData["Warning"] = "Debe ingresar el motivo del ajuste.";
                return View(a);
            }

            // 🔥 VALIDACIONES GENERALES
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Debe completar correctamente los campos.";
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
            catch
            {
                TempData["Error"] = "Ocurrió un error al ajustar la asistencia.";
                return RedirectToAction(nameof(Index));
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
