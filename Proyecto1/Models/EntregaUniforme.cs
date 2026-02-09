using System;
using System.ComponentModel.DataAnnotations;

namespace Proyecto1.Models
{
    public class EntregaUniforme
    {
        [Key]
        public int IdEntrega { get; set; }

        [Required]
        public int IdColaborador { get; set; }

        [Required]
        public int IdTipoUniforme { get; set; }

        [Required]
        public string Talla { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public DateTime FechaEntrega { get; set; }

        public decimal? Costo { get; set; }
        public bool CobrarEnPlanilla { get; set; }
        public string? Observaciones { get; set; }
    }
}
