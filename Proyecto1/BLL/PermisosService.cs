using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Proyecto1.DAL;
using Proyecto1.Models;

namespace Proyecto1.BLL
{
    public class PermisosService
    {
        private readonly ApplicationDbContext _context;

        public PermisosService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Registrar nuevo permiso
        public async Task RegistrarAsync(Permiso p)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Permisos_Registrar @IdColaborador, @TipoPermiso, @FechaInicio, @FechaFin, @Observaciones",
                new SqlParameter("@IdColaborador", p.IdColaborador),
                new SqlParameter("@TipoPermiso", p.TipoPermiso),
                new SqlParameter("@FechaInicio", p.FechaInicio),
                new SqlParameter("@FechaFin", p.FechaFin),
                new SqlParameter("@Observaciones", (object?)p.Observaciones ?? DBNull.Value)
            );
        }

        // Listado general de permisos
        public async Task<List<Permiso>> ListarAsync()
        {
            return await _context.Permisos
                .FromSqlRaw("EXEC sp_Permisos_Listar")
                .ToListAsync();
        }

        // Listado por colaborador
        public async Task<List<PermisoColaboradorVM>> ListarPorColaboradorAsync(int idColaborador)
        {
            return await _context.PermisosColaborador
                .FromSqlRaw(
                    "EXEC sp_Permisos_ListarPorColaborador @IdColaborador",
                    new SqlParameter("@IdColaborador", idColaborador)
                )
                .ToListAsync();
        }


        // Cambiar estado (Aprobar / Rechazar)
        public async Task CambiarEstadoAsync(int idPermiso, string estado)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Permisos_CambiarEstado @IdPermiso, @Estado",
                new SqlParameter("@IdPermiso", idPermiso),
                new SqlParameter("@Estado", estado)
            );
        }

        // Obtener detalle para enviar correo
        public PermisoDetalleVM? ObtenerDetalle(int idPermiso)
        {
            return _context.PermisosDetalle
                .FromSqlRaw(
                    "EXEC sp_Permisos_ObtenerDetalle @IdPermiso",
                    new SqlParameter("@IdPermiso", idPermiso)
                )
                .AsEnumerable()
                .FirstOrDefault();
        }

        // Verificar existencia de colaborador
        public async Task<bool> ColaboradorExisteAsync(int idColaborador)
        {
            return await _context.Colaboradores
                .AnyAsync(c => c.IdColaborador == idColaborador);
        }

    }
}

