namespace Proyecto1.Models
{
    public class IncapacidadAdminVM
    {
        public int IdIncapacidad { get; set; }
        public int IdColaborador { get; set; }

        public string NombreColaborador { get; set; } = string.Empty;

        public string TipoIncapacidad { get; set; } = string.Empty;

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public int DiasIncapacidad { get; set; }

        public bool EsRemunerada { get; set; }

        public string Estado { get; set; } = string.Empty;
    }
}
