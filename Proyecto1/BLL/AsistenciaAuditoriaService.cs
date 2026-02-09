using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Proyecto1.DAL;
using Proyecto1.Models;


namespace Proyecto1.Services
{
    public class AsistenciaAuditoriaService
    {
        private readonly ApplicationDbContext _context;

        public AsistenciaAuditoriaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AsistenciaAuditoria>> ListarAsync()
        {
            return await _context.AsistenciaAuditoria
                .FromSqlRaw("SELECT * FROM AsistenciaAuditoria ORDER BY FechaModificacion DESC")
                .ToListAsync();
        }

        public async Task<List<AsistenciaAuditoria>> ObtenerPorColaboradorAsync(int idColaborador)
        {
            return await _context.AsistenciaAuditoria
                .FromSqlRaw(
                    "SELECT * FROM AsistenciaAuditoria WHERE IdColaborador = @Id ORDER BY FechaModificacion DESC",
                    new SqlParameter("@Id", idColaborador)
                )
                .ToListAsync();
        }
    }
}
