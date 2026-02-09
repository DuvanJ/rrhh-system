namespace Proyecto1.Models
{
    public class PermisoColaboradorVM
    {
        public int IdPermiso { get; set; }
        public string TipoPermiso { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
    }
}
