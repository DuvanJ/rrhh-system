using Microsoft.AspNetCore.Mvc;
using Proyecto1.Filters;

namespace Proyecto1.Controllers
{
    public class HomeController : Controller
    {
        [ValidateSession]
        public IActionResult Index()
        {
            ViewBag.Usuario = HttpContext.Session.GetString("Usuario") ?? "Usuario";
            ViewBag.Perfil = HttpContext.Session.GetString("Perfil") ?? "";

            return View();
        }

        public IActionResult SinPermisos()
        {
            return View();
        }
    }
}
