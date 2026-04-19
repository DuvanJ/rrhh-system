using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto1.BLL;
using Proyecto1.Filters;
using Proyecto1.Models;
using Proyecto1.Services;
using System.Diagnostics;

namespace Proyecto1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ColaboradoresService _colaboradoresService;
        private readonly PermisosService _permisosService;
        private readonly UsuarioService _usuarioService;
        private readonly PlanillasService _planillasService;
        private readonly IncapacidadesService _incapacidadesService;
        private readonly AsistenciaService _asistenciaService;
        private readonly VacacionesService _vacacionesService;
        private readonly HorasExtrasService _horasExtrasService;
        private readonly EvaluacionService _evaluacionService;
        private readonly UniformeService _uniformeService;
        private readonly AguinaldoService _aguinaldoService;
        private readonly LiquidacionService _liquidacionService;


        public HomeController(
            ColaboradoresService colaboradoresService,
            PermisosService permisosService,
            UsuarioService usuarioService,
            PlanillasService planillasService,
            IncapacidadesService incapacidadesService,
            AsistenciaService asistenciaService,
            VacacionesService vacacionesService,
            HorasExtrasService horasExtrasService,
            EvaluacionService evaluacionService,
            UniformeService uniformeService,
            AguinaldoService aguinaldoService,
            LiquidacionService liquidacionService)
        {
            _colaboradoresService = colaboradoresService;
            _permisosService = permisosService;
            _usuarioService = usuarioService;
            _planillasService = planillasService;
            _incapacidadesService = incapacidadesService;
            _asistenciaService = asistenciaService;
            _vacacionesService = vacacionesService;
            _horasExtrasService = horasExtrasService;
            _evaluacionService = evaluacionService;
            _uniformeService = uniformeService;
            _aguinaldoService = aguinaldoService;
            _liquidacionService = liquidacionService;
        }

        [ValidateSession]
        public async Task<IActionResult> Index()
        {
            try
            {
                var rol = HttpContext.Session.GetString("Perfil") ?? "";
                var idColaborador = HttpContext.Session.GetInt32("IdColaborador") ?? 0;

                ViewBag.Usuario = HttpContext.Session.GetString("Usuario") ?? "Usuario";
                ViewBag.Perfil = rol;

                // ═══════════════════════════════════════════════════
                // 🔹 KPIs ADMINISTRADOR
                // ═══════════════════════════════════════════════════
                if (rol == "Administrador")
                {
                    var usuarios = await _usuarioService.ListarAsync();
                    ViewBag.TotalUsuarios = usuarios?.Count ?? 0;

                    var colaboradores = await _colaboradoresService.ObtenerColaboradoresActivos();
                    ViewBag.TotalColaboradores = colaboradores?.Count ?? 0;

                    var asistencias = await _asistenciaService.ListarAsync();
                    ViewBag.TotalAsistencias = asistencias?.Count ?? 0;

                    var horasExtras = await _horasExtrasService.ListarAsync();
                    ViewBag.HorasExtrasPendientes = horasExtras?.Count(h => h.Estado == "Pendiente") ?? 0;

                    var permisos = await _permisosService.ListarAsync();
                    ViewBag.PermisosPendientes = permisos?.Count(p => p.Estado == "Pendiente") ?? 0;

                    var vacaciones = await _vacacionesService.ListarAdminAsync();
                    ViewBag.VacacionesPendientes = vacaciones?.Count(v => v.Estado == "Pendiente") ?? 0;

                    var incapacidades = await _incapacidadesService.ListarAdminAsync();
                    ViewBag.IncapacidadesActivas = incapacidades?.Count(i => i.Estado == "Activa") ?? 0;


                    

                    var planillas = await _planillasService.ListarAsync();
                    ViewBag.TotalPlanillas = planillas?.Count ?? 0;

                    return View("_DashboardAdministrador");
                }

                // ═══════════════════════════════════════════════════
                // 🔹 KPIs RRHH (TODO MENOS USUARIOS Y COLABORADORES)
                // ═══════════════════════════════════════════════════
                else if (rol == "RRHH")
                {
                    var asistencias = await _asistenciaService.ListarAsync();
                    ViewBag.TotalAsistencias = asistencias?.Count ?? 0;

                    var horasExtras = await _horasExtrasService.ListarAsync();
                    ViewBag.HorasExtrasPendientes = horasExtras?.Count(h => h.Estado == "Pendiente") ?? 0;

                    var permisos = await _permisosService.ListarAsync();
                    ViewBag.PermisosPendientes = permisos?.Count(p => p.Estado == "Pendiente") ?? 0;

                    var vacaciones = await _vacacionesService.ListarAdminAsync();
                    ViewBag.VacacionesPendientes = vacaciones?.Count(v => v.Estado == "Pendiente") ?? 0;

                    var incapacidades = await _incapacidadesService.ListarAdminAsync();
                    ViewBag.IncapacidadesActivas = incapacidades?.Count(i => i.Estado == "Activa") ?? 0;

                    var liquidaciones = await _liquidacionService.ListarAsync();
                    ViewBag.LiquidacionesPendientes = liquidaciones?.Count(l => l.Estado == "Pendiente") ?? 0;

                    var aguinaldos = await _aguinaldoService.ListarAsync();
                    ViewBag.AguinaldosPendientes = aguinaldos?.Count(a => a.Estado == "Pendiente") ?? 0;

                    var planillas = await _planillasService.ListarAsync();
                    ViewBag.TotalPlanillas = planillas?.Count ?? 0;

                    return View("_DashboardRRHH");
                }

                // ═══════════════════════════════════════════════════
                // 🔹 KPIs COLABORADOR
                // ═══════════════════════════════════════════════════
                else if (rol == "Colaborador")
                {
                    // 1️⃣ Mi Asistencia
                    var asistencias = await _asistenciaService.ObtenerPorColaboradorAsync(idColaborador);
                    var hoy = DateTime.Today;
                    var mesActual = asistencias.Where(a => a.Fecha.Month == hoy.Month && a.Fecha.Year == hoy.Year).ToList();

                    ViewBag.AsistenciaPresente = mesActual.Count(a => a.HoraEntrada != null && a.HoraEntrada <= new TimeSpan(8, 0, 0));
                    ViewBag.AsistenciaTardanzas = mesActual.Count(a => a.HoraEntrada != null && a.HoraEntrada > new TimeSpan(8, 0, 0));
                    ViewBag.AsistenciaFaltas = mesActual.Count(a => a.HoraEntrada == null);

                    // 2️⃣ Mis Permisos
                    var misPermisos = await _permisosService.ListarAsync();
                    var permisosColaborador = misPermisos.Where(p => p.IdColaborador == idColaborador).ToList();

                    ViewBag.PermisosAprobados = permisosColaborador.Count(p => p.Estado == "Aprobado");
                    ViewBag.PermisosPendientes = permisosColaborador.Count(p => p.Estado == "Pendiente");

                    // 3️⃣ Mis Vacaciones
                    var misVacaciones = await _vacacionesService.ListarPorColaboradorAsync(idColaborador);
                    var vacacionesAprobadas = misVacaciones.Where(v => v.Estado == "Aprobado").ToList();

                    // Calcular días disponibles
                    var diasDisponibles = await _vacacionesService.CalcularDiasDisponiblesAsync(idColaborador);
                    var diasUsados = vacacionesAprobadas.Sum(v => v.DiasSolicitados);

                    ViewBag.VacacionesDisponibles = diasDisponibles > 0 ? diasDisponibles : 0;
                    ViewBag.VacacionesUsadas = diasUsados;

                    // 4️⃣ Mis Horas Extras
                    var misHorasExtras = await _horasExtrasService.ListarAsync();
                    var horasExtrasColaborador = misHorasExtras.Where(h => h.IdColaborador == idColaborador).ToList();

                    ViewBag.HorasExtrasPendientes = horasExtrasColaborador.Count(h => h.Estado == "Pendiente");
                    ViewBag.HorasExtrasAprobadas = horasExtrasColaborador.Count(h => h.Estado == "Aprobada");

                    return View("_DashboardColaborador");
                }

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar el dashboard";
                return View();
            }
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult SinPermisos()
        {
            return View();
        }


        
        // 🔴 ERROR - Maneja todos los errores
        [AllowAnonymous]
        public IActionResult Error(int? statusCode = null)
        {
            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = statusCode ?? 500
            };

            return View(errorViewModel);
        }
    }
}