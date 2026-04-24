using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Proyecto1.DAL;
using Proyecto1.Models;

namespace Proyecto1.BLL
{
    public class PlanillasService
    {
        private readonly ApplicationDbContext _context;

        public PlanillasService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= GENERAR PLANILLA =================
        public async Task GenerarAsync(int mes, int anio)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Planilla_Generar @Mes, @Anio",
                new SqlParameter("@Mes", mes),
                new SqlParameter("@Anio", anio)
            );
        }

        // ================= ANULAR PLANILLA =================
        public async Task AnularAsync(int idPlanilla)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Planilla_Anular @IdPlanilla",
                new SqlParameter("@IdPlanilla", idPlanilla)
            );
        }


        // ================= CAMBIAR ESTADO PLANILLA =================
        public async Task CambiarEstadoAsync(int idPlanilla, int estado)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Planilla_CambiarEstado @IdPlanilla, @Estado",
                new SqlParameter("@IdPlanilla", idPlanilla),
                new SqlParameter("@Estado", estado)
            );
        }



        // ================= LISTAR PLANILLAS =================
        public async Task<List<Planilla>> ListarAsync()
        {
            return await _context.Planillas
                .OrderByDescending(p => p.Año)
                .ThenByDescending(p => p.Mes)
                .ToListAsync();
        }

        // ================= DETALLE POR PLANILLA =================
        public async Task<List<PlanillaDetalleVM>> ObtenerDetalleAsync(int idPlanilla)
        {
            return await _context.PlanillaDetalleVM
                .FromSqlRaw("EXEC sp_Planilla_Detalle_PorPlanilla @IdPlanilla",
                    new SqlParameter("@IdPlanilla", idPlanilla))
                .ToListAsync();
        }



        // ================= DATOS PARA PDF GENERAL =================
        public async Task<List<PlanillaDetalleVM>> ObtenerDetallePdfAsync(int idPlanilla)
        {
            return await _context.PlanillaDetalleVM
                .FromSqlRaw("EXEC sp_Planilla_Detalle_PorPlanilla @IdPlanilla",
                    new SqlParameter("@IdPlanilla", idPlanilla))
                .ToListAsync();
        }

        
        // ================= Consultas =================
        public async Task<List<ConsultaPlanillaVM>> ListarParaConsultaAsync(int? idColaborador = null)
        {
            var planillas = await _context.Planillas
                .OrderByDescending(p => p.Año)
                .ThenByDescending(p => p.Mes)
                .ToListAsync();

            var detalles = await _context.PlanillaDetalle.ToListAsync();
            var colaboradores = await _context.Colaboradores.ToListAsync();
            var permisos = await _context.Permisos.ToListAsync();
            var vacaciones = await _context.Vacaciones.ToListAsync();
            var asistencias = await _context.Asistencia.ToListAsync();

            var resultado = new List<ConsultaPlanillaVM>();

            foreach (var planilla in planillas)
            {
                DateTime inicioMes = new DateTime(planilla.Año, planilla.Mes, 1);
                DateTime finMes = inicioMes.AddMonths(1).AddDays(-1);

                var detallesPlanilla = detalles
                    .Where(d => d.IdPlanilla == planilla.IdPlanilla);

                foreach (var detalle in detallesPlanilla)
                {
                    if (idColaborador.HasValue && detalle.IdColaborador != idColaborador.Value)
                        continue;

                    var colaborador = colaboradores
                        .FirstOrDefault(c => c.IdColaborador == detalle.IdColaborador);

                    if (colaborador == null)
                        continue;

                    var permisosColab = permisos
                        .Where(p => p.IdColaborador == detalle.IdColaborador &&
                                    p.FechaInicio <= finMes &&
                                    p.FechaFin >= inicioMes)
                        .ToList();

                    int totalDiasConGoce = permisosColab.Where(p => p.EsConGoce).Sum(p =>
                    {
                        DateTime inicio = p.FechaInicio < inicioMes ? inicioMes : p.FechaInicio;
                        DateTime fin = p.FechaFin > finMes ? finMes : p.FechaFin;
                        return (fin - inicio).Days + 1;
                    });

                    int totalDiasSinGoce = permisosColab.Where(p => !p.EsConGoce).Sum(p =>
                    {
                        DateTime inicio = p.FechaInicio < inicioMes ? inicioMes : p.FechaInicio;
                        DateTime fin = p.FechaFin > finMes ? finMes : p.FechaFin;
                        return (fin - inicio).Days + 1;
                    });

                    var vacacionesColab = vacaciones
                        .Where(v => v.IdColaborador == detalle.IdColaborador &&
                                    v.Estado.Trim().ToLower() == "aprobada" &&
                                    v.FechaInicio <= finMes &&
                                    v.FechaFin >= inicioMes)
                        .ToList();

                    int diasVacaciones = vacacionesColab.Sum(v =>
                    {
                        DateTime inicio = v.FechaInicio < inicioMes ? inicioMes : v.FechaInicio;
                        DateTime fin = v.FechaFin > finMes ? finMes : v.FechaFin;
                        return (fin - inicio).Days + 1;
                    });

                    var asistenciasColab = asistencias
                        .Where(a => a.IdColaborador == detalle.IdColaborador &&
                                    a.Fecha >= inicioMes &&
                                    a.Fecha <= finMes)
                        .ToList();

                    resultado.Add(new ConsultaPlanillaVM
                    {
                        IdPlanilla = planilla.IdPlanilla,
                        Mes = planilla.Mes,
                        Año = planilla.Año,
                        IdColaborador = detalle.IdColaborador,
                        NombreColaborador = colaborador.Nombre + " " + colaborador.Apellidos,
                        SalarioBruto = detalle.SalarioBruto,
                        SalarioNeto = detalle.SalarioNeto,
                        Deducciones = detalle.Deducciones,
                        MontoRebajoAsistencia = detalle.MontoRebajoAsistencia,
                        DiasLaborados = detalle.DiasLaborados,
                        DiasVacaciones = diasVacaciones,
                        DiasIncapacidad = detalle.DiasIncapacidad,
                        TotalDiasConGoce = totalDiasConGoce,
                        TotalDiasSinGoce = totalDiasSinGoce,
                        AlertaDiasIncompletos = detalle.AlertaDiasIncompletos,

                        Permisos = permisosColab,
                        Vacaciones = vacacionesColab,
                        Asistencias = asistenciasColab
                    });
                }
            }

            return resultado;
        }




        // ================= OBTENER PLANILLA =================
        public async Task<Planilla?> ObtenerPlanillaAsync(int idPlanilla)
        {
            return await _context.Planillas
                .FirstOrDefaultAsync(p => p.IdPlanilla == idPlanilla);
        }

        // ================= DETALLE POR COLABORADOR =================
        public async Task<PlanillaDetalleColaboradorVM?> ObtenerDetalleColaboradorAsync(int idPlanilla, int idColaborador)
        {
            var detalle = await _context.PlanillaDetalle
                .FirstOrDefaultAsync(p =>
                    p.IdPlanilla == idPlanilla &&
                    p.IdColaborador == idColaborador);

            if (detalle == null)
                return null;

            var planilla = await _context.Planillas.FindAsync(idPlanilla);
            var colaborador = await _context.Colaboradores
                .FirstOrDefaultAsync(c => c.IdColaborador == idColaborador);

            if (planilla == null || colaborador == null)
                return null;

            DateTime inicioMes = new DateTime(planilla.Año, planilla.Mes, 1);
            DateTime finMes = inicioMes.AddMonths(1).AddDays(-1);

            var asistencias = await _context.Asistencia
                .Where(a => a.IdColaborador == idColaborador &&
                            a.Fecha >= inicioMes &&
                            a.Fecha <= finMes)
                .ToListAsync();

            var permisos = await _context.Permisos
                .Where(p => p.IdColaborador == idColaborador &&
                            p.FechaInicio <= finMes &&
                            p.FechaFin >= inicioMes)
                .ToListAsync();

            int totalDiasSinGoce = 0;
            int totalDiasConGoce = 0;

            foreach (var permiso in permisos)
            {
                DateTime inicio = permiso.FechaInicio < inicioMes ? inicioMes : permiso.FechaInicio;
                DateTime fin = permiso.FechaFin > finMes ? finMes : permiso.FechaFin;

                int dias = (fin - inicio).Days + 1;

                if (permiso.EsConGoce)
                    totalDiasConGoce += dias;
                else
                    totalDiasSinGoce += dias;
            }

            var vacaciones = await _context.Vacaciones
                .Where(v => v.IdColaborador == idColaborador
                    && v.Estado.Trim().ToLower() == "aprobada"
                    && v.FechaInicio <= finMes
                    && v.FechaFin >= inicioMes)
                .OrderBy(v => v.FechaInicio)
                .ToListAsync();

            int diasVacaciones = vacaciones.Sum(v =>
            {
                DateTime inicio = v.FechaInicio < inicioMes ? inicioMes : v.FechaInicio;
                DateTime fin = v.FechaFin > finMes ? finMes : v.FechaFin;
                return (fin - inicio).Days + 1;
            });

            var deduccionesBD = await _context.PlanillaDetalleDeducciones
    .Where(d => d.IdPlanillaDetalle == detalle.IdPlanillaDetalle)
    .ToListAsync();

            decimal deduccionSalud = deduccionesBD
                .Where(d => d.IdDeduccion == 1)
                .Select(d => d.MontoTrabajador)
                .FirstOrDefault();

            decimal deduccionIVM = deduccionesBD
                .Where(d => d.IdDeduccion == 2)
                .Select(d => d.MontoTrabajador)
                .FirstOrDefault();

            decimal deduccionBancoPopular = deduccionesBD
                .Where(d => d.IdDeduccion == 7)
                .Select(d => d.MontoTrabajador)
                .FirstOrDefault();

            decimal deduccionRenta = deduccionesBD
    .Where(d => d.IdDeduccion == 11)  // ID de "Impuesto sobre la Renta"
    .Select(d => d.MontoTrabajador)
    .FirstOrDefault();


            decimal totalCCSS = deduccionSalud + deduccionIVM;

            return new PlanillaDetalleColaboradorVM
            {
                IdPlanilla = idPlanilla,
                IdColaborador = idColaborador,
                NombreColaborador = colaborador.Nombre + " " + colaborador.Apellidos,

                SalarioBase = detalle.SalarioBase,
                DiasLaborados = detalle.DiasLaborados,
                HorasOrdinarias = detalle.HorasOrdinarias,
                HorasExtras = detalle.HorasExtras,
                MontoHorasExtras = detalle.MontoHorasExtras,

                SalarioBruto = detalle.SalarioBruto,
                SalarioNeto = detalle.SalarioNeto,


                HorasFeriado = detalle.HorasFeriado ?? 0,
                MontoFeriado = detalle.MontoFeriado ?? 0,


                // 🔥 ESTE ES EL TOTAL REAL
                Deducciones = detalle.Deducciones,

                MontoRebajoAsistencia = detalle.MontoRebajoAsistencia,

                DiasIncapacidad = detalle.DiasIncapacidad,
                MontoIncapacidadPatrono = detalle.MontoIncapacidadPatrono,
                MontoIncapacidadCCSS = detalle.MontoIncapacidadCCSS,
                MontoIncapacidadINS = detalle.MontoIncapacidadINS,
                MontoRebajoIncapacidad = detalle.MontoRebajoIncapacidad,

                MontoUniformes = detalle.MontoUniformes,

                DeduccionSalud = deduccionSalud,
                DeduccionIVM = deduccionIVM,
                DeduccionBancoPopular = deduccionBancoPopular,
                DeduccionRenta = deduccionRenta,
                TotalDeduccionCCSS = totalCCSS,

                TotalDiasSinGoce = totalDiasSinGoce,
                TotalDiasConGoce = totalDiasConGoce,
                MontoRebajoSinGoce = detalle.MontoRebajoSinGoce,

                Asistencias = asistencias,
                Permisos = permisos,
                Vacaciones = vacaciones,
                DiasVacaciones = diasVacaciones,

                AlertaDiasIncompletos = detalle.AlertaDiasIncompletos

            };
        }
    }
}