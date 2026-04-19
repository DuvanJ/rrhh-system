using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Proyecto1.DAL;
using Proyecto1.Models;

namespace Proyecto1.Services
{
    public class ColaboradoresService
    {
        private readonly ApplicationDbContext _context;

        public ColaboradoresService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Colaborador>> ObtenerColaboradores()
        {
            return await _context.Colaboradores
                .FromSqlRaw("EXEC sp_ObtenerColaboradores")
                .ToListAsync();
        }

        public async Task<Colaborador?> ObtenerPorId(int id)
        {
            var p = new SqlParameter("@IdColaborador", id);

            var result = await _context.Colaboradores
                .FromSqlRaw("EXEC sp_ObtenerColaboradorPorID @IdColaborador", p)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task Insertar(Colaborador c)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_InsertarColaborador @Cedula, @Nombre, @Apellidos, @Puesto, @Departamento, @FechaIngreso, @SalarioBase, @Correo",
                new SqlParameter("@Cedula", c.Cedula),
                new SqlParameter("@Nombre", c.Nombre),
                new SqlParameter("@Apellidos", c.Apellidos),
                new SqlParameter("@Puesto", c.Puesto),
                new SqlParameter("@Departamento", c.Departamento),
                new SqlParameter("@FechaIngreso", c.FechaIngreso),
                new SqlParameter("@SalarioBase", c.SalarioBase),
                new SqlParameter("@Correo", (object?)c.Correo ?? DBNull.Value)
            );
        }


        public async Task Actualizar(Colaborador c)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_ActualizarColaborador @IdColaborador, @Cedula, @Nombre, @Apellidos, @Puesto, @Departamento, @FechaIngreso, @SalarioBase, @Correo, @Estado",
                new SqlParameter("@IdColaborador", c.IdColaborador),
                new SqlParameter("@Cedula", c.Cedula),
                new SqlParameter("@Nombre", c.Nombre),
                new SqlParameter("@Apellidos", c.Apellidos),
                new SqlParameter("@Puesto", c.Puesto),
                new SqlParameter("@Departamento", c.Departamento),
                new SqlParameter("@FechaIngreso", c.FechaIngreso),
                new SqlParameter("@SalarioBase", c.SalarioBase),
                new SqlParameter("@Correo", (object?)c.Correo ?? DBNull.Value),
                new SqlParameter("@Estado", c.Estado)
            );
        }



        public async Task Eliminar(int id)
        {
            Console.WriteLine("ELIMINANDO ID: " + id);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_EliminarColaborador @IdColaborador",
                new SqlParameter("@IdColaborador", id)
            );
        }


        public async Task<Colaborador?> ObtenerInfoAsync(int id)
        {
            return (await _context.Colaboradores
                .FromSqlRaw("EXEC sp_ObtenerColaboradorPorID @IdColaborador",
                    new SqlParameter("@IdColaborador", id))
                .ToListAsync())
                .FirstOrDefault();
        }


        public async Task<List<Colaborador>> ObtenerColaboradoresConsulta()
        {
            return await _context.Colaboradores
                .FromSqlRaw("EXEC sp_ObtenerColaboradores")
                .ToListAsync();
        }


        public async Task<List<Colaborador>> ObtenerColaboradoresActivos()
        {
            return await _context.Colaboradores
                .FromSqlRaw("EXEC sp_ObtenerColaboradoresActivos")
                .ToListAsync();
        }

    }
}
