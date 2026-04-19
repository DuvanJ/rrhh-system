using Microsoft.AspNetCore.Mvc;
using Proyecto1.BLL;
using Proyecto1.Filters;
using Proyecto1.Models;
using Proyecto1.Services;

namespace Proyecto1.Controllers
{
    public class ConsultasController : Controller
    {
        private readonly AguinaldoService _aguinaldoService;
        private readonly ColaboradoresService _colaboradoresService;
        private readonly EvaluacionService _evaluacionService;
        private readonly HorasExtrasService _horasExtrasService;
        private readonly IncapacidadesService _incapacidadesService;
        private readonly PermisosService _permisosService;
        private readonly PlanillasService _planillaService;
        private readonly UniformeService _uniformeService;
        private readonly VacacionesService _vacacionesService;
        private readonly LiquidacionService _liquidacionService;
        private readonly UsuarioService _usuarioService;
        private readonly AsistenciaService _asistenciaService;

        public ConsultasController(
            AguinaldoService aguinaldoService,
            ColaboradoresService colaboradoresService,
            EvaluacionService evaluacionService,
            HorasExtrasService horasExtrasService,
            IncapacidadesService incapacidadesService,
            PermisosService permisosService,
            PlanillasService planillaService,
            UniformeService uniformeService,
            VacacionesService vacacionesService,
            LiquidacionService liquidacionService,
            UsuarioService usuarioService,
            AsistenciaService asistenciaService)
        {
            _aguinaldoService = aguinaldoService;
            _colaboradoresService = colaboradoresService;
            _evaluacionService = evaluacionService;
            _horasExtrasService = horasExtrasService;
            _incapacidadesService = incapacidadesService;
            _permisosService = permisosService;
            _planillaService = planillaService;
            _uniformeService = uniformeService;
            _vacacionesService = vacacionesService;
            _liquidacionService = liquidacionService;
            _usuarioService = usuarioService;
            _asistenciaService = asistenciaService;
        }

        // ================= INDEX =================
        public IActionResult Index()
        {
            return View();
        }

        // ================= AGUINALDOS =================
        public async Task<IActionResult> Aguinaldos(int? anio)
        {
            List<Aguinaldo> lista = anio.HasValue
                ? await _aguinaldoService.ListarPorAnioAsync(anio.Value)
                : await _aguinaldoService.ListarAsync();

            ViewBag.Año = anio;
            return View("Aguinaldo", lista);
        }


        // ================= ASISTENCIA =================
        public async Task<IActionResult> Asistencia()
        {
            var lista = await _asistenciaService.ListarReporteAsync();

            var reporte = lista.Select(a => new AsistenciaReporteVM
            {
                IdAsistencia = a.IdAsistencia,
                IdColaborador = a.IdColaborador,
                NombreColaborador = a.NombreColaborador,
                Fecha = a.Fecha,
                HoraEntrada = a.HoraEntrada,
                HoraSalida = a.HoraSalida,
                Estado =
                    a.HoraEntrada == null ? "Falta" :
                    a.HoraEntrada > new TimeSpan(8, 0, 0) ? "Tardanza" :
                    "Presente"
            }).ToList();

            return View(reporte);
        }


        // ================= AUDITORIA ASISTENCIA =================
        public async Task<IActionResult> AuditoriaAsistencia(int? idColaborador)
        {
            var lista = idColaborador.HasValue
                ? await _asistenciaService.ObtenerAuditoriaPorColaboradorAsync(idColaborador.Value)
                : await _asistenciaService.ListarAuditoriaAsync();

            if (idColaborador.HasValue && !lista.Any())
                TempData["Warning"] = $"No existen ajustes de asistencia para el colaborador ID {idColaborador.Value}.";

            return View(lista);
        }


        // ================= COLABORADORES =================
        public async Task<IActionResult> Colaboradores()
        {
            var lista = await _colaboradoresService.ObtenerColaboradoresConsulta();
            return View(lista);
        }

        // ================= EVALUACIONES =================
        public async Task<IActionResult> Evaluaciones()
        {
            var lista = await _evaluacionService.ListarEvaluacionesAsync();
            return View(lista);
        }

