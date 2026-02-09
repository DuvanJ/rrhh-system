using Microsoft.AspNetCore.Mvc;
using Proyecto1.BLL;
using Proyecto1.Models;
using Proyecto1.Services;

namespace Proyecto1.Controllers
{
    public class PermisosController : Controller
    {
        private readonly PermisosService _service;
        private readonly EmailService _emailService;
        private readonly IConfiguration _config;

        public PermisosController(PermisosService service, EmailService emailService, IConfiguration config)
        {
            _service = service;
            _emailService = emailService;
            _config = config;
        }

        // =================================
        // Vista principal (Admin)
        // =================================
        // 📌 Vista principal (Admin / RRHH)
        public async Task<IActionResult> Index(int? idColaborador)
        {
            // 1️⃣ Cargar todos los permisos
            var lista = await _service.ListarAsync();

            // 2️⃣ Filtrar por ID de colaborador si se proporciona
            if (idColaborador.HasValue)
            {
                // ✅ Verificar si el colaborador existe
                bool existe = await _service.ColaboradorExisteAsync(idColaborador.Value);

                if (!existe)
                {
                    TempData["Error"] = $"No existe ningún colaborador con ID {idColaborador.Value}.";
                    return View(lista); // ✔ Mostrar toda la lista
                }

                // ✅ Filtrar permisos por colaborador
                var filtrada = lista
                    .Where(p => p.IdColaborador == idColaborador.Value)
                    .ToList();

                if (!filtrada.Any())
                {
                    TempData["Warning"] = $"El colaborador con ID {idColaborador.Value} no tiene permisos registrados.";
                    return View(lista); // ✔ Mostrar toda la lista
                }

                TempData["Success"] = "Permisos encontrados correctamente.";
                return View(filtrada); // ✔ Mostrar solo los permisos filtrados
            }

            // 3️⃣ Si no hay ID, mostrar toda la lista
            return View(lista);
        }


        // =================================
        // Aprobar / Rechazar permiso (Admin)
        // =================================
        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, string estado)
        {
            await _service.CambiarEstadoAsync(id, estado);

            var detalle = _service.ObtenerDetalle(id);

            if (detalle == null)
            {
                TempData["Error"] = "No se pudieron obtener los datos del permiso.";
                return RedirectToAction(nameof(Index));
            }

            string asunto = "Estado de su solicitud de permiso";
            string mensaje = $@"
Hola {detalle.NombreColaborador},

Su solicitud de permiso del {detalle.FechaInicio:dd/MM/yyyy} al {detalle.FechaFin:dd/MM/yyyy}
ha sido {estado}.

Tipo de permiso: {detalle.TipoPermiso}

Atentamente,
Departamento de RRHH
";

            // Enviar correo al colaborador
            if (!string.IsNullOrWhiteSpace(detalle.CorreoColaborador))
            {
                try
                {
                    await _emailService.EnviarAsync(detalle.CorreoColaborador, asunto, mensaje);
                }
                catch
                {
                    TempData["Warning"] = "El permiso fue procesado, pero no se pudo enviar el correo al colaborador.";
                }
            }

            // Enviar correo a RRHH siempre
            await _emailService.EnviarAsync(_config["SendGrid:CorreoRRHH"], $"Permiso {estado}", mensaje);

            TempData["Success"] = $"Permiso {estado.ToLower()} correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // =================================
        // Vista para el colaborador: Mis permisos
        // =================================
        public async Task<IActionResult> MisPermisos()
        {
            int? idColaborador = HttpContext.Session.GetInt32("IdColaborador");

            if (!idColaborador.HasValue)
                return RedirectToAction("Index", "Login");

            var permisos = await _service.ListarPorColaboradorAsync(idColaborador.Value);
            return View(permisos);
        }

        // =================================
        // Crear nuevo permiso (Colaborador)
        // =================================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Permiso model)
        {
            if (!ModelState.IsValid)
                return View(model);

            int? idColaborador = HttpContext.Session.GetInt32("IdColaborador");
            if (!idColaborador.HasValue)
                return RedirectToAction("Index", "Login");

            model.IdColaborador = idColaborador.Value;

            await _service.RegistrarAsync(model);

            TempData["Success"] = "Solicitud de permiso enviada correctamente";
            return RedirectToAction("MisPermisos");
        }
    }
}

