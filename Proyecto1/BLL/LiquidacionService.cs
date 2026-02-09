using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Proyecto1.DAL;
using Proyecto1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto1.BLL
{
    public class LiquidacionService
    {
        private readonly ApplicationDbContext _context;

        public LiquidacionService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // ================= LISTAR =================
        public async Task<List<LiquidacionVM>> ListarAsync()
        {
            return await _context.Set<LiquidacionVM>()
                .FromSqlRaw("EXEC sp_Liquidacion_Listar")
                .AsNoTracking()
                .ToListAsync();
        }

        // ================= EXISTE LIQUIDACIÓN =================
        public async Task<bool> ExisteLiquidacionAsync(int idColaborador)
        {
            // Evitar duplicados: solo permite liquidación si no hay pendiente/aprobada
            return await _context.Liquidaciones
                .AnyAsync(l => l.IdColaborador == idColaborador &&
                               (l.Estado == "Pendiente" || l.Estado == "Aprobada"));
        }

        // ================= CALCULAR / GENERAR =================
        public async Task CalcularAsync(int idColaborador, int idTipoSalida, DateTime fechaSalida, string usuarioRegistro)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_Liquidacion_Generar @IdColaborador, @IdTipoSalida, @FechaSalida, @UsuarioRegistro",
                    new SqlParameter("@IdColaborador", idColaborador),
                    new SqlParameter("@IdTipoSalida", idTipoSalida),
                    new SqlParameter("@FechaSalida", fechaSalida),
                    new SqlParameter("@UsuarioRegistro", usuarioRegistro)
                );
            }
            catch (SqlException ex)
            {
                // RAISERROR en SQL Server retorna error Number = 50000 por defecto
                if (ex.Number == 50000)
                {
                    throw new Exception(ex.Message); // Mensaje específico del SP
                }
                throw; // otros errores
            }
        }


        // ================= LIQUIDACIÓN PENDIENTE =================
        public async Task<bool> LiquidacionPendienteAsync(int idColaborador)
        {
            return await _context.Liquidaciones
                .AnyAsync(l => l.IdColaborador == idColaborador && l.Estado == "Pendiente");
        }




        // ================= COLABORADOR ACTIVO =================
        public async Task<bool> ColaboradorActivoAsync(int idColaborador)
        {
            return await _context.Colaboradores
                .AnyAsync(c => c.IdColaborador == idColaborador && c.Estado);
        }

        // ================= COLABORADOR EXISTE =================
        public async Task<bool> ColaboradorExisteAsync(int idColaborador)
        {
            return await _context.Colaboradores
                .AnyAsync(c => c.IdColaborador == idColaborador);
        }

        // ================= OBTENER INFO =================
        public async Task<Colaborador?> ObtenerInfoAsync(int idColaborador)
        {
            return await _context.Colaboradores
                .Where(c => c.IdColaborador == idColaborador && c.Estado)
                .FirstOrDefaultAsync();
        }

        // ================= DETALLE =================
        public async Task<List<LiquidacionDetalleVM>> ObtenerDetalleAsync(int idLiquidacion)
        {
            return await _context.Set<LiquidacionDetalleVM>()
                .FromSqlRaw("EXEC sp_Liquidacion_Detalle @IdLiquidacion",
                    new SqlParameter("@IdLiquidacion", idLiquidacion))
                .AsNoTracking()
                .ToListAsync();
        }

        // ================= APROBAR =================
        public async Task AprobarAsync(int idLiquidacion)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Liquidacion_Aprobar @IdLiquidacion",
                new SqlParameter("@IdLiquidacion", idLiquidacion)
            );
        }

        // ================= PAGAR =================
        public async Task PagarAsync(int idLiquidacion)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Liquidacion_Pagar @IdLiquidacion",
                new SqlParameter("@IdLiquidacion", idLiquidacion)
            );
        }

        // ================= TIPO SALIDA =================
        public async Task<List<TipoSalidaVM>> ListarTiposSalidaAsync()
        {
            return await _context.Set<TipoSalidaVM>()
                .FromSqlRaw("SELECT IdTipoSalida, Nombre FROM Cat_TipoSalida")
                .AsNoTracking()
                .ToListAsync();
        }
    }
}

