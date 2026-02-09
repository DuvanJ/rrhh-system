namespace Proyecto1.Models
{
    public class LiquidacionVM
    {
        public int IdLiquidacion { get; set; }

        public int IdColaborador { get; set; }
        public string Colaborador { get; set; }
        public string TipoSalida { get; set; }
        public DateTime FechaSalida { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
    }
}
