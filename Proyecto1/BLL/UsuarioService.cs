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

        // LISTAR
        public async Task<List<Usuario>> ListarAsync()
        {
            return await _context.Usuarios
                .FromSqlRaw("EXEC sp_Usuarios_Listar")
                .ToListAsync();
        }

        // OBTENER POR ID
        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            var param = new SqlParameter("@IdUsuario", id);

            var data = await _context.Usuarios
                .FromSqlRaw("SELECT * FROM Usuarios WHERE IdUsuario = @IdUsuario", param)
                .ToListAsync();

            return data.FirstOrDefault();
        }

        // INSERTAR
        public async Task InsertarAsync(Usuario u)
        {
            var p1 = new SqlParameter("@NombreUsuario", u.NombreUsuario);
            var p2 = new SqlParameter("@Correo", u.Correo);
            var p3 = new SqlParameter("@ContrasenaHash", u.ContrasenaHash);
            var p4 = new SqlParameter("@IdPerfil", u.IdPerfil);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Usuarios_Insertar @NombreUsuario, @Correo, @ContrasenaHash, @IdPerfil",
                p1, p2, p3, p4);
        }

        // ACTUALIZAR
        public async Task ActualizarAsync(Usuario u)
        {
            var p0 = new SqlParameter("@IdUsuario", u.IdUsuario);
            var p1 = new SqlParameter("@NombreUsuario", u.NombreUsuario);
            var p2 = new SqlParameter("@Correo", u.Correo);
            var p3 = new SqlParameter("@IdPerfil", u.IdPerfil);
            var p4 = new SqlParameter("@Estado", u.Estado);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Usuarios_Actualizar @IdUsuario, @NombreUsuario, @Correo, @IdPerfil, @Estado",
                p0, p1, p2, p3, p4);
        }

        // CAMBIAR ESTADO
        public async Task CambiarEstadoAsync(int id)
        {
            var p = new SqlParameter("@IdUsuario", id);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Usuarios_CambiarEstado @IdUsuario", p);
        }
    }
}
