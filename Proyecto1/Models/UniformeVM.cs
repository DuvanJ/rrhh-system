using System;

namespace Proyecto1.Models
{
    public class UniformeVM
    {
        public int IdEntrega { get; set; }
        public int IdColaborador { get; set; }
        public string Nombre { get; set; }         // nombre del colaborador
        public string TipoUniforme { get; set; }   // nombre del uniforme
        public string Talla { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaEntrega { get; set; }
        public decimal? Costo { get; set; }
        public bool CobrarEnPlanilla { get; set; }
        public string? Observaciones { get; set; }
    }
}
