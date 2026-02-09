using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Proyecto1.DAL;
using Proyecto1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Proyecto1.BLL
{
    public class UniformeService
    {
        private readonly ApplicationDbContext _context;

        public UniformeService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= LISTAR HISTORIAL =================
        public async Task<List<UniformeHistorialVM>> ListarHistorialAsync(int? idColaborador)
        {
            var query = @"
        SELECT 
            e.IdEntrega,
            e.IdColaborador,
            c.Nombre AS NombreColaborador,
            t.NombreTipo,
            e.Talla,
            e.Cantidad,
            e.FechaEntrega,
            e.Costo,
            e.CobrarEnPlanilla,
            e.Observaciones
        FROM EntregasUniforme e
        INNER JOIN Cat_TiposUniforme t 
            ON e.IdTipoUniforme = t.IdTipoUniforme
        INNER JOIN Colaboradores c 
            ON e.IdColaborador = c.IdColaborador
    ";

            if (idColaborador.HasValue)
            {
                query += " WHERE e.IdColaborador = @idColaborador";
            }

            query += " ORDER BY e.FechaEntrega DESC";

            var param = new SqlParameter("@idColaborador", idColaborador ?? (object)DBNull.Value);

            if (idColaborador.HasValue)
            {
                return await _context.Set<UniformeHistorialVM>()
                    .FromSqlRaw(query, param)
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {
                return await _context.Set<UniformeHistorialVM>()
                    .FromSqlRaw(query)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }



        // ================= LISTAR TIPOS DE UNIFORME ACTIVOS =================
        public async Task<List<TipoUniforme>> ListarTiposAsync()
        {
            return await _context.TiposUniforme
                .Where(t => t.Activo)
                .AsNoTracking()
                .ToListAsync();
        }

        // ================= OBTENER ENTREGA POR ID =================
        public async Task<EntregaUniforme> ObtenerPorIdAsync(int id)
        {
            return await _context.EntregasUniforme
                .FirstOrDefaultAsync(e => e.IdEntrega == id);
        }

        // ================= CREAR NUEVA ENTREGA =================
        public async Task<bool> CrearAsync(EntregaUniforme model)
        {
            // Validar colaborador activo
            var colaborador = await _context.Colaboradores
                .FirstOrDefaultAsync(c => c.IdColaborador == model.IdColaborador && c.Estado);

            if (colaborador == null)
                return false;

            _context.EntregasUniforme.Add(model);
            await _context.SaveChangesAsync();
            return true;
        }

        // ================= ACTUALIZAR ENTREGA =================
        public async Task ActualizarAsync(EntregaUniforme model)
        {
            _context.EntregasUniforme.Update(model);
            await _context.SaveChangesAsync();
        }

        // ================= ELIMINAR ENTREGA =================
        public async Task EliminarAsync(int id)
        {
            var entrega = await _context.EntregasUniforme
                .FirstOrDefaultAsync(x => x.IdEntrega == id);

            if (entrega != null)
            {
                _context.EntregasUniforme.Remove(entrega);
                await _context.SaveChangesAsync();
            }
        }

        // ================= OBTENER COLABORADOR ACTIVO =================
        public async Task<Colaborador> ObtenerColaboradorActivoAsync(int id)
        {
            return await _context.Colaboradores
                .FirstOrDefaultAsync(c => c.IdColaborador == id && c.Estado);
        }


        public async Task CrearMultiplesAsync(List<EntregaUniforme> uniformes)
        {
            _context.EntregasUniforme.AddRange(uniformes);
            await _context.SaveChangesAsync();
        }


        // ================= LISTAR COLABORADORES ACTIVOS =================
        public async Task<List<Colaborador>> ListarColaboradoresAsync()
        {
            return await _context.Colaboradores
                .Where(c => c.Estado)
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }


        // ================= VALIDAR SI COLABORADOR EXISTE =================
        public async Task<bool> ColaboradorExisteAsync(int idColaborador)
        {
            return await _context.Colaboradores
                .AnyAsync(c => c.IdColaborador == idColaborador);
        }


    }
}
