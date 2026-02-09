namespace Proyecto1.Models
{
    public class VacacionAdminVM
    {
        public int IdVacacion { get; set; }
        public int IdColaborador { get; set; }
        public string NombreColaborador { get; set; } = string.Empty;
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int DiasSolicitados { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
