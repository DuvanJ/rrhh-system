using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto1.Models
{
    public class Vacacion
    {
        [Key]
        public int IdVacacion { get; set; }
        public int IdColaborador { get; set; }
        public DateTime FechaSolicitud { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }
        public int DiasSolicitados { get; set; }
        public string Estado { get; set; } = string.Empty;

        [NotMapped]
        public int DiasDisponibles { get; set; }
        
    }
}
