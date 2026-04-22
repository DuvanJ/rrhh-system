using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Proyecto1.DAL;
using Proyecto1.Models;

namespace Proyecto1.BLL
{
    public class LoginService
    {
        private readonly ApplicationDbContext _context;

        public LoginService(ApplicationDbContext context)
        {
            _context = context;
        }

        // =============================
        // LOGIN CON VALIDACIÓN DE ESTADO
        // =============================

        public async Task<(bool esValido, string mensaje, LoginUsuarioResult usuario)> ValidarLogin(string usuario, string contrasena)
        {
            var pUsuario = new SqlParameter("@NombreUsuario", usuario);
            var pPass = new SqlParameter("@ContrasenaHash", contrasena);

            var result = await _context.LoginUsuarioResult
                .FromSqlRaw("EXEC sp_LoginUsuario @NombreUsuario, @ContrasenaHash",
                    pUsuario, pPass)
                .ToListAsync();

            var user = result.FirstOrDefault();

            // Si no hay resultado, las credenciales son inválidas
            if (user == null)
            {
                return (false, "Usuario o contraseña incorrectos", null);
            }

            // Si el usuario existe pero está inactivo
            if (!user.Estado)
            {
                return (false, "El usuario se encuentra inactivo. Por favor contacte al administrador.", user);
            }

            // Si todo es válido
            return (true, "Login exitoso", user);
        }

        // =============================
        // OBTENER USUARIO POR CORREO
        // =============================

        public async Task<Usuario?> ObtenerPorCorreo(string correo)
        {
            var param = new SqlParameter("@Correo", correo);

            var result = await _context.Usuarios
                .FromSqlRaw("EXEC sp_Usuarios_ObtenerPorCorreo @Correo", param)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        // =============================
        // ACTUALIZAR CONTRASEÑA
        // =============================

        public async Task ActualizarContrasena(string correo, string nuevaContrasena)
        {
            var pCorreo = new SqlParameter("@Correo", correo);
            var pPass = new SqlParameter("@NuevaContrasena", nuevaContrasena);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Usuarios_ActualizarContrasena @Correo, @NuevaContrasena",
                pCorreo,
                pPass
            );
        }
    }
}