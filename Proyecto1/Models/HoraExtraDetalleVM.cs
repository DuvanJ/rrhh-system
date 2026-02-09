namespace Proyecto1.Models
{
    public class HoraExtraDetalleVM
    {
        public string NombreColaborador { get; set; } = string.Empty;
        public string CorreoColaborador { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public decimal CantidadHoras { get; set; }
    }
}
