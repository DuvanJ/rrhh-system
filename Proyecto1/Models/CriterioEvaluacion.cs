using System.ComponentModel.DataAnnotations;

namespace Proyecto1.Models
{
    public class CriterioEvaluacion
    {
        [Key]
        public int IdCriterio { get; set; }
        public string NombreCriterio { get; set; }
        public string? Descripcion { get; set; }
        public bool Estado { get; set; } = true;
    }
}
