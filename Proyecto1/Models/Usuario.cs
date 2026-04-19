using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto1.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El colaborador es obligatorio")]
        public int IdColaborador { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [StringLength(80, ErrorMessage = "El nombre no puede superar 80 caracteres")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo no es valido")]
        [StringLength(120, ErrorMessage = "El correo no puede superar 120 caracteres")]
        public string Correo { get; set; } = string.Empty;

        [StringLength(255)]
        public string ContrasenaHash { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un perfil")]
        public int IdPerfil { get; set; }

        public bool Estado { get; set; }

        [NotMapped]
        public string NombrePerfil { get; set; } = string.Empty;
    }
}