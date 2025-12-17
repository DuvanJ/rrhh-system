using Microsoft.AspNetCore.Mvc;
using Proyecto1.Services;

namespace Proyecto1.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string usuario, string contrasena)
        {
            var user = await _loginService.ValidarLogin(usuario, contrasena);

            if (user != null)
            {
                HttpContext.Session.SetString("Usuario", user.NombreUsuario);
                HttpContext.Session.SetString("Perfil", user.NombrePerfil);
                HttpContext.Session.SetInt32("IdUsuario", user.IdUsuario);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
