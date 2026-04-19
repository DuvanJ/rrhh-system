namespace Proyecto1.Models
{
    public class ConsultaPlanillaVM
    {
        public int IdPlanilla { get; set; }
        public int IdColaborador { get; set; }
        public string NombreColaborador { get; set; } = "";

        public int Mes { get; set; }
        public int Año { get; set; }

        public decimal SalarioBruto { get; set; }
        public decimal SalarioNeto { get; set; }
        public decimal Deducciones { get; set; }
        
        public int DiasLaborados { get; set; }
        public int DiasVacaciones { get; set; }
        public int DiasIncapacidad { get; set; }
        public int TotalDiasConGoce { get; set; }
        public int TotalDiasSinGoce { get; set; }
        public decimal? MontoRebajoAsistencia { get; set; }
        public bool? AlertaDiasIncompletos { get; set; }

        // Listas opcionales de detalle
        public List<Permiso>? Permisos { get; set; }
        public List<Vacacion>? Vacaciones { get; set; }
        public List<Asistencia>? Asistencias { get; set; }
    }
}
