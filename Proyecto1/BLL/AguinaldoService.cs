using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Proyecto1.DAL;
using Proyecto1.Models;
using System.Data;

namespace Proyecto1.BLL
{
    public class AguinaldoService
    {
        private readonly ApplicationDbContext _context;

        public AguinaldoService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= CALCULAR =================
        public async Task CalcularAsync(int año)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Aguinaldo_Calcular @Año",
                new SqlParameter("@Año", año)
            );
        }

        // ================= APROBAR =================
        public async Task AprobarAsync(int idAguinaldo)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Aguinaldo_Aprobar @IdAguinaldo",
                new SqlParameter("@IdAguinaldo", idAguinaldo)
            );
        }

        // ================= PAGAR =================
        public async Task PagarAsync(int idAguinaldo)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Aguinaldo_Pagar @IdAguinaldo",
                new SqlParameter("@IdAguinaldo", idAguinaldo)
            );
        }

        // ================= LISTAR =================
        public async Task<List<Aguinaldo>> ListarAsync()
        {
            return await _context.Aguinaldos
                .FromSqlRaw("EXEC sp_Aguinaldo_Listar")
                .ToListAsync();
        }

        public async Task<List<Aguinaldo>> ListarPorAnioAsync(int anio)
        {
            return await _context.Aguinaldos
                .FromSqlRaw("EXEC sp_Aguinaldo_ListarPorAnio @Año={0}", anio)
                .ToListAsync();
        }

        // ================= EXISTE AGUINALDO =================
        public async Task<bool> ExisteAguinaldoAsync(int anio)
        {
            var conn = _context.Database.GetDbConnection();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "sp_Aguinaldo_Existe";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Año", anio));

            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync(); // 🔒 solo si está cerrada

            var result = (int)await cmd.ExecuteScalarAsync();
            return result > 0;
        }





        // ================= HAY SALARIOS =================
        public async Task<bool> HaySalariosAsync(int anio)
        {
            var conn = _context.Database.GetDbConnection();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "sp_Aguinaldo_HayDatos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Año", anio));

            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            var result = (int)await cmd.ExecuteScalarAsync();
            return result > 0;
        }



    }
}
