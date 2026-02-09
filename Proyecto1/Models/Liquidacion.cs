using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto1.Models
{
    public class Liquidacion
    {
        [Key]
        public int IdLiquidacion { get; set; }   // PK
        public int IdColaborador { get; set; }
        public int IdTipoSalida { get; set; }
        public DateTime FechaSalida { get; set; }
        public decimal MontoTotal { get; set; }
        public string Estado { get; set; }

        // Campos extra para vistas (JOIN del SP)
        public string Nombre { get; set; }       // Nombre + Apellidos
        public string TipoSalida { get; set; }   // Cat_TipoSalida.Nombre
    }
}

