using System.ComponentModel.DataAnnotations;

namespace Proyecto1.Models
{
    public class LiquidacionDetalle
    {
        [Key]
        public int IdDetalle { get; set; }
        public int IdLiquidacion { get; set; }
        public string Rubro { get; set; }
        public decimal Monto { get; set; }
    }
}
