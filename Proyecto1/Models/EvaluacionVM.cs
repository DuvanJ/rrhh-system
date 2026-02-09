using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace Proyecto1.Models
{
    public class EvaluacionVM
    {
        public int IdEvaluacion { get; set; }

        public int IdColaborador { get; set; }

        [BindNever]
        public string? NombreColaborador { get; set; }

        public DateTime FechaEvaluacion { get; set; }

        public string ComentariosGenerales { get; set; }

        public decimal? PuntajeTotal { get; set; }

        // Lista de criterios seleccionables
        public List<CriterioEvaluacion> Criterios { get; set; } = new List<CriterioEvaluacion>();

        // Lista de colaboradores para el dropdown
        public List<ColaboradorVM> Colaboradores { get; set; } = new List<ColaboradorVM>();


        // Detalles de la evaluación (puntajes y observaciones)
        public List<DetalleEvaluacionVM> Detalles { get; set; } = new List<DetalleEvaluacionVM>();
    }

    public class DetalleEvaluacionVM
    {
        public int IdCriterio { get; set; }
        public decimal? Puntaje { get; set; }
        public string Observacion { get; set; }
    }

}
