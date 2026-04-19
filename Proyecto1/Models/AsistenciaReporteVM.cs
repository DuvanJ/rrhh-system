using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto1.Models
{
    public class AsistenciaReporteVM
    {
        public int IdAsistencia { get; set; }

        public int IdColaborador { get; set; }

        public string NombreColaborador { get; set; } = "";

        public DateTime Fecha { get; set; }

        public TimeSpan? HoraEntrada { get; set; }

        public TimeSpan? HoraSalida { get; set; }

        [NotMapped]
        public string Estado { get; set; } = "";
    }
}