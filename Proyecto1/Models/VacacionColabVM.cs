namespace Proyecto1.Models
{
    public class VacacionColabVM
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int DiasSolicitados { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
