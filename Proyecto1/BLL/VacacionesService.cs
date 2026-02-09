using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Proyecto1.DAL;
using Proyecto1.Models;

namespace Proyecto1.BLL
{
    public class VacacionesService
    {
        private readonly ApplicationDbContext _context;

        public VacacionesService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ADMIN
        public async Task<List<VacacionAdminVM>> ListarAdminAsync()
        {
            return await _context.Set<VacacionAdminVM>()
                .FromSqlRaw("EXEC sp_Vacaciones_Listar_Admin")
                .ToListAsync();
        }

        // COLABORADOR
        public async Task<List<VacacionColabVM>> ListarPorColaboradorAsync(int idColaborador)
        {
            return await _context.Set<VacacionColabVM>()
                .FromSqlRaw(
                    "EXEC sp_Vacaciones_ListarPorColaborador @IdColaborador",
                    new SqlParameter("@IdColaborador", idColaborador)
                )
                .ToListAsync();
        }


        public async Task RegistrarAsync(Vacacion v)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Vacaciones_Registrar @IdColaborador, @FechaInicio, @FechaFin, @DiasSolicitados",
                new SqlParameter("@IdColaborador", v.IdColaborador),
                new SqlParameter("@FechaInicio", v.FechaInicio),
                new SqlParameter("@FechaFin", v.FechaFin),
                new SqlParameter("@DiasSolicitados", v.DiasSolicitados)
            );
        }



        public async Task CambiarEstadoAsync(int idVacacion, string estado)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Vacaciones_CambiarEstado @IdVacacion, @Estado",
                new SqlParameter("@IdVacacion", idVacacion),
                new SqlParameter("@Estado", estado)
            );
        }

        public async Task<bool> ColaboradorExisteAsync(int idColaborador)
        {
            return await _context.Colaboradores
                .AnyAsync(c => c.IdColaborador == idColaborador);
        }




        public Task<int> CalcularDiasDisponiblesAsync(int idColaborador)
        {
            var resultado = _context.Set<DiasDisponiblesVM>()
                .FromSqlRaw(
                    "EXEC sp_Vacaciones_CalcularDiasDisponibles @IdColaborador",
                    new SqlParameter("@IdColaborador", idColaborador)
                )
                .AsEnumerable()   // rompe EF → pasa a memoria
                .FirstOrDefault();

            int dias = resultado?.DiasDisponibles ?? 0;

            return Task.FromResult(dias);
        }



        public Task<VacacionDetalleVM?> ObtenerDetalleAsync(int idVacacion)
        {
            var resultado = _context.Set<VacacionDetalleVM>()
                .FromSqlRaw(
                    "EXEC sp_Vacaciones_ObtenerDetalle @IdVacacion",
                    new SqlParameter("@IdVacacion", idVacacion)
                )
                .AsEnumerable()      // rompe EF
                .FirstOrDefault();   // ya es en memoria

            return Task.FromResult(resultado);
        }




    }
}

