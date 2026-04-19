namespace Proyecto1.Models
{
    public class PermisoDetalleVM
    {
        public string NombreColaborador { get; set; } = string.Empty;
        public int IdColaborador { get; set; }
        public string CorreoColaborador { get; set; } = string.Empty;
        public string TipoPermiso { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
