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
    public class EvaluacionService
    {
        private readonly ApplicationDbContext _context;

        public EvaluacionService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= LISTAR COLABORADORES =================
        public async Task<List<ColaboradorVM>> ListarColaboradoresAsync()
        {
            return await _context.ColaboradoresEvaluacion
                .FromSqlRaw("EXEC sp_Evaluacion_ListarColaboradores")
                .AsNoTracking()
                .ToListAsync();
        }


        // ================= LISTAR CRITERIOS =================
        public async Task<List<CriterioEvaluacion>> ListarCriteriosAsync()
        {
            return await _context.CriteriosEvaluacion
                .FromSqlRaw("EXEC sp_Evaluacion_ListarCriterios")
                .AsNoTracking()
                .ToListAsync();
        }

        // ================= CREAR EVALUACIÓN =================
        public async Task<int> CrearEvaluacionAsync(Evaluacion evaluacion, List<EvaluacionDetalle> detalles)
        {
            if (evaluacion == null || detalles == null || detalles.Count == 0)
                throw new ArgumentException("Debe proporcionar la evaluación y al menos un detalle.");

            evaluacion.PuntajeTotal = detalles.Sum(d => d.Puntaje);

            // Parámetros del SP
            var paramIdCol = new SqlParameter("@IdColaborador", evaluacion.IdColaborador);
            var paramFecha = new SqlParameter("@FechaEvaluacion", evaluacion.FechaEvaluacion);
            var paramCom = new SqlParameter("@ComentariosGenerales",
                (object?)evaluacion.ComentariosGenerales ?? DBNull.Value);
            var paramTotal = new SqlParameter("@PuntajeTotal", evaluacion.PuntajeTotal);

            // Parámetro OUTPUT para recibir el IdEvaluacion
            var paramIdEvaluacion = new SqlParameter("@IdEvaluacion", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            // Ejecutar SP principal
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Evaluacion_Insertar @IdColaborador, @FechaEvaluacion, @ComentariosGenerales, @PuntajeTotal, @IdEvaluacion OUTPUT",
                paramIdCol, paramFecha, paramCom, paramTotal, paramIdEvaluacion
            );

            int idEvaluacion = (int)paramIdEvaluacion.Value;

            // Insertar cada detalle
            foreach (var d in detalles)
            {
                var p1 = new SqlParameter("@IdEvaluacion", idEvaluacion);
                var p2 = new SqlParameter("@IdCriterio", d.IdCriterio);
                var p3 = new SqlParameter("@Puntaje", d.Puntaje);
                var p4 = new SqlParameter("@Observacion",
                    (object?)d.Observacion ?? DBNull.Value);

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_Evaluacion_InsertarDetalle @IdEvaluacion, @IdCriterio, @Puntaje, @Observacion",
                    p1, p2, p3, p4
                );
            }

            return idEvaluacion;
        }


        // ================= LISTAR EVALUACIONES POR COLABORADOR =================
        public async Task<List<EvaluacionVM>> ListarEvaluacionesPorColaboradorAsync(int idColaborador)
        {
            var param = new SqlParameter("@IdColaborador", idColaborador);

            return await _context.Set<EvaluacionVM>()
                .FromSqlRaw("EXEC sp_Evaluacion_ListarPorColaborador @IdColaborador", param)
                .AsNoTracking()
                .ToListAsync();
        }

        // ================= VALIDAR COLABORADOR =================
        public async Task<bool> ColaboradorActivoAsync(int idColaborador)
        {
            return await _context.Colaboradores
                .AnyAsync(c => c.IdColaborador == idColaborador && c.Estado);
        }


        // ================= OBTENER COLABORADOR ACTIVO =================
        public async Task<Colaborador> ObtenerColaboradorActivoAsync(int id)
        {
            return await _context.Colaboradores
                .FirstOrDefaultAsync(c => c.IdColaborador == id && c.Estado);
        }


        public async Task<List<EvaluacionVM>> ListarEvaluacionesAsync()
        {
            return await _context.Set<EvaluacionVM>()
                .FromSqlRaw("EXEC sp_Evaluacion_Listar")
                .AsNoTracking()
                .ToListAsync();
        }



        // ================= OBTENER EVALUACION POR ID =================
        public async Task<EvaluacionVM> ObtenerEvaluacionPorIdAsync(int idEvaluacion)
        {
            var param = new SqlParameter("@IdEvaluacion", idEvaluacion);

            // ===== CABECERA =====
            var evaluacion = (await _context.Set<EvaluacionVM>()
                .FromSqlRaw("EXEC sp_Evaluacion_ObtenerPorId @IdEvaluacion", param)
                .AsNoTracking()
                .ToListAsync())
                .FirstOrDefault();

            if (evaluacion == null)
                return null;

            // ===== DETALLES =====
            var detalles = await _context.Set<DetalleEvaluacionVM>()
                .FromSqlRaw("EXEC sp_Evaluacion_ObtenerDetalles @IdEvaluacion", param)
                .AsNoTracking()
                .ToListAsync();

            evaluacion.Detalles = detalles;

            return evaluacion;
        }






        public async Task ActualizarEvaluacionAsync(EvaluacionVM vm)
        {
            using var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            // ===== Actualizar cabecera =====
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "sp_Evaluacion_Actualizar";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@IdEvaluacion", vm.IdEvaluacion));
                cmd.Parameters.Add(new SqlParameter("@ComentariosGenerales", vm.ComentariosGenerales ?? ""));
                cmd.Parameters.Add(new SqlParameter("@PuntajeTotal", vm.PuntajeTotal ?? 0));

                await cmd.ExecuteNonQueryAsync();
            }

            // ===== Actualizar detalles =====
            foreach (var d in vm.Detalles)
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "sp_Evaluacion_ActualizarDetalle";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@IdEvaluacion", vm.IdEvaluacion));
                cmd.Parameters.Add(new SqlParameter("@IdCriterio", d.IdCriterio));
                cmd.Parameters.Add(new SqlParameter("@Puntaje", d.Puntaje ?? 0));
                cmd.Parameters.Add(new SqlParameter("@Observacion", d.Observacion ?? ""));

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task EliminarEvaluacionAsync(int idEvaluacion)
        {
            var param = new SqlParameter("@IdEvaluacion", idEvaluacion);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Evaluacion_Eliminar @IdEvaluacion",
                param
            );
        }







    }
}


