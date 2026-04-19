using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto1.Models
{

    public class Incapacidad
    {
        [Key]
        public int IdIncapacidad { get; set; }

        [Required]
        public int IdColaborador { get; set; }

        [Required]
        public int IdTipoIncapacidad { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        public int DiasIncapacidad { get; set; }

        public bool EsRemunerada { get; set; }

        [StringLength(200)]
        public string? Motivo { get; set; }

        [StringLength(250)]
        public string? DocumentoAdjunto { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required, StringLength(20)]
        public string Estado { get; set; } = "Activa";
    }
}