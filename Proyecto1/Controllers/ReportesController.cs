using Microsoft.AspNetCore.Mvc;
using Proyecto1.BLL;
using Proyecto1.Models;
using Proyecto1.Services;
using ClosedXML.Excel;
using System.IO;

namespace Proyecto1.Controllers
{
    public class ReportesController : Controller
    {
        private readonly PlanillasService _planillaService;
        private readonly ExcelService _excelService;
        private readonly AguinaldoService _aguinaldoService;
        private readonly AsistenciaService _asistenciaService;
        private readonly ColaboradoresService _colaboradoresService;
        private readonly EvaluacionService _evaluacionService;
        private readonly HorasExtrasService _horasExtrasService;
        private readonly IncapacidadesService _incapacidadesService;
        private readonly LiquidacionService _liquidacionService;
        private readonly PermisosService _permisosService;
        private readonly UniformeService _uniformeService;
        private readonly UsuarioService _usuarioService;
        private readonly VacacionesService _vacacionesService;

        public ReportesController(PlanillasService planillaService,
            AguinaldoService aguinaldoService,
            AsistenciaService asistenciaService,
            ColaboradoresService colaboradoresService,
            ExcelService excelService,
            EvaluacionService evaluacionService,
            HorasExtrasService horasExtrasService,
            IncapacidadesService incapacidadesService,
            LiquidacionService liquidacionService,
            PermisosService permisosService,
            UniformeService uniformeService,
            UsuarioService usuarioService,
            VacacionesService vacacionesService)
        {
            _planillaService = planillaService;
            _aguinaldoService = aguinaldoService;
            _asistenciaService = asistenciaService;
            _colaboradoresService = colaboradoresService;
            _excelService = excelService;
            _evaluacionService = evaluacionService;
            _horasExtrasService = horasExtrasService;
            _incapacidadesService = incapacidadesService;
            _liquidacionService = liquidacionService;
            _permisosService = permisosService;
            _uniformeService = uniformeService;
            _usuarioService = usuarioService;
            _vacacionesService = vacacionesService;
        }



        // ================= INDEX =================
        public IActionResult Index()
        {
            return View();
        }

        // ================= REPORTE PLANILLAS (LISTA) =================

        public async Task<IActionResult> Planillas(int? mes, int? anio)
        {
            var lista = await _planillaService.ListarAsync();

            if (mes.HasValue)
                lista = lista.Where(p => p.Mes == mes.Value).ToList();

            if (anio.HasValue)
                lista = lista.Where(p => p.Año == anio.Value).ToList();

            ViewBag.Mes = mes;
            ViewBag.Anio = anio;

            return View(lista);
        }

        // ================= REPORTE PLANILLA DETALLADA =================

        public async Task<IActionResult> PlanillaDetallada(int idPlanilla)
        {
            var detalle = await _planillaService.ObtenerDetalleAsync(idPlanilla);
            ViewBag.IdPlanilla = idPlanilla;

            return View(detalle);
        }

        // ================= REPORTE PLANILLA RESUMEN =================

        public async Task<IActionResult> PlanillaResumen(int idPlanilla)
        {
            var detalle = await _planillaService.ObtenerDetalleAsync(idPlanilla);

            return View(detalle);
        }

        public async Task<IActionResult> ExportarPlanillasExcel(int? mes, int? anio)
        {
            var lista = await _planillaService.ListarAsync();

            if (mes.HasValue)
                lista = lista.Where(p => p.Mes == mes.Value).ToList();

            if (anio.HasValue)
                lista = lista.Where(p => p.Año == anio.Value).ToList();

            var archivo = _excelService.ExportarPlanillas(lista);

            return File(
                archivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "ReportePlanillas.xlsx"
            );
        }


        public async Task<IActionResult> ExportarPlanillaResumenExcel(int idPlanilla)
        {
            var datos = await _planillaService.ObtenerDetalleAsync(idPlanilla);

            var archivo = _excelService.ExportarPlanillaResumen(datos);

            return File(
                archivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"PlanillaResumen_{idPlanilla}.xlsx"
            );
        }


        public async Task<IActionResult> ExportarPlanillaDetalleExcel(int idPlanilla)
        {
            var datos = await _planillaService.ObtenerDetalleAsync(idPlanilla);

            var archivo = _excelService.ExportarPlanillaDetalle(datos);

            return File(
                archivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"PlanillaDetalle_{idPlanilla}.xlsx"
            );
        }



        public async Task<IActionResult> Aguinaldo(int? anio)
        {
            List<Aguinaldo> lista;

            if (anio.HasValue)
                lista = await _aguinaldoService.ListarPorAnioAsync(anio.Value);
            else
                lista = await _aguinaldoService.ListarAsync();

            ViewBag.Anio = anio;

            return View(lista);
        }



        public async Task<IActionResult> ExportarAguinaldoExcel(int? anio)
        {
            List<Aguinaldo> lista;

            if (anio.HasValue)
                lista = await _aguinaldoService.ListarPorAnioAsync(anio.Value);
            else
                lista = await _aguinaldoService.ListarAsync();

            var archivo = _excelService.ExportarAguinaldos(lista);

            return File(
                archivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Reporte_Aguinaldos.xlsx"
            );
        }



        public async Task<IActionResult> Asistencia(
    string? colaborador,
    DateTime? fechaInicio,
    DateTime? fechaFin,
    string? estado)
        {
            var lista = await _asistenciaService.ListarReporteAsync();

            if (!string.IsNullOrEmpty(colaborador))
                lista = lista
                    .Where(x => x.NombreColaborador
                        .Contains(colaborador, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            if (fechaInicio.HasValue)
                lista = lista.Where(x => x.Fecha >= fechaInicio.Value).ToList();

            if (fechaFin.HasValue)
                lista = lista.Where(x => x.Fecha <= fechaFin.Value).ToList();

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

            if (!string.IsNullOrEmpty(estado))
                reporte = reporte.Where(x => x.Estado == estado).ToList();

            ViewBag.TotalRegistros = reporte.Count;
            ViewBag.TotalTardanzas = reporte.Count(x => x.Estado == "Tardanza");
            ViewBag.TotalFaltas = reporte.Count(x => x.Estado == "Falta");
            ViewBag.Colaborador = colaborador;

            return View(reporte);
        }


        

        [HttpGet]
        public async Task<IActionResult> AuditoriaAsistencia(int? idColaborador)
        {
            var auditorias = await _asistenciaService.ListarAuditoriaAsync();

            if (idColaborador.HasValue)
                auditorias = auditorias.Where(a => a.IdColaborador == idColaborador.Value).ToList();

            return View(auditorias);
        }

        [HttpGet]
        public async Task<IActionResult> ExportarAuditoriaExcel()
        {
            var auditorias = await _asistenciaService.ListarAuditoriaAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Auditoría Asistencia");

                // Encabezado
                worksheet.Cell(1, 1).Value = "AUDITORÍA DE ASISTENCIA";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Font.FontSize = 14;
                worksheet.Range(1, 1, 1, 9).Merge();

                // Encabezados de columna
                var headerRow = 3;
                worksheet.Cell(headerRow, 1).Value = "Colaborador ID";
                worksheet.Cell(headerRow, 2).Value = "Fecha Cambio";
                worksheet.Cell(headerRow, 3).Value = "Hora";
                worksheet.Cell(headerRow, 4).Value = "Entrada Anterior";
                worksheet.Cell(headerRow, 5).Value = "Entrada Nueva";
                worksheet.Cell(headerRow, 6).Value = "Salida Anterior";
                worksheet.Cell(headerRow, 7).Value = "Salida Nueva";
                worksheet.Cell(headerRow, 8).Value = "Usuario";
                worksheet.Cell(headerRow, 9).Value = "Motivo";

                var headerRange = worksheet.Range(headerRow, 1, headerRow, 9);
                headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
                headerRange.Style.Font.Bold = true;

                // Datos
                var row = 4;
                foreach (var item in auditorias.OrderByDescending(a => a.FechaModificacion))
                {
                    worksheet.Cell(row, 1).Value = item.IdColaborador;
                    worksheet.Cell(row, 2).Value = item.FechaModificacion.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 3).Value = item.FechaModificacion.ToString("HH:mm:ss");
                    worksheet.Cell(row, 4).Value = item.HoraEntradaAnterior?.ToString(@"hh\:mm");
                    worksheet.Cell(row, 5).Value = item.HoraEntradaNueva?.ToString(@"hh\:mm");
                    worksheet.Cell(row, 6).Value = item.HoraSalidaAnterior?.ToString(@"hh\:mm");
                    worksheet.Cell(row, 7).Value = item.HoraSalidaNueva?.ToString(@"hh\:mm");
                    worksheet.Cell(row, 8).Value = item.UsuarioModifica;
                    worksheet.Cell(row, 9).Value = item.Motivo;
                    row++;
                }

                // Formato de columnas
                worksheet.Column(1).Width = 15;
                worksheet.Column(2).Width = 12;
                worksheet.Column(3).Width = 12;
                worksheet.Column(4).Width = 15;
                worksheet.Column(5).Width = 15;
                worksheet.Column(6).Width = 15;
                worksheet.Column(7).Width = 15;
                worksheet.Column(8).Width = 15;
                worksheet.Column(9).Width = 20;

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;

                    var fileName = $"Auditoria_Asistencia_{DateTime.Now:yyyyMMdd}.xlsx";
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
                }
            }
        }



        public async Task<IActionResult> ExportarAsistenciaExcel(
    string? colaborador,
    DateTime? fechaInicio,
    DateTime? fechaFin,
    string? estado)
        {
            var lista = await _asistenciaService.ListarAsync();

            // FILTRO COLABORADOR
            if (!string.IsNullOrEmpty(colaborador))
            {
                lista = lista
                    .Where(x => x.NombreColaborador
                    .Contains(colaborador, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // FILTRO FECHAS
            if (fechaInicio.HasValue)
                lista = lista.Where(x => x.Fecha >= fechaInicio.Value).ToList();

            if (fechaFin.HasValue)
                lista = lista.Where(x => x.Fecha <= fechaFin.Value).ToList();

            // CONVERTIR A REPORTE
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

            // FILTRO ESTADO
            if (!string.IsNullOrEmpty(estado))
                reporte = reporte.Where(x => x.Estado == estado).ToList();

            var archivo = _excelService.ExportarAsistencia(reporte);

            return File(
                archivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Reporte_Asistencia.xlsx"
            );
        }



        public async Task<IActionResult> Colaboradores(
    string? nombre,
    string? departamento,
    string? estado)
        {
            var lista = await _colaboradoresService.ObtenerColaboradores();

            // 🔎 FILTRO NOMBRE
            if (!string.IsNullOrEmpty(nombre))
            {
                lista = lista
                    .Where(c => (c.Nombre + " " + c.Apellidos)
                    .Contains(nombre, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // 🏢 FILTRO DEPARTAMENTO
            if (!string.IsNullOrEmpty(departamento))
            {
                lista = lista
                    .Where(c => c.Departamento
                    .Contains(departamento, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // 📊 FILTRO ESTADO
            if (!string.IsNullOrEmpty(estado))
            {
                bool estadoBool = estado == "Activo";

                lista = lista
                    .Where(c => c.Estado == estadoBool)
                    .ToList();
            }

            // 📈 INDICADORES
            ViewBag.TotalColaboradores = lista.Count;
            ViewBag.TotalActivos = lista.Count(x => x.Estado);
            ViewBag.TotalInactivos = lista.Count(x => !x.Estado);

            return View(lista);
        }


        public async Task<IActionResult> ExportarColaboradoresExcel(
    string? nombre,
    string? departamento,
    string? estado)
        {
            var lista = await _colaboradoresService.ObtenerColaboradores();

            if (!string.IsNullOrEmpty(nombre))
            {
                lista = lista
                    .Where(c => (c.Nombre + " " + c.Apellidos)
                    .Contains(nombre, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(departamento))
            {
                lista = lista
                    .Where(c => c.Departamento
                    .Contains(departamento, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(estado))
            {
                bool estadoBool = estado == "Activo";

                lista = lista
                    .Where(c => c.Estado == estadoBool)
                    .ToList();
            }

            var archivo = _excelService.ExportarColaboradores(lista);

            return File(
                archivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Reporte_Colaboradores.xlsx"
            );
        }



        public async Task<IActionResult> Evaluaciones(
    string? colaborador,
    DateTime? fechaInicio,
    DateTime? fechaFin)
        {
            var lista = await _evaluacionService.ListarEvaluacionesAsync();

            // 🔎 FILTRO COLABORADOR
            if (!string.IsNullOrEmpty(colaborador))
            {
                lista = lista
                    .Where(x => x.NombreColaborador
                    .Contains(colaborador, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // 📅 FILTRO FECHAS
            if (fechaInicio.HasValue)
                lista = lista.Where(x => x.FechaEvaluacion >= fechaInicio.Value).ToList();

            if (fechaFin.HasValue)
                lista = lista.Where(x => x.FechaEvaluacion <= fechaFin.Value).ToList();

            // 📊 INDICADORES
            ViewBag.TotalEvaluaciones = lista.Count;

            ViewBag.PromedioPuntaje = lista.Any()
                ? lista.Average(x => x.PuntajeTotal ?? 0)
                : 0;

            ViewBag.MejorPuntaje = lista.Any()
                ? lista.Max(x => x.PuntajeTotal ?? 0)
                : 0;

            return View(lista);
        }



        public async Task<IActionResult> DesempeñoColaborador(
    string? colaborador,
    DateTime? fechaInicio,
    DateTime? fechaFin)
        {
            var lista = await _evaluacionService.ListarEvaluacionesAsync();

            // FILTRO COLABORADOR
            if (!string.IsNullOrEmpty(colaborador))
            {
                lista = lista
                    .Where(x => x.NombreColaborador
                    .Contains(colaborador, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // FILTRO FECHAS
            if (fechaInicio.HasValue)
                lista = lista.Where(x => x.FechaEvaluacion >= fechaInicio.Value).ToList();

            if (fechaFin.HasValue)
                lista = lista.Where(x => x.FechaEvaluacion <= fechaFin.Value).ToList();

            // PROMEDIO
            ViewBag.Promedio = lista.Any()
                ? lista.Average(x => x.PuntajeTotal ?? 0)
                : 0;

            // TENDENCIA
            string tendencia = "Sin datos";

            if (lista.Count >= 2)
            {
                var ordenado = lista
                    .OrderBy(x => x.FechaEvaluacion)
                    .ToList();

                var primero = ordenado.First().PuntajeTotal ?? 0;
                var ultimo = ordenado.Last().PuntajeTotal ?? 0;

                if (ultimo > primero)
                    tendencia = "Mejora";
                else if (ultimo < primero)
                    tendencia = "Baja";
                else
                    tendencia = "Estable";
            }

            ViewBag.Tendencia = tendencia;

            return View(lista);
        }


        public async Task<IActionResult> ExportarEvaluacionesExcel(
    string? colaborador,
    DateTime? fechaInicio,
    DateTime? fechaFin)
        {
            var lista = await _evaluacionService.ListarEvaluacionesAsync();

            if (!string.IsNullOrEmpty(colaborador))
            {
                lista = lista
                    .Where(x => x.NombreColaborador
                    .Contains(colaborador, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (fechaInicio.HasValue)
                lista = lista.Where(x => x.FechaEvaluacion >= fechaInicio.Value).ToList();

            if (fechaFin.HasValue)
                lista = lista.Where(x => x.FechaEvaluacion <= fechaFin.Value).ToList();

            var archivo = _excelService.ExportarEvaluaciones(lista);

            return File(
                archivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Reporte_Evaluaciones.xlsx"
            );
        }

        public async Task<IActionResult> HorasExtras(
    string? colaborador,
    DateTime? fechaInicio,
    DateTime? fechaFin,
    string? estado)
        {
            var lista = await _horasExtrasService.ListarAsync();

            // FILTRO COLABORADOR
            if (!string.IsNullOrEmpty(colaborador))
            {
                lista = lista
                    .Where(x => x.NombreColaborador
                    .Contains(colaborador, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // FILTRO FECHAS
            if (fechaInicio.HasValue)
                lista = lista.Where(x => x.Fecha >= fechaInicio.Value).ToList();

            if (fechaFin.HasValue)
                lista = lista.Where(x => x.Fecha <= fechaFin.Value).ToList();

            // FILTRO ESTADO
            if (!string.IsNullOrEmpty(estado))
                lista = lista.Where(x => x.Estado == estado).ToList();

            // INDICADORES
            ViewBag.TotalRegistros = lista.Count;
            ViewBag.TotalHoras = lista.Sum(x => x.CantidadHoras);

            ViewBag.Colaborador = colaborador;

            return View(lista);
        }




        public async Task<IActionResult> ExportarHorasExtrasExcel(
    string? colaborador,
    DateTime? fechaInicio,
    DateTime? fechaFin,
    string? estado)
        {
            var lista = await _horasExtrasService.ListarAsync();

            if (!string.IsNullOrEmpty(colaborador))
            {
                lista = lista
                    .Where(x => x.NombreColaborador
                    .Contains(colaborador, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (fechaInicio.HasValue)
                lista = lista.Where(x => x.Fecha >= fechaInicio.Value).ToList();

            if (fechaFin.HasValue)
                lista = lista.Where(x => x.Fecha <= fechaFin.Value).ToList();

            if (!string.IsNullOrEmpty(estado))
                lista = lista.Where(x => x.Estado == estado).ToList();

            var archivo = _excelService.ExportarHorasExtras(lista);

            return File(
                archivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Reporte_HorasExtras.xlsx"
            );
        }



        public async Task<IActionResult> Incapacidades(
    string? colaborador,
    DateTime? fechaInicio,
    DateTime? fechaFin,
    string? estado)
        {
            var lista = await _incapacidadesService.ListarAdminAsync();

            // FILTRO COLABORADOR
            if (!string.IsNullOrEmpty(colaborador))
            {
                lista = lista
                    .Where(x => x.NombreColaborador
                    .Contains(colaborador, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // FILTRO FECHAS
            if (fechaInicio.HasValue)
                lista = lista.Where(x => x.FechaInicio >= fechaInicio.Value).ToList();

            if (fechaFin.HasValue)
                lista = lista.Where(x => x.FechaFin <= fechaFin.Value).ToList();

            // FILTRO ESTADO
            if (!string.IsNullOrEmpty(estado))
                lista = lista.Where(x => x.Estado == estado).ToList();

            // ================= KPIs =================

            ViewBag.TotalRegistros = lista.Count;

            ViewBag.TotalActivas = lista.Count(x => x.Estado == "Activa");
            ViewBag.TotalFinalizadas = lista.Count(x => x.Estado == "Finalizada");
            ViewBag.TotalAnuladas = lista.Count(x => x.Estado == "Anulada");

            // OPCIONAL (muy bueno para reportes)
            ViewBag.TotalDias = lista.Sum(x => x.DiasIncapacidad);

            ViewBag.Colaborador = colaborador;

            return View(lista);
        }





        public async Task<IActionResult> ExportarIncapacidadesExcel(
    string? colaborador,
    DateTime? fechaInicio,
    DateTime? fechaFin,
    string? estado)
        {
            var lista = await _incapacidadesService.ListarAdminAsync();

            if (!string.IsNullOrEmpty(colaborador))
            {
                lista = lista
                    .Where(x => x.NombreColaborador
                    .Contains(colaborador, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (fechaInicio.HasValue)
                lista = lista.Where(x => x.FechaInicio >= fechaInicio.Value).ToList();

            if (fechaFin.HasValue)
                lista = lista.Where(x => x.FechaFin <= fechaFin.Value).ToList();

            if (!string.IsNullOrEmpty(estado))
                lista = lista.Where(x => x.Estado == estado).ToList();

            var archivo = _excelService.ExportarIncapacidades(lista);

            return File(
                archivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Reporte_Incapacidades.xlsx"
            );
        }



        public async Task<IActionResult> Liquidaciones(
string? colaborador,
DateTime? fechaInicio,
DateTime? fechaFin,
string? estado)
        {
            var lista = await _liquidacionService.ListarAsync();

            // FILTRO COLABORADOR
            if (!string.IsNullOrEmpty(colaborador))
            {
                lista = lista
                    .Where(x => x.Colaborador
                    .Contains(colaborador, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // FILTRO FECHAS
            if (fechaInicio.HasValue)
                lista = lista.Where(x => x.FechaSalida >= fechaInicio.Value).ToList();

            if (fechaFin.HasValue)
                lista = lista.Where(x => x.FechaSalida <= fechaFin.Value).ToList();

            // FILTRO ESTADO
            if (!string.IsNullOrEmpty(estado))
                lista = lista.Where(x => x.Estado == estado).ToList();

            // ================= KPIs =================

            ViewBag.TotalRegistros = lista.Count;

            ViewBag.TotalPendientes = lista.Count(x => x.Estado == "Pendiente");
            ViewBag.TotalAprobadas = lista.Count(x => x.Estado == "Aprobada");
            ViewBag.TotalPagadas = lista.Count(x => x.Estado == "Pagada");

            ViewBag.TotalMonto = lista.Sum(x => x.Total);

            ViewBag.Colaborador = colaborador;

            return View(lista);
        }



        public async Task<IActionResult> ExportarLiquidacionesExcel(
string? colaborador,
DateTime? fechaInicio,
DateTime? fechaFin,
string? estado)
        {
            var lista = await _liquidacionService.ListarAsync();

            // FILTRO COLABORADOR
            if (!string.IsNullOrEmpty(colaborador))
            {
                lista = lista
                    .Where(x => x.Colaborador
                    .Contains(colaborador, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // FILTRO FECHAS
            if (fechaInicio.HasValue)
                lista = lista.Where(x => x.FechaSalida >= fechaInicio.Value).ToList();

            if (fechaFin.HasValue)
                lista = lista.Where(x => x.FechaSalida <= fechaFin.Value).ToList();

            // FILTRO ESTADO
            if (!string.IsNullOrEmpty(estado))
                lista = lista.Where(x => x.Estado == estado).ToList();

            // GENERAR EXCEL DESDE SERVICE
            var archivo = _excelService.ExportarLiquidaciones(lista);

            return File(
                archivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Reporte_Liquidaciones.xlsx");
        }



        public async Task<IActionResult> Permisos(
string? colaborador,
DateTime? fechaInicio,
DateTime? fechaFin,
string? estado)
        {
            var lista = await _permisosService.ListarAsync();

            // FILTRO COLABORADOR
            if (!string.IsNullOrEmpty(colaborador))
            {
                lista = lista
                    .Where(x => x.NombreColaborador
                    .Contains(colaborador, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // FILTRO FECHA
            if (fechaInicio.HasValue)
                lista = lista.Where(x => x.FechaInicio >= fechaInicio.Value).ToList();

            if (fechaFin.HasValue)
                lista = lista.Where(x => x.FechaFin <= fechaFin.Value).ToList();

            // FILTRO ESTADO
            if (!string.IsNullOrEmpty(estado))
                lista = lista.Where(x => x.Estado == estado).ToList();

            // ================= KPIs =================

            ViewBag.TotalRegistros = lista.Count;

            ViewBag.TotalPendientes = lista.Count(x => x.Estado == "Pendiente");
            ViewBag.TotalAprobados = lista.Count(x => x.Estado == "Aprobado");
            ViewBag.TotalRechazados = lista.Count(x => x.Estado == "Rechazado");

            ViewBag.Colaborador = colaborador;

            return View(lista);
        }



        public async Task<IActionResult> ExportarPermisosExcel(
string? colaborador,
DateTime? fechaInicio,
DateTime? fechaFin,
string? estado)
        {
            var lista = await _permisosService.ListarAsync();

            if (!string.IsNullOrEmpty(colaborador))
            {
                lista = lista
                    .Where(x => x.NombreColaborador
                    .Contains(colaborador, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (fechaInicio.HasValue)
                lista = lista.Where(x => x.FechaInicio >= fechaInicio.Value).ToList();

            if (fechaFin.HasValue)
                lista = lista.Where(x => x.FechaFin <= fechaFin.Value).ToList();

            if (!string.IsNullOrEmpty(estado))
                lista = lista.Where(x => x.Estado == estado).ToList();

            var archivo = _excelService.ExportarPermisos(lista);

            return File(
                archivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Reporte_Permisos.xlsx");
        }



        public async Task<IActionResult> Uniformes(
string? colaborador,
DateTime? fechaInicio,
DateTime? fechaFin)
        {
            var lista = await _uniformeService.ListarHistorialAsync(null);

            // FILTRO COLABORADOR
            if (!string.IsNullOrEmpty(colaborador))
            {
                lista = lista
                    .Where(x => x.NombreColaborador
                    .Contains(colaborador, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // FILTRO FECHA
            if (fechaInicio.HasValue)
                lista = lista.Where(x => x.FechaEntrega >= fechaInicio.Value).ToList();

            if (fechaFin.HasValue)
                lista = lista.Where(x => x.FechaEntrega <= fechaFin.Value).ToList();

            // ================= KPIs =================

            ViewBag.TotalEntregas = lista.Count;

            ViewBag.TotalCosto = lista.Sum(x => x.Costo ?? 0);

            ViewBag.TotalCobrarPlanilla = lista
                .Where(x => x.CobrarEnPlanilla)
                .Sum(x => x.Costo ?? 0);

            ViewBag.Colaborador = colaborador;

            return View(lista);
        }

        public async Task<IActionResult> ExportarUniformesExcel(
string? colaborador,
DateTime? fechaInicio,
DateTime? fechaFin)
        {
            var lista = await _uniformeService.ListarHistorialAsync(null);

            if (!string.IsNullOrEmpty(colaborador))
            {
                lista = lista
                    .Where(x => x.NombreColaborador
                    .Contains(colaborador, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (fechaInicio.HasValue)
                lista = lista.Where(x => x.FechaEntrega >= fechaInicio.Value).ToList();

            if (fechaFin.HasValue)
                lista = lista.Where(x => x.FechaEntrega <= fechaFin.Value).ToList();

            var archivo = _excelService.ExportarUniformes(lista);

            return File(
                archivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Reporte_Uniformes.xlsx");
        }



        public async Task<IActionResult> Usuarios(
string? usuario,
string? perfil,
bool? estado)
        {
            var lista = await _usuarioService.ListarAsync();

            // FILTRO USUARIO
            if (!string.IsNullOrEmpty(usuario))
            {
                lista = lista
                    .Where(x => x.NombreUsuario
                    .Contains(usuario, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // FILTRO PERFIL
            if (!string.IsNullOrEmpty(perfil))
            {
                lista = lista
                    .Where(x => x.NombrePerfil == perfil)
                    .ToList();
            }

            // FILTRO ESTADO
            if (estado.HasValue)
            {
                lista = lista
                    .Where(x => x.Estado == estado.Value)
                    .ToList();
            }

            // ================= KPIs =================

            ViewBag.TotalUsuarios = lista.Count;

            ViewBag.UsuariosActivos = lista.Count(x => x.Estado);

            ViewBag.UsuariosInactivos = lista.Count(x => !x.Estado);

            ViewBag.Usuario = usuario;

            return View(lista);
        }



        public async Task<IActionResult> ExportarUsuariosExcel(
string? usuario,
string? perfil,
bool? estado)
        {
            var lista = await _usuarioService.ListarAsync();

            if (!string.IsNullOrEmpty(usuario))
            {
                lista = lista
                    .Where(x => x.NombreUsuario
                    .Contains(usuario, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(perfil))
            {
                lista = lista
                    .Where(x => x.NombrePerfil == perfil)
                    .ToList();
            }

            if (estado.HasValue)
            {
                lista = lista
                    .Where(x => x.Estado == estado.Value)
                    .ToList();
            }

            var archivo = _excelService.ExportarUsuarios(lista);

            return File(
                archivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Reporte_Usuarios.xlsx");
        }


        public async Task<IActionResult> Vacaciones(
string? colaborador,
string? estado)
        {
            var lista = await _vacacionesService.ListarAdminAsync();

            // FILTRO COLABORADOR
            if (!string.IsNullOrEmpty(colaborador))
            {
                lista = lista
                    .Where(x => x.NombreColaborador
                    .Contains(colaborador, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // FILTRO ESTADO
            if (!string.IsNullOrEmpty(estado))
            {
                lista = lista
                    .Where(x => x.Estado == estado)
                    .ToList();
            }

            // KPIs
            ViewBag.TotalSolicitudes = lista.Count;
            ViewBag.Aprobadas = lista.Count(x => x.Estado == "Aprobado");
            ViewBag.Pendientes = lista.Count(x => x.Estado == "Pendiente");
            ViewBag.Rechazadas = lista.Count(x => x.Estado == "Rechazado");

            ViewBag.Colaborador = colaborador;

            return View(lista);
        }


        public async Task<IActionResult> ExportarVacacionesExcel(
string? colaborador,
string? estado)
        {
            var lista = await _vacacionesService.ListarAdminAsync();

            if (!string.IsNullOrEmpty(colaborador))
            {
                lista = lista
                    .Where(x => x.NombreColaborador
                    .Contains(colaborador, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(estado))
            {
                lista = lista
                    .Where(x => x.Estado == estado)
                    .ToList();
            }

            var archivo = _excelService.ExportarVacaciones(lista);

            return File(
                archivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Reporte_Vacaciones.xlsx");
        }


    }




}