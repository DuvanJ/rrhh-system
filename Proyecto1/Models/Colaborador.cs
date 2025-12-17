using System.ComponentModel.DataAnnotations;

namespace Proyecto1.Models
{
    public class Colaborador
    {
        [Key]
        public int IdColaborador { get; set; }

        [Required]
        public string Cedula { get; set; } = string.Empty;

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Apellidos { get; set; } = string.Empty;

        public string Puesto { get; set; } = string.Empty;

        public string Departamento { get; set; } = string.Empty;

        [Required]
        public DateTime FechaIngreso { get; set; }

        [Required]
        public decimal SalarioBase { get; set; }

        public string? Correo { get; set; }

        public bool Estado { get; set; }
    }
}

