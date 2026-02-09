using Microsoft.AspNetCore.Mvc;
using Proyecto1.Services;

namespace Proyecto1.Controllers
{
    public class AsistenciaAuditoriaController : Controller
    {
        private readonly AsistenciaAuditoriaService _service;

        public AsistenciaAuditoriaController(AsistenciaAuditoriaService service)
        {
            _service = service;
        }

        // 📌 Vista principal de auditoría
        public async Task<IActionResult> Index(int? idColaborador)
        {
            var lista = idColaborador.HasValue
                ? await _service.ObtenerPorColaboradorAsync(idColaborador.Value)
                : await _service.ListarAsync();

            if (idColaborador.HasValue)
            {
                if (!lista.Any())
                {
                    TempData["Warning"] = $"No existen ajustes de asistencia para el colaborador ID {idColaborador.Value}.";
                }
                else
                {
                    TempData["Success"] = "Registros cargados correctamente.";
                }
            }

            return View(lista);
        }

    }
}

