namespace Proyecto1.Models
{
    public class PlanillaDetalleVM
    {
        public int IdPlanillaDetalle { get; set; }
        public int IdPlanilla { get; set; }
        public int IdColaborador { get; set; }
        public string NombreColaborador { get; set; }
        public decimal SalarioBase { get; set; }
        public int DiasLaborados { get; set; }
        public decimal HorasOrdinarias { get; set; }
        public decimal HorasExtras { get; set; }
        public decimal MontoHorasExtras { get; set; }
        public decimal Deducciones { get; set; }
        public decimal SalarioBruto { get; set; }
        public decimal SalarioNeto { get; set; }
        public decimal MontoUniformes { get; set; }
        public decimal? HorasFeriado { get; set; }
        public decimal? MontoFeriado { get; set; }
    }
}