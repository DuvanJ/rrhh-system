using System.ComponentModel.DataAnnotations;

namespace Proyecto1.Models
{
    public class EvaluacionDetalle
    {
        [Key]
        public int IdEvaluacionDetalle { get; set; }
        public int IdEvaluacion { get; set; }
        public int IdCriterio { get; set; }
        public decimal Puntaje { get; set; }
        public string? Observacion { get; set; }

        // Relación
        public CriterioEvaluacion? Criterio { get; set; }
    }
}
