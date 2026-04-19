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


        [NotMapped]
        public string NombreColaborador { get; set; } = string.Empty;

        public decimal SalarioBase { get; set; }
        public int DiasLaborados { get; set; }
        public decimal HorasOrdinarias { get; set; }
        public decimal HorasExtras { get; set; }
        public decimal MontoHorasExtras { get; set; }

        public decimal Deducciones { get; set; }
        public decimal SalarioBruto { get; set; }
        public decimal SalarioNeto { get; set; }


        public int DiasIncapacidad { get; set; }
        public decimal MontoIncapacidadPatrono { get; set; }
        public decimal MontoIncapacidadCCSS { get; set; }
        public decimal MontoIncapacidadINS { get; set; }

        public decimal MontoRebajoIncapacidad { get; set; }

        public decimal MontoUniformes { get; set; }

        public decimal MontoRebajoAsistencia { get; set; }

        public decimal MontoRebajoSinGoce { get; set; }

        public bool AlertaDiasIncompletos { get; set; }

        public decimal? HorasFeriado { get; set; }
        public decimal? MontoFeriado { get; set; }


    }
}
