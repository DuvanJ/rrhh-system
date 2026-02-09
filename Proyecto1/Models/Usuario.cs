using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto1.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        public int IdColaborador { get; set; }

        [Required]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        public string Correo { get; set; } = string.Empty;

        
        public string ContrasenaHash { get; set; } = string.Empty;

        [Required]
        public int IdPerfil { get; set; }

        public bool Estado { get; set; }

        // SOLO para mostrar (JOIN / SP)
        [NotMapped]
        public string NombrePerfil { get; set; } = string.Empty;
    }
}
