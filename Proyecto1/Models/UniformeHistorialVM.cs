using System;

namespace Proyecto1.Models
{
    public class UniformeHistorialVM
    {
        public int IdEntrega { get; set; }

        public int IdColaborador { get; set; }
        public string NombreColaborador { get; set; }
        public string NombreTipo { get; set; }
        public string Talla { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaEntrega { get; set; }
        public decimal? Costo { get; set; }
        public bool CobrarEnPlanilla { get; set; }
        public string? Observaciones { get; set; }
    }
}
