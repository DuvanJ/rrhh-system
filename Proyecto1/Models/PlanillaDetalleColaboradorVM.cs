using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto1.Models
{
    public class PlanillaDetalleColaboradorVM
    {
        // Datos generales
        public int IdPlanilla { get; set; }
        public int IdColaborador { get; set; }

        
        public string NombreColaborador { get; set; } = string.Empty;

        // Planilla
        public decimal SalarioBase { get; set; }
        public int DiasLaborados { get; set; }
        public decimal HorasOrdinarias { get; set; }
        public decimal HorasExtras { get; set; }
        public decimal MontoHorasExtras { get; set; }
        public decimal Deducciones { get; set; }
        public decimal SalarioBruto { get; set; }
        public decimal SalarioNeto { get; set; }

        // Detalles adicionales
        public List<Asistencia> Asistencias { get; set; } = new();
        public List<Permiso> Permisos { get; set; } = new();
        // después agregamos incapacidades y vacaciones

        public int DiasVacaciones { get; set; }

        public List<Vacacion> Vacaciones { get; set; } = new();


        public int DiasIncapacidad { get; set; }
        public decimal MontoIncapacidadPatrono { get; set; }
        public decimal MontoIncapacidadCCSS { get; set; }
        public decimal MontoIncapacidadINS { get; set; }

        public decimal MontoRebajoIncapacidad { get; set; }

        public decimal MontoUniformes { get; set; }


        public decimal DeduccionSalud { get; set; }
        public decimal DeduccionIVM { get; set; }

        public decimal DeduccionBancoPopular { get; set; }
        public decimal TotalDeduccionCCSS { get; set; }

        public decimal DeduccionRenta { get; set; }

        public decimal MontoRebajoAsistencia { get; set; }

        public decimal MontoRebajoSinGoce { get; set; }

        public int TotalDiasSinGoce { get; set; }
        public int TotalDiasConGoce { get; set; }

        public bool AlertaDiasIncompletos { get; set; }


        public decimal HorasFeriado { get; set; }
        public decimal MontoFeriado { get; set; }

    }
}
