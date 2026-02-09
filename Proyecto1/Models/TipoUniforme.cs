using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto1.Models
{
    [Table("Cat_TiposUniforme")]
    public class TipoUniforme
    {
        [Key]
        public int IdTipoUniforme { get; set; }

        [Required]
        public string NombreTipo { get; set; }

        public string? Descripcion { get; set; }

        // Reglas de asignación
        public bool EntregaAutomatica { get; set; }
        public int? MesesParaEntrega { get; set; }
        public int? CantidadPorEntrega { get; set; }
        public int? FrecuenciaMeses { get; set; }

        public bool EsOpcional { get; set; }
        public bool PermiteCobro { get; set; }

        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
