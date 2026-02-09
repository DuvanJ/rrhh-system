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
    }
}
