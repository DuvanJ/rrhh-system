using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto1.Models
{
    public class PlanillaDetalle
    {
        [Key]
        public int IdPlanillaDetalle { get; set; }

        public int IdPlanilla { get; set; }
        public int IdColaborador { get; set; }

        
        

        public decimal SalarioBase { get; set; }
        public int DiasLaborados { get; set; }
        public decimal HorasOrdinarias { get; set; }
        public decimal HorasExtras { get; set; }
        public decimal MontoHorasExtras { get; set; }

        public decimal Deducciones { get; set; }
        public decimal SalarioBruto { get; set; }
        public decimal SalarioNeto { get; set; }
    }
}
