using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto1.Models
{
    public class HoraExtra
    {
        [Key]
        public int IdHoraExtra { get; set; }
        public int IdColaborador { get; set; }
        public DateTime Fecha { get; set; }
        public decimal CantidadHoras { get; set; }
        public string? Motivo { get; set; }
        public string? Estado { get; set; } // Pendiente, Aprobada, Rechazada

        // SOLO PARA MOSTRAR
        [NotMapped]
        public string NombreColaborador { get; set; } = string.Empty;
    }
}
