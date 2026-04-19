using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto1.Models
{
    public class Planilla
    {
        [Key]
        public int IdPlanilla { get; set; }

        public int Mes { get; set; }
        public int Año { get; set; }

        public DateTime FechaGeneracion { get; set; }

        public int Estado { get; set; }

        [NotMapped]
        public string EstadoTexto =>
            Estado == 1 ? "Generada" :
            Estado == 2 ? "Anulada" :
            Estado == 3 ? "Regenerada" :
            "Desconocido";

        public virtual ICollection<PlanillaDetalle> Detalles { get; set; } = new List<PlanillaDetalle>();
    }

    
}