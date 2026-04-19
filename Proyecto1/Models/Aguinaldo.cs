using System.ComponentModel.DataAnnotations;

namespace Proyecto1.Models
{
    public class Aguinaldo
    {
        [Key]
        public int IdAguinaldo { get; set; }
        public int IdColaborador { get; set; }
        public int Año { get; set; }
        public decimal TotalSalarios { get; set; }
        public decimal MontoAguinaldo { get; set; }
        public int? IdEstadoAguinaldo { get; set; }
        public DateTime? FechaCalculo { get; set; }
        public DateTime? FechaPago { get; set; }

        public string? NombreColaborador { get; set; } // Para vistas
        public string? Estado { get; set; }
    }
}
