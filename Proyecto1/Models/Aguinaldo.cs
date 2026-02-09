namespace Proyecto1.Models
{
    public class Aguinaldo
    {
        public int IdAguinaldo { get; set; }
        public int IdColaborador { get; set; }
        public int Año { get; set; }
        public decimal TotalSalarios { get; set; }
        public decimal MontoAguinaldo { get; set; }
        public int? IdEstadoAguinaldo { get; set; }
        public DateTime? FechaCalculo { get; set; }

        public string? NombreColaborador { get; set; } // Para vistas
        public string? Estado { get; set; }
    }
}
