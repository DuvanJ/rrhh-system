using System.ComponentModel.DataAnnotations;

namespace Proyecto1.Models
{
    public class PlanillaDetalleDeduccion
    {
        [Key]
        public int IdDetalleDeduccion { get; set; }
        public int IdPlanillaDetalle { get; set; }
        public int IdDeduccion { get; set; }
        public decimal PorcentajeAplicadoPatrono { get; set; }
        public decimal PorcentajeAplicadoTrabajador { get; set; }
        public decimal MontoPatrono { get; set; }
        public decimal MontoTrabajador { get; set; }
    }
}
