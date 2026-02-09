namespace Proyecto1.Models
{
    public class VacacionDetalleVM
    {
        public int IdVacacion { get; set; }
        public int IdColaborador { get; set; } // 🔴 NECESARIO
        public string NombreColaborador { get; set; } = string.Empty;
        public string CorreoColaborador { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int DiasSolicitados { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
