using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Proyecto1.DAL;
using Proyecto1.Models;

namespace Proyecto1.Services
{
    public class AsistenciaService
    {
        private readonly ApplicationDbContext _context;

        public AsistenciaService(ApplicationDbContext context)
        {
            _context = context;
        }

        // 📌 Registrar entrada o salida
        public async Task RegistrarAsync(int idColaborador, DateTime fecha, TimeSpan? horaEntrada, TimeSpan? horaSalida)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Asistencia_Registrar @IdColaborador, @Fecha, @HoraEntrada, @HoraSalida",
                new SqlParameter("@IdColaborador", idColaborador),
                new SqlParameter("@Fecha", fecha),
                new SqlParameter("@HoraEntrada", (object?)horaEntrada ?? DBNull.Value),
                new SqlParameter("@HoraSalida", (object?)horaSalida ?? DBNull.Value)
            );
        }

        // 📌 Consultar asistencia por colaborador
        public async Task<List<Asistencia>> ObtenerPorColaboradorAsync(int idColaborador)
        {
            var pId = new SqlParameter("@IdColaborador", idColaborador);
            var pInicio = new SqlParameter("@FechaInicio", DBNull.Value);
            var pFin = new SqlParameter("@FechaFin", DBNull.Value);

            return await _context.Asistencia
                .FromSqlRaw(
                    "EXEC sp_Asistencia_ObtenerPorColaborador @IdColaborador, @FechaInicio, @FechaFin",
                    pId, pInicio, pFin)
                .ToListAsync();
        }

        // 📌 Obtener asistencia por ID (para editar)
        public async Task<Asistencia?> ObtenerPorIdAsync(int id)
        {
            var pId = new SqlParameter("@IdAsistencia", id);

            var result = await _context.Asistencia
                .FromSqlRaw("EXEC sp_Asistencia_ObtenerPorId @IdAsistencia", pId)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        // 📌 Ajustar asistencia
        public async Task ActualizarAsync(Asistencia a)
        {
            var pId = new SqlParameter("@IdAsistencia", a.IdAsistencia);
            var pEntrada = new SqlParameter("@HoraEntrada", (object?)a.HoraEntrada ?? DBNull.Value);
            var pSalida = new SqlParameter("@HoraSalida", (object?)a.HoraSalida ?? DBNull.Value);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Asistencia_Actualizar @IdAsistencia, @HoraEntrada, @HoraSalida",
                pId, pEntrada, pSalida
            );
        }

        // 📌 Listado general (reportes / RRHH)
        public async Task<List<Asistencia>> ListarAsync()
        {
            var asistencias = await _context.Asistencia
                .FromSqlRaw("EXEC sp_Asistencia_Listar")
                .ToListAsync();

            // 🔹 Obtener todos los colaboradores de una sola vez
            var idColaboradores = asistencias.Select(a => a.IdColaborador).Distinct().ToList();
            var colaboradores = await _context.Colaboradores
                .Where(c => idColaboradores.Contains(c.IdColaborador))
                .ToDictionaryAsync(c => c.IdColaborador, c => c.Nombre);

            // 🔹 Asignar nombres
            foreach (var a in asistencias)
            {
                a.NombreColaborador = colaboradores.ContainsKey(a.IdColaborador)
                    ? colaboradores[a.IdColaborador]
                    : $"Colaborador #{a.IdColaborador}";
            }

            return asistencias;
        }

        //verificar existencia del colaborador
        public async Task<bool> ColaboradorExisteAsync(int idColaborador)
        {
            return await _context.Colaboradores
                .AnyAsync(c => c.IdColaborador == idColaborador);
        }

        //Ajustar Asistencia
        public async Task AjustarAsistenciaAsync(
            Asistencia nueva,
            string usuario,
            string motivo)
        {
            // 1️⃣ Auditoría
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Asistencia_AuditarAjuste @Id, @Usuario, @Entrada, @Salida, @Motivo",
                new SqlParameter("@Id", nueva.IdAsistencia),
                new SqlParameter("@Usuario", usuario),
                new SqlParameter("@Entrada", nueva.HoraEntrada ?? (object)DBNull.Value),
                new SqlParameter("@Salida", nueva.HoraSalida ?? (object)DBNull.Value),
                new SqlParameter("@Motivo", motivo)
            );

            // 2️⃣ Actualizar asistencia
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Asistencia_Actualizar @Id, @Entrada, @Salida",
                new SqlParameter("@Id", nueva.IdAsistencia),
                new SqlParameter("@Entrada", nueva.HoraEntrada ?? (object)DBNull.Value),
                new SqlParameter("@Salida", nueva.HoraSalida ?? (object)DBNull.Value)
            );
        }

        public async Task<List<AsistenciaReporteVM>> ListarReporteAsync()
        {
            return await _context.Set<AsistenciaReporteVM>()
                .FromSqlRaw("EXEC sp_Asistencia_Listar")
                .AsNoTracking()
                .ToListAsync();
        }

        // ================= AUDITORIA =================
        public async Task<List<AsistenciaAuditoria>> ListarAuditoriaAsync()
        {
            return await _context.AsistenciaAuditoria
                .FromSqlRaw("SELECT * FROM AsistenciaAuditoria ORDER BY FechaModificacion DESC")
                .ToListAsync();
        }

        public async Task<List<AsistenciaAuditoria>> ObtenerAuditoriaPorColaboradorAsync(int idColaborador)
        {
            return await _context.AsistenciaAuditoria
                .FromSqlRaw(
                    "SELECT * FROM AsistenciaAuditoria WHERE IdColaborador = @Id ORDER BY FechaModificacion DESC",
                    new SqlParameter("@Id", idColaborador))
                .ToListAsync();
        }
    }
}