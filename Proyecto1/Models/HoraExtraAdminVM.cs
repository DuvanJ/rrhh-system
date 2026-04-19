namespace Proyecto1.Models
{
    public class HoraExtraAdminVM
    {
        public int IdHoraExtra { get; set; }
        public int IdColaborador { get; set; }
        public string NombreColaborador { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public decimal CantidadHoras { get; set; }
        public string? Motivo { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}