using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Proyecto1.DAL;
using Proyecto1.Models;

namespace Proyecto1.BLL
{
    public class IncapacidadesService
    {
        private readonly ApplicationDbContext _context;

        public IncapacidadesService(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // ADMIN
        // =========================

        public async Task<List<IncapacidadAdminVM>> ListarAdminAsync()
        {
            return await _context.Set<IncapacidadAdminVM>()
                .FromSqlRaw("EXEC sp_Incapacidad_Listar_Admin")
                .ToListAsync();
        }

        // =========================
        // COLABORADOR
        // =========================

        public async Task<List<IncapacidadColabVM>> ListarPorColaboradorAsync(int idColaborador)
        {
            return await _context.Set<IncapacidadColabVM>()
                .FromSqlRaw(
                    "EXEC sp_Incapacidad_ConsultarPorColaborador @IdColaborador",
                    new SqlParameter("@IdColaborador", idColaborador)
                )
                .ToListAsync();
        }


        public async Task<List<TiposIncapacidad>> ObtenerTiposActivosAsync()
        {
            return await _context.Set<TiposIncapacidad>()
                .FromSqlRaw("SELECT * FROM Cat_TiposIncapacidad WHERE Estado = 1")
                .ToListAsync();
        }

        // =========================
        // REGISTRAR
        // =========================

        public async Task RegistrarAsync(Incapacidad i)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC sp_Incapacidad_Insertar 
            @IdColaborador,
            @IdTipoIncapacidad,
            @FechaInicio,
            @FechaFin,
            @Motivo,
            @DocumentoAdjunto",
                new SqlParameter("@IdColaborador", i.IdColaborador),
                new SqlParameter("@IdTipoIncapacidad", i.IdTipoIncapacidad),
                new SqlParameter("@FechaInicio", i.FechaInicio),
                new SqlParameter("@FechaFin", i.FechaFin),
                new SqlParameter("@Motivo", i.Motivo ?? (object)DBNull.Value),
                new SqlParameter("@DocumentoAdjunto", i.DocumentoAdjunto ?? (object)DBNull.Value)
            );
        }

        // =========================
        // ACTUALIZAR
        // =========================

        public async Task ActualizarAsync(Incapacidad i)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC sp_Incapacidad_Actualizar
            @IdIncapacidad,
            @IdTipoIncapacidad,
            @FechaInicio,
            @FechaFin,
            @Motivo,
            @DocumentoAdjunto,
            @Estado",
                new SqlParameter("@IdIncapacidad", i.IdIncapacidad),
                new SqlParameter("@IdTipoIncapacidad", i.IdTipoIncapacidad),
                new SqlParameter("@FechaInicio", i.FechaInicio),
                new SqlParameter("@FechaFin", i.FechaFin),
                new SqlParameter("@Motivo", i.Motivo ?? (object)DBNull.Value),
                new SqlParameter("@DocumentoAdjunto", i.DocumentoAdjunto ?? (object)DBNull.Value),
                new SqlParameter("@Estado", i.Estado)
            );
        }

        // =========================
        // CAMBIAR ESTADO
        // =========================

        public async Task CambiarEstadoAsync(int idIncapacidad, string estado)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Incapacidad_CambiarEstado @IdIncapacidad, @Estado",
                new SqlParameter("@IdIncapacidad", idIncapacidad),
                new SqlParameter("@Estado", estado)
            );
        }

        // =========================
        // DETALLE
        // =========================

        public Task<IncapacidadDetalleVM?> ObtenerDetalleAsync(int idIncapacidad)
        {
            var resultado = _context.Set<IncapacidadDetalleVM>()
                .FromSqlRaw(
                    "EXEC sp_Incapacidad_ObtenerDetalle @IdIncapacidad",
                    new SqlParameter("@IdIncapacidad", idIncapacidad)
                )
                .AsEnumerable()
                .FirstOrDefault();

            return Task.FromResult(resultado);
        }

        // =========================
        // VALIDACIONES
        // =========================

        public async Task<bool> ColaboradorExisteAsync(int idColaborador)
        {
            return await _context.Colaboradores
                .AnyAsync(c => c.IdColaborador == idColaborador);
        }


        public async Task<Colaborador?> ObtenerInfoColaboradorAsync(int idColaborador)
        {
            return await _context.Colaboradores
                .Where(c => c.IdColaborador == idColaborador && c.Estado)
                .Select(c => new Colaborador
                {
                    IdColaborador = c.IdColaborador,
                    Nombre = c.Nombre,
                    Apellidos = c.Apellidos
                })
                .FirstOrDefaultAsync();
        }

    }
}
