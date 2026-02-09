using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto1.Models;
using Proyecto1.Services;

namespace Proyecto1.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioService _service;
        private readonly ColaboradoresService _colaboradorService; // ✅ DECLARAR

        public UsuariosController(
            UsuarioService service,
            ColaboradoresService colaboradorService) // ✅ INYECTAR
        {
            _service = service;
            _colaboradorService = colaboradorService;
        }


        public IActionResult Create()
        {
            CargarPerfiles();
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Usuario u)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Error: revise los datos ingresados.";
                CargarPerfiles();
                return View(u);
            }

            await _service.InsertarAsync(u);
            TempData["Success"] = "Usuario creado correctamente.";
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> ObtenerColaborador(int id)
        {
            var c = await _colaboradorService.ObtenerInfoAsync(id);

            if (c == null)
                return NotFound();

            return Json(new
            {
                nombreCompleto = $"{c.Nombre} {c.Apellidos}",
                correo = c.Correo
            });
        }


        private void CargarPerfiles()
        {
            ViewBag.Perfiles = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Administrador" },
                new SelectListItem { Value = "2", Text = "RRHH" },
                new SelectListItem { Value = "3", Text = "Colaborador" }
            };
        }

        [HttpPost]
        public async Task<IActionResult> BuscarColaborador(Usuario model)
        {
            CargarPerfiles();

            var c = await _colaboradorService.ObtenerInfoAsync(model.IdColaborador);

            if (c == null)
            {
                ModelState.AddModelError("", "Colaborador no encontrado.");
                return View("Create", model);
            }

            ViewBag.NombreColaborador = $"{c.Nombre} {c.Apellidos}";
            ViewBag.CorreoColaborador = c.Correo;

            model.Correo = c.Correo;

            return View("Create", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _service.ObtenerPorIdAsync(id);
            if (user == null)
                return NotFound();

            CargarPerfiles();
            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Usuario u)
        {
            if (!ModelState.IsValid)
            {
                CargarPerfiles();
                TempData["Error"] = "No se pudo actualizar el usuario.";
                return View(u);
            }

            await _service.ActualizarAsync(u);
            TempData["Success"] = "Usuario actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Index(string buscar)
        {
            var lista = await _service.ListarAsync();

            if (!string.IsNullOrEmpty(buscar))
            {
                lista = lista
                    .Where(u => u.NombreUsuario.Contains(buscar))
                    .ToList();


                if (!lista.Any())
                    ViewBag.SinResultados = true;
            }

            ViewBag.Buscar = buscar;
            return View(lista);
        }

        public async Task<IActionResult> CambiarEstado(int id)
        {
            await _service.CambiarEstadoAsync(id);

            TempData["Success"] = "Estado del usuario actualizado.";
            return RedirectToAction(nameof(Index));
        }




    }
}

