using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto1.Models
{
    public class Asistencia
    {
        [Key]
        public int IdAsistencia { get; set; }

        public int IdColaborador { get; set; }

        public DateTime Fecha { get; set; }

        public TimeSpan? HoraEntrada { get; set; }

        public TimeSpan? HoraSalida { get; set; }

        // 🔹 SOLO PARA MOSTRAR
        [NotMapped]
        public string NombreColaborador { get; set; } = string.Empty;

        // 🔹 SOLO PARA AJUSTE / AUDITORÍA
        [NotMapped]
        [Required(ErrorMessage = "Debe indicar el motivo del ajuste.")]
        [StringLength(250)]
        public string Motivo { get; set; } = string.Empty;


    }
}

