using System.ComponentModel.DataAnnotations;

namespace Proyecto1.Models
{
    public class CatDeduccionObligatoria
    {
        [Key]
        public int IdDeduccion { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string TipoAporte { get; set; } = string.Empty; // Patrono / Trabajador / Ambos
        public decimal PorcentajePatrono { get; set; }
        public decimal PorcentajeTrabajador { get; set; }
    }
}
