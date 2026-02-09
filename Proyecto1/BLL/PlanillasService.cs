using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
        public async Task GenerarAsync(int mes, int año)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Planilla_Generar @Mes, @Año",
                new SqlParameter("@Mes", mes),
                new SqlParameter("@Año", año)
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



        public async Task<PlanillaDetalleColaboradorVM> ObtenerDetalleColaboradorAsync(int idPlanilla, int idColaborador)
        {
            var detalle = await _context.PlanillaDetalle
                .FirstOrDefaultAsync(p => p.IdPlanilla == idPlanilla && p.IdColaborador == idColaborador);

            if (detalle == null)
                return null;

            var planilla = await _context.Planillas.FindAsync(idPlanilla);

            var colaborador = await _context.Colaboradores
                .FirstOrDefaultAsync(c => c.IdColaborador == idColaborador);

            var asistencias = await _context.Asistencia
                .Where(a => a.IdColaborador == idColaborador &&
                            a.Fecha.Month == planilla.Mes &&
                            a.Fecha.Year == planilla.Año)
                .ToListAsync();

            var permisos = await _context.Permisos
                .Where(p => p.IdColaborador == idColaborador &&
                            p.FechaInicio.Month == planilla.Mes &&
                            p.FechaInicio.Year == planilla.Año)
                .ToListAsync();

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
                Deducciones = detalle.Deducciones,
                SalarioBruto = detalle.SalarioBruto,
                SalarioNeto = detalle.SalarioNeto,

                Asistencias = asistencias,
                Permisos = permisos
            };
        }




    }
}
