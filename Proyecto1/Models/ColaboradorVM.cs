using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto1.Models
{
    public class ColaboradorVM
    {
        public int IdColaborador { get; set; }
        
        public string NombreCompleto { get; set; }
    }
}
