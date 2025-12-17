using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto1.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        public string Correo { get; set; } = string.Empty;

        [NotMapped]
        [Required]
        public string ContrasenaHash { get; set; } = string.Empty;

        [Required]
        public int IdPerfil { get; set; }

        public bool Estado { get; set; }

        // ✔ ESTE SÍ lo devuelve el SP → NO se marca como NotMapped
        public string NombrePerfil { get; set; } = string.Empty;
    }
}
