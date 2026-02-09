namespace Proyecto1.Models
{
    public class UsuarioListado
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string NombrePerfil { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
}
