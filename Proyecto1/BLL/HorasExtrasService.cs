using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Proyecto1.DAL;
using Proyecto1.Models;


namespace Proyecto1.Services
{
    public class HorasExtrasService
    {
        private readonly ApplicationDbContext _context;

        public HorasExtrasService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<HoraExtraAdminVM>> ListarAsync()
        {
            return await _context.HorasExtrasAdmin
                .FromSqlRaw("EXEC sp_HorasExtras_Listar")
                .ToListAsync();
        }



        public async Task RegistrarAsync(HoraExtra h)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_HorasExtras_Registrar @IdColaborador, @Fecha, @CantidadHoras, @Motivo",
                new SqlParameter("@IdColaborador", h.IdColaborador),
                new SqlParameter("@Fecha", h.Fecha),
                new SqlParameter("@CantidadHoras", h.CantidadHoras),
                new SqlParameter("@Motivo", h.Motivo ?? (object)DBNull.Value)
            );
        }

        public async Task CambiarEstadoAsync(int idHoraExtra, string estado)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_HorasExtras_CambiarEstado @IdHoraExtra, @Estado",
                new SqlParameter("@IdHoraExtra", idHoraExtra),
                new SqlParameter("@Estado", estado)
            );
        }

        public async Task<List<HoraExtra>> ListarPorColaboradorAsync(int idColaborador)
        {
            return await _context.HorasExtras
                .FromSqlRaw(
                    "EXEC sp_HorasExtras_ListarPorColaborador @IdColaborador",
                    new SqlParameter("@IdColaborador", idColaborador)
                )
                .ToListAsync();
        }

        public async Task<bool> ColaboradorExisteAsync(int idColaborador)
        {
            return await _context.Colaboradores
                .AnyAsync(c => c.IdColaborador == idColaborador);
        }

        public Task<HoraExtraDetalleVM?> ObtenerDetalleAsync(int idHoraExtra)
        {
            var resultado = _context.Set<HoraExtraDetalleVM>()
                .FromSqlRaw(
                    "EXEC dbo.sp_HorasExtras_ObtenerDetalle @IdHoraExtra",
                    new SqlParameter("@IdHoraExtra", idHoraExtra)
                )
                .AsEnumerable()     // rompe EF
                .FirstOrDefault();  // ya es en memoria

            return Task.FromResult(resultado);
        }


    }

}