        // ================= HORAS EXTRAS =================
        public async Task<IActionResult> HorasExtras()
        {
            var lista = await _horasExtrasService.ListarAsync();
            return View(lista);
        }

        // ================= INCAPACIDADES =================
        public async Task<IActionResult> Incapacidades()
        {
            var lista = await _incapacidadesService.ListarAdminAsync();
            return View(lista);
        }

        // ================= PERMISOS =================
        public async Task<IActionResult> Permisos()
        {
            var lista = await _permisosService.ListarAsync(); 
            return View(lista);
        }

        
        // ================= PLANILLAS =================
        public async Task<IActionResult> Planillas(int? idColaborador)
        {
            var consultas = await _planillaService.ListarParaConsultaAsync(idColaborador);
            return View(consultas);
        }


        // ================= CONSULTA UNIFORMES =================

        public async Task<IActionResult> Uniformes(int? idColaborador)
        {
            var lista = await _uniformeService.ListarHistorialAsync(idColaborador);
            return View(lista);
        }


        // ================= CONSULTA VACACIONES =================

        public async Task<IActionResult> Vacaciones()
        {
            var lista = await _vacacionesService.ListarAdminAsync();
            return View(lista);
        }


        // ================= CONSULTA LIQUIDACIONES =================

        public async Task<IActionResult> Liquidaciones()
        {
            var lista = await _liquidacionService.ListarAsync();
            return View(lista);
        }


        // ================= CONSULTA USUARIOS =================

        public async Task<IActionResult> Usuarios()
        {
            var lista = await _usuarioService.ListarAsync();
            return View(lista);
        }


        // ================= MI INFORMACIÓN (COLABORADOR) =================
        [ValidateSession]
        public async Task<IActionResult> MiInformacion()
        {
            var idColaborador = HttpContext.Session.GetInt32("IdColaborador") ?? 0;

            if (idColaborador == 0)
            {
                TempData["Error"] = "No se pudo obtener tu información.";
                return RedirectToAction("Index", "Home");
            }

            var colaborador = await _colaboradoresService.ObtenerPorId(idColaborador);

            if (colaborador == null)
            {
                TempData["Error"] = "Colaborador no encontrado.";
                return RedirectToAction("Index", "Home");
            }

            return View(colaborador);
        }


        // ================= MIS EVALUACIONES (COLABORADOR) =================
        [ValidateSession]
        public async Task<IActionResult> MisEvaluaciones()
        {
            var idColaborador = HttpContext.Session.GetInt32("IdColaborador") ?? 0;

            if (idColaborador == 0)
            {
                TempData["Error"] = "No se pudo obtener tus evaluaciones.";
                return RedirectToAction("Index", "Home");
            }

            var evaluaciones = await _evaluacionService.ListarEvaluacionesPorColaboradorAsync(idColaborador);

            if (!evaluaciones.Any())
            {
                TempData["Info"] = "No tienes evaluaciones registradas.";
            }

            return View(evaluaciones);
        }


        // ================= MIS INCAPACIDADES (COLABORADOR) =================
        [ValidateSession]
        public async Task<IActionResult> MisIncapacidades()
        {
            var idColaborador = HttpContext.Session.GetInt32("IdColaborador") ?? 0;

            if (idColaborador == 0)
            {
                TempData["Error"] = "No se pudo obtener tus incapacidades.";
                return RedirectToAction("Index", "Home");
            }

            var incapacidades = await _incapacidadesService.ListarPorColaboradorAsync(idColaborador);

            if (!incapacidades.Any())
            {
                TempData["Info"] = "No tienes incapacidades registradas.";
            }

            return View(incapacidades);
        }


        // ================= MIS UNIFORMES (COLABORADOR) =================
        [ValidateSession]
        public async Task<IActionResult> MisUniformes()
        {
            var idColaborador = HttpContext.Session.GetInt32("IdColaborador") ?? 0;

            if (idColaborador == 0)
            {
                TempData["Error"] = "No se pudo obtener tus uniformes.";
                return RedirectToAction("Index", "Home");
            }

            var uniformes = await _uniformeService.ListarHistorialAsync(idColaborador);

            if (!uniformes.Any())
            {
                TempData["Info"] = "No tienes uniformes asignados.";
            }

            return View(uniformes);
        }

    }
}