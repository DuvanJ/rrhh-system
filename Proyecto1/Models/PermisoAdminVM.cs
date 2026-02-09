namespace Proyecto1.Models
{
    public class PermisoAdminVM
    {
        public int IdPermiso { get; set; }
        public int IdColaborador { get; set; }
        public string NombreColaborador { get; set; } = string.Empty;
        public string TipoPermiso { get; set; } = string.Empty;
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
    }
}
