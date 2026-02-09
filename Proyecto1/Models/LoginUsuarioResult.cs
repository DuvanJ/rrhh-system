public class LoginUsuarioResult
{
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; }
    public int IdPerfil { get; set; }
    public string NombrePerfil { get; set; }
    public int IdColaborador { get; set; }
    public bool Estado { get; set; } // 🔥 BIT
}

