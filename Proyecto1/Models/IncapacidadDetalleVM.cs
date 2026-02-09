namespace Proyecto1.Models
{
    public class IncapacidadDetalleVM
    {
        public int IdIncapacidad { get; set; }

        public string NombreColaborador { get; set; } = string.Empty;

        public string TipoIncapacidad { get; set; } = string.Empty;

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public int DiasIncapacidad { get; set; }

        public bool EsRemunerada { get; set; }

        public string? Motivo { get; set; }

        public string? DocumentoAdjunto { get; set; }

        public string Estado { get; set; } = string.Empty;

        public DateTime FechaRegistro { get; set; }
    }
}
