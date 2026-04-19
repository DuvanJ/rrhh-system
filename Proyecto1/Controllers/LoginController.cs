using Microsoft.AspNetCore.Mvc;
using Proyecto1.BLL;

namespace Proyecto1.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;
        private readonly EmailService _emailService;

        public LoginController(LoginService loginService, EmailService emailService)
        {
            _loginService = loginService;
            _emailService = emailService;
        }

        // =============================
        // LOGIN
        // =============================

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string usuario, string contrasena)
        {
            var user = await _loginService.ValidarLogin(usuario, contrasena);

            if (user == null)
            {
                TempData["Error"] = "Usuario o contraseña incorrectos";
                return View();
            }

            HttpContext.Session.SetString("Usuario", user.NombreUsuario);
            HttpContext.Session.SetString("Perfil", user.NombrePerfil);
            HttpContext.Session.SetInt32("IdColaborador", user.IdColaborador);

            return RedirectToAction("Index", "Home");
        }

        // =============================
        // RECUPERAR CONTRASEÑA
        // =============================

        [HttpGet]
        public IActionResult RecuperarContrasena()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecuperarContrasena(string correo)
        {
            var usuario = await _loginService.ObtenerPorCorreo(correo);

            if (usuario == null)
            {
                ViewBag.Error = "El correo no está registrado.";
                return View();
            }

            // Guardar correo en sesión
            HttpContext.Session.SetString("RecuperacionCorreo", usuario.Correo);

            var token = Guid.NewGuid().ToString();

            var link = Url.Action(
                "RestablecerContrasena",
                "Login",
                new { token = token },
                Request.Scheme
            );

            var mensaje = $@"
Hola {usuario.NombreUsuario},

Para restablecer tu contraseña ingresa al siguiente enlace:

{link}

Si no solicitaste este cambio puedes ignorar este correo.
";

            await _emailService.EnviarAsync(
                usuario.Correo,
                "Recuperar contraseña",
                mensaje
            );

            ViewBag.Mensaje = "Se envió un enlace de recuperación a su correo.";

            return View();
        }

        // =============================
        // RESTABLECER CONTRASEÑA
        // =============================

        [HttpGet]
        public IActionResult RestablecerContrasena(string token)
        {
            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RestablecerContrasena(string password, string confirmar)
        {
            if (password != confirmar)
            {
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }

            var correo = HttpContext.Session.GetString("RecuperacionCorreo");

            if (correo == null)
            {
                return RedirectToAction("Index");
            }

            await _loginService.ActualizarContrasena(correo, password);

            HttpContext.Session.Remove("RecuperacionCorreo");

            TempData["Mensaje"] = "Contraseña actualizada correctamente.";

            return RedirectToAction("Index");
        }

        // =============================
        // LOGOUT
        // =============================

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}