using System.ComponentModel.DataAnnotations;

namespace Proyecto1.Models
{
    public class IncapacidadCreateVM
    {
        [Required]
        public int IdColaborador { get; set; }

        [Required]
        public int IdTipoIncapacidad { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        public string? Motivo { get; set; }

        public IFormFile? Documento { get; set; }
    }
}
