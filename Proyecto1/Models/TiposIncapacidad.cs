using System.ComponentModel.DataAnnotations;

namespace Proyecto1.Models
{
    public class TiposIncapacidad
    {
        [Key]
        public int IdTipoIncapacidad { get; set; }

        [Required, StringLength(20)]
        public string Codigo { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Entidad { get; set; } = string.Empty;

        [Required]
        public bool EsRemunerada { get; set; }

        [StringLength(200)]
        public string? DescripcionPago { get; set; }

        [Required]
        public bool Estado { get; set; } = true;
    }
}
