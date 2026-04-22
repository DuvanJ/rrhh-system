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
        private readonly ColaboradoresService _colaboradoresService;
        private readonly IConfiguration _config;

        public PermisosController(
            PermisosService service,
            EmailService emailService,
            ColaboradoresService colaboradoresService,
            IConfiguration config)
        {
            _service = service;
            _emailService = emailService;
            _colaboradoresService = colaboradoresService;
            _config = config;
        }

        // =================================
        // Vista principal (Admin)
        // =================================
        public async Task<IActionResult> Index(int? idColaborador)
        {
            var lista = await _service.ListarAsync();

            if (idColaborador.HasValue)
            {
                bool existe = await _service.ColaboradorExisteAsync(idColaborador.Value);

                if (!existe)
                {
                    TempData["Error"] = $"No existe ningún colaborador con ID {idColaborador.Value}.";
                    return View(lista);
                }

                var filtrada = lista
                    .Where(p => p.IdColaborador == idColaborador.Value)
                    .ToList();

                if (!filtrada.Any())
                {
                    TempData["Warning"] = $"El colaborador con ID {idColaborador.Value} no tiene permisos registrados.";
                    return View(lista);
                }

                TempData["Success"] = "Permisos encontrados correctamente.";
                return View(filtrada);
            }

            return View(lista);
        }


        // =================================
        // Aprobar / Rechazar permiso (Admin)
        // =================================
        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, string estado)
        {
            try
            {
                // ✅ VALIDACIÓN: El usuario logueado NO puede aprobar sus propios permisos
                int? idUsuarioLogueado = HttpContext.Session.GetInt32("IdColaborador");
                var solicitud = _service.ObtenerDetalle(id);

                if (solicitud == null)
                {
                    TempData["Error"] = "No se encontró el permiso.";
                    return RedirectToAction(nameof(Index));
                }

                // Si el usuario logueado es el que hizo la solicitud, NO PUEDE aprobarla
                if (idUsuarioLogueado.HasValue && solicitud.IdColaborador == idUsuarioLogueado.Value)
                {
                    TempData["Error"] = "No puedes aprobar o rechazar tus propios permisos. Otro miembro de RRHH debe hacerlo.";
                    return RedirectToAction(nameof(Index));
                }

                // ✅ Si pasa la validación, continúa normalmente
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

                // 🔴 OBTENER CORREO: primero del SP, si no, de Colaboradores
                string correoColaborador = !string.IsNullOrWhiteSpace(detalle.CorreoColaborador)
                    ? detalle.CorreoColaborador
                    : null;

                if (string.IsNullOrWhiteSpace(correoColaborador) && detalle.IdColaborador > 0)
                {
                    var colaborador = await _colaboradoresService.ObtenerPorId(detalle.IdColaborador);
                    correoColaborador = colaborador?.Correo;
                }

                // Enviar correo al colaborador
                if (!string.IsNullOrWhiteSpace(correoColaborador))
                {
                    try
                    {
                        await _emailService.EnviarAsync(correoColaborador, asunto, mensaje);
                    }
                    catch
                    {
                        TempData["Warning"] = "El permiso fue procesado, pero no se pudo enviar el correo al colaborador.";
                    }
                }
                else
                {
                    TempData["Warning"] = "El permiso fue procesado, pero el colaborador no tiene correo registrado.";
                }

                // 🔴 ENVIAR CORREO A RRHH - VALIDAR QUE NO SEA NULO
                string correoRRHH = _config["SendGrid:CorreoRRHH"];

                if (!string.IsNullOrWhiteSpace(correoRRHH))
                {
                    try
                    {
                        await _emailService.EnviarAsync(correoRRHH, $"Permiso {estado}", mensaje);
                    }
                    catch
                    {
                        // Log silenciosamente
                    }
                }

                TempData["Success"] = $"Permiso {estado.ToLower()} correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al procesar el permiso: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
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

            // Determinar si es con goce o no
            model.EsConGoce = model.TipoPermiso == "Con goce salarial";

            await _service.RegistrarAsync(model);

            TempData["Success"] = "Solicitud de permiso enviada correctamente";
            return RedirectToAction("MisPermisos");
        }
    }
}
