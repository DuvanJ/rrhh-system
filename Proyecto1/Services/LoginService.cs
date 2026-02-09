using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Proyecto1.DAL;
using Proyecto1.Models;

namespace Proyecto1.Services
{
    public class LoginService
    {
        private readonly ApplicationDbContext _context;

        public LoginService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LoginUsuarioResult?> ValidarLogin(string usuario, string contrasena)
        {
            var pUsuario = new SqlParameter("@NombreUsuario", usuario);
            var pPass = new SqlParameter("@ContrasenaHash", contrasena);

            var result = await _context.LoginUsuarioResult
                .FromSqlRaw("EXEC sp_LoginUsuario @NombreUsuario, @ContrasenaHash",
                    pUsuario, pPass)
                .ToListAsync();

            return result.FirstOrDefault();
        }


    }
}
