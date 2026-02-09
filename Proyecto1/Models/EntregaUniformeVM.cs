using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;

namespace Proyecto1.Models
{
    public class EntregaUniformeVM
    {
        public int IdColaborador { get; set; }
        public string? NombreColaborador { get; set; }

        public DateTime FechaEntrega { get; set; } = DateTime.Now;
        public string? Observaciones { get; set; }


        public List<EntregaUniforme> Uniformes { get; set; } = new();

        [ValidateNever]
        public List<TipoUniforme> TiposUniforme { get; set; }

        [ValidateNever]
        public List<string> Tallas { get; set; }
    }
}

