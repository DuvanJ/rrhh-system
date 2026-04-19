using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto1.Models
{
    public class Permiso
    {
        [Key]
        public int IdPermiso { get; set; }
        public int IdColaborador { get; set; }
        public string TipoPermiso { get; set; } = string.Empty;
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? Observaciones { get; set; }

        public bool EsConGoce { get; set; }

        // SOLO PARA VISTAS ADMIN
        [NotMapped]
        public string NombreColaborador { get; set; } = string.Empty;
    }
}
