using System.ComponentModel.DataAnnotations;

namespace Proyecto1.Models
{
    public class DescansoColaborador
    {
        [Key]
        public int IdDescanso { get; set; }
        public int IdColaborador { get; set; }
        public DateTime FechaDescanso { get; set; }

        public Colaborador Colaborador { get; set; }
    }
}
