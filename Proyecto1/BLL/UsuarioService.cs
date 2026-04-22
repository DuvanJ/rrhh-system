using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Proyecto1.DAL;
using Proyecto1.Models;

namespace Proyecto1.Services
{
    public class UsuarioService
    {
        private readonly ApplicationDbContext _context;

        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UsuarioListado>> ListarAsync()
        {
            return await _context.UsuariosListado
                .FromSqlRaw("EXEC sp_ObtenerUsuarios")
                .ToListAsync();
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return (await _context.Usuarios
                .FromSqlRaw("EXEC sp_Usuarios_ObtenerPorId @IdUsuario",
                    new SqlParameter("@IdUsuario", id))
                .ToListAsync())
                .FirstOrDefault();
        }

        public async Task InsertarAsync(Usuario u)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_InsertarUsuario @NombreUsuario, @Correo, @ContrasenaHash, @IdPerfil, @IdColaborador",
                new SqlParameter("@NombreUsuario", u.NombreUsuario),
                new SqlParameter("@Correo", u.Correo ?? ""),
                new SqlParameter("@ContrasenaHash", u.ContrasenaHash),
                new SqlParameter("@IdPerfil", u.IdPerfil),
                new SqlParameter("@IdColaborador", u.IdColaborador)
            );
        }

        public async Task ActualizarAsync(Usuario u)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Usuarios_Actualizar @IdUsuario, @NombreUsuario, @Correo, @IdPerfil, @Estado",
                new SqlParameter("@IdUsuario", u.IdUsuario),
                new SqlParameter("@NombreUsuario", u.NombreUsuario),
                new SqlParameter("@Correo", u.Correo ?? ""),
                new SqlParameter("@IdPerfil", u.IdPerfil),
                new SqlParameter("@Estado", u.Estado)
            );
        }

        public async Task CambiarEstadoAsync(int id)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Usuarios_CambiarEstado @IdUsuario",
                new SqlParameter("@IdUsuario", id)
            );
        }



        public async Task DesactivarPorColaboradorAsync(int idColaborador)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Usuarios_DesactivarPorColaborador @IdColaborador",
                new SqlParameter("@IdColaborador", idColaborador)
            );
        }
    }

}
