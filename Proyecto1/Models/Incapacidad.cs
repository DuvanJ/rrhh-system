using System.ComponentModel.DataAnnotations;

namespace Proyecto1.Models
{
    public class Incapacidad
    {
        [Key]
        public int IdIncapacidad { get; set; }

        [Required]
        public int IdColaborador { get; set; }

        // CCSS-EC, INS-RT, CCSS-MAT, etc.
        [Required]
        [StringLength(50)]
        public string TipoIncapacidad { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        public int DiasIncapacidad { get; set; }

        // true solo para casos especiales (ej: maternidad)
        public bool EsRemunerada { get; set; }

        [StringLength(200)]
        public string? Motivo { get; set; }

        [StringLength(250)]
        public string? DocumentoAdjunto { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [StringLength(20)]
        public string Estado { get; set; } = "Activa";

        [StringLength(10)]
        public string Entidad { get; set; } = string.Empty;

    }
}
