using System.ComponentModel.DataAnnotations;

namespace Proyecto1.Models
{
    public class AsistenciaAuditoria
    {
        [Key]
        public int IdAuditoria { get; set; }

        public int IdAsistencia { get; set; }
        public int IdColaborador { get; set; }
        public DateTime Fecha { get; set; }

        public TimeSpan? HoraEntradaAnterior { get; set; }
        public TimeSpan? HoraSalidaAnterior { get; set; }

        public TimeSpan? HoraEntradaNueva { get; set; }
        public TimeSpan? HoraSalidaNueva { get; set; }

        public string UsuarioModifica { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string? Motivo { get; set; }
    }
}

