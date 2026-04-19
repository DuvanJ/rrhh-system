using System.ComponentModel.DataAnnotations;

namespace Proyecto1.Models
{
    public class CatTramosRenta
    {
        [Key]
        public int IdTramoRenta { get; set; }
        public decimal LimiteInferior { get; set; }
        public decimal? LimiteSuperior { get; set; }
        public decimal Porcentaje { get; set; }
        public bool Estado { get; set; } = true;
    }
}
