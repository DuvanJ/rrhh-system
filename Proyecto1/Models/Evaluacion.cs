using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proyecto1.Models
{
    public class Evaluacion
    {
        [Key]
        public int IdEvaluacion { get; set; }
        public int IdColaborador { get; set; }
        public DateTime FechaEvaluacion { get; set; }
        public string? ComentariosGenerales { get; set; }
        public decimal PuntajeTotal { get; set; }

        // Relación
        public List<EvaluacionDetalle>? Detalles { get; set; }
    }
}
