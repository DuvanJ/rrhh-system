using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Proyecto1.DAL;
using Proyecto1.Models;

namespace Proyecto1.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTAR
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios
                .FromSqlRaw(@"SELECT 
                                U.IdUsuario,
                                U.NombreUsuario,
                                U.Correo,
                                U.IdPerfil,
                                U.Estado,
                                P.NombrePerfil
                             FROM Usuarios U
                             INNER JOIN Perfiles P ON U.IdPerfil = P.IdPerfil")
                .ToListAsync();

            return View(usuarios);
        }

        // CREAR (GET)
        public IActionResult Crear()
        {
            return View();
        }

        // CREAR (POST)
        [HttpPost]
        public async Task<IActionResult> Crear(Usuario usuario)
        {
            if (!ModelState.IsValid)
                return View(usuario);

            var sql = @"INSERT INTO Usuarios 
                       (NombreUsuario, Correo, ContrasenaHash, IdPerfil, Estado)
                       VALUES (@NombreUsuario, @Correo, @ContrasenaHash, @IdPerfil, 1)";

            var p1 = new SqlParameter("@NombreUsuario", usuario.NombreUsuario);
            var p2 = new SqlParameter("@Correo", usuario.Correo);
            var p3 = new SqlParameter("@ContrasenaHash", usuario.ContrasenaHash);
            var p4 = new SqlParameter("@IdPerfil", usuario.IdPerfil);

            await _context.Database.ExecuteSqlRawAsync(sql, p1, p2, p3, p4);

            return RedirectToAction("Index");
        }

        // EDITAR (GET)
        public async Task<IActionResult> Editar(int id)
        {
            var sql = @"SELECT 
                            U.IdUsuario,
                            U.NombreUsuario,
                            U.Correo,
                            U.IdPerfil,
                            U.Estado,
                            P.NombrePerfil
                        FROM Usuarios U
                        INNER JOIN Perfiles P ON U.IdPerfil = P.IdPerfil
                        WHERE IdUsuario = @Id";

            var p = new SqlParameter("@Id", id);

            var usuario = await _context.Usuarios
                .FromSqlRaw(sql, p)
                .FirstOrDefaultAsync();

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // EDITAR (POST)
        [HttpPost]
        public async Task<IActionResult> Editar(Usuario usuario)
        {
            var sql = @"UPDATE Usuarios SET
                           NombreUsuario = @NombreUsuario,
                           Correo = @Correo,
                           IdPerfil = @IdPerfil,
                           Estado = @Estado
                        WHERE IdUsuario = @IdUsuario";

            var p1 = new SqlParameter("@NombreUsuario", usuario.NombreUsuario);
            var p2 = new SqlParameter("@Correo", usuario.Correo);
            var p3 = new SqlParameter("@IdPerfil", usuario.IdPerfil);
            var p4 = new SqlParameter("@Estado", usuario.Estado);
            var p5 = new SqlParameter("@IdUsuario", usuario.IdUsuario);

            await _context.Database.ExecuteSqlRawAsync(sql, p1, p2, p3, p4, p5);

            return RedirectToAction("Index");
        }

        // DESACTIVAR
        public async Task<IActionResult> Desactivar(int id)
        {
            var sql = @"UPDATE Usuarios SET Estado = 0 WHERE IdUsuario = @Id";
            var p = new SqlParameter("@Id", id);

            await _context.Database.ExecuteSqlRawAsync(sql, p);

            return RedirectToAction("Index");
        }

        // ACTIVAR
        public async Task<IActionResult> Activar(int id)
        {
            var sql = @"UPDATE Usuarios SET Estado = 1 WHERE IdUsuario = @Id";
            var p = new SqlParameter("@Id", id);

            await _context.Database.ExecuteSqlRawAsync(sql, p);

            return RedirectToAction("Index");
        }
    }
}
