using Microsoft.AspNetCore.Mvc;
using Proyecto1.Models;
using Proyecto1.Services;
using System.Text.RegularExpressions;

namespace Proyecto1.Controllers
{
    public class ColaboradoresController : Controller
    {
        private readonly ColaboradoresService _service;

        public ColaboradoresController(ColaboradoresService service)
        {
            _service = service;
        }

        // ========== LISTAS PARA DROPDOWNS ==========
        private readonly List<string> _departamentos = new List<string>
        {
            "Recursos Humanos",
            "Finanzas",
            "Contabilidad",
            "Ventas",
            "Marketing",
            "Operaciones",
            "Tecnología",
            "Producción",
            "Logística",
            "Administración"
        };

        private readonly List<string> _puestos = new List<string>
        {
            "Analista RRHH",
            "Asistente Administrativo",
            "Contador",
            "Ejecutivo de Ventas",
            "Gerente de Proyecto",
            "Ingeniero de Sistemas",
            "Operario",
            "Supervisor",
            "Coordinador",
            "Especialista en Nómina",
            "Asistente de Nómina",
            "Jefe de Departamento",
            "Director Financiero",
            "Coordinador de Logística"
        };

        private void CargarDropdowns()
        {
            ViewBag.Departamentos = _departamentos.OrderBy(d => d).ToList();
            ViewBag.Puestos = _puestos.OrderBy(p => p).ToList();
        }

        public IActionResult Create()
        {
            CargarDropdowns();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Colaborador c)
        {
            // ========== LIMPIAR VALIDACIONES AUTOMÁTICAS ==========
            ModelState.Clear();

            // ========== VALIDAR CÉDULA ==========
            if (string.IsNullOrWhiteSpace(c.Cedula))
            {
                ModelState.AddModelError("Cedula", "La cédula es requerida.");
            }
            else
            {
                string cedulaLimpia = c.Cedula.Replace("-", "").Trim();

                if (!Regex.IsMatch(cedulaLimpia, @"^\d+$"))
                {
                    ModelState.AddModelError("Cedula", "La cédula solo debe contener dígitos.");
                }
                else if (cedulaLimpia.Length != 9 && cedulaLimpia.Length != 12)
                {
                    ModelState.AddModelError("Cedula", "La cédula debe tener 9 dígitos (costarricense) o 12 dígitos (DIMEX).");
                }
            }

            // ========== VALIDAR NOMBRE ==========
            if (string.IsNullOrWhiteSpace(c.Nombre))
            {
                ModelState.AddModelError("Nombre", "El nombre es requerido.");
            }
            else if (c.Nombre.Length < 3)
            {
                ModelState.AddModelError("Nombre", "El nombre debe tener al menos 3 caracteres.");
            }

            // ========== VALIDAR APELLIDOS ==========
            if (string.IsNullOrWhiteSpace(c.Apellidos))
            {
                ModelState.AddModelError("Apellidos", "Los apellidos son requeridos.");
            }
            else if (c.Apellidos.Length < 3)
            {
                ModelState.AddModelError("Apellidos", "Los apellidos deben tener al menos 3 caracteres.");
            }

            // ========== VALIDAR CORREO ==========
            if (!string.IsNullOrWhiteSpace(c.Correo))
            {
                if (!Regex.IsMatch(c.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    ModelState.AddModelError("Correo", "El correo electrónico no tiene un formato válido.");
                }
            }

            // ========== VALIDAR PUESTO ==========
            if (string.IsNullOrWhiteSpace(c.Puesto))
            {
                ModelState.AddModelError("Puesto", "El puesto es requerido.");
            }
            else if (!_puestos.Contains(c.Puesto))
            {
                ModelState.AddModelError("Puesto", "Debe seleccionar un puesto válido.");
            }

            // ========== VALIDAR DEPARTAMENTO ==========
            if (string.IsNullOrWhiteSpace(c.Departamento))
            {
                ModelState.AddModelError("Departamento", "El departamento es requerido.");
            }
            else if (!_departamentos.Contains(c.Departamento))
            {
                ModelState.AddModelError("Departamento", "Debe seleccionar un departamento válido.");
            }

            // ========== VALIDAR FECHA DE INGRESO ==========
            if (c.FechaIngreso == default)
            {
                ModelState.AddModelError("FechaIngreso", "La fecha de ingreso es requerida.");
            }
            else if (c.FechaIngreso > DateTime.Today)
            {
                ModelState.AddModelError("FechaIngreso", "La fecha de ingreso no puede ser futura.");
            }

            // ========== VALIDAR SALARIO BASE ==========
            if (c.SalarioBase <= 0)
            {
                ModelState.AddModelError("SalarioBase", "El salario base debe ser mayor a cero.");
            }

            // ========== SI HAY ERRORES ==========
            if (!ModelState.IsValid)
            {
                CargarDropdowns();
                ViewBag.Errores = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToList());

                TempData["Error"] = "Por favor revise los campos con error. Hay formatos incorrectos o campos incompletos.";
                return View(c);
            }

            // ========== GUARDAR ==========
            try
            {
                await _service.Insertar(c);
                TempData["Success"] = "Colaborador registrado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                CargarDropdowns();
                TempData["Error"] = "Error al registrar el colaborador: " + ex.Message;
                return View(c);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var col = await _service.ObtenerPorId(id);
            if (col == null)
            {
                TempData["Error"] = "El colaborador no existe.";
                return RedirectToAction("Index");
            }
            return View(col);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var col = await _service.ObtenerPorId(id);
            if (col == null)
            {
                TempData["Error"] = "El colaborador no existe.";
                return RedirectToAction("Index");
            }
            CargarDropdowns();
            return View(col);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Colaborador c)
        {
            // ========== LIMPIAR VALIDACIONES AUTOMÁTICAS ==========
            ModelState.Clear();

            // ========== VALIDAR CÉDULA ==========
            if (string.IsNullOrWhiteSpace(c.Cedula))
            {
                ModelState.AddModelError("Cedula", "La cédula es requerida.");
            }
            else
            {
                string cedulaLimpia = c.Cedula.Replace("-", "").Trim();

                if (!Regex.IsMatch(cedulaLimpia, @"^\d+$"))
                {
                    ModelState.AddModelError("Cedula", "La cédula solo debe contener dígitos.");
                }
                else if (cedulaLimpia.Length != 9 && cedulaLimpia.Length != 12)
                {
                    ModelState.AddModelError("Cedula", "La cédula debe tener 9 dígitos (costarricense) o 12 dígitos (DIMEX).");
                }
            }

            // ========== VALIDAR NOMBRE ==========
            if (string.IsNullOrWhiteSpace(c.Nombre))
            {
                ModelState.AddModelError("Nombre", "El nombre es requerido.");
            }
            else if (c.Nombre.Length < 3)
            {
                ModelState.AddModelError("Nombre", "El nombre debe tener al menos 3 caracteres.");
            }

            // ========== VALIDAR APELLIDOS ==========
            if (string.IsNullOrWhiteSpace(c.Apellidos))
            {
                ModelState.AddModelError("Apellidos", "Los apellidos son requeridos.");
            }
            else if (c.Apellidos.Length < 3)
            {
                ModelState.AddModelError("Apellidos", "Los apellidos deben tener al menos 3 caracteres.");
            }

            // ========== VALIDAR CORREO ==========
            if (!string.IsNullOrWhiteSpace(c.Correo))
            {
                if (!Regex.IsMatch(c.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    ModelState.AddModelError("Correo", "El correo electrónico no tiene un formato válido.");
                }
            }

            // ========== VALIDAR PUESTO ==========
            if (string.IsNullOrWhiteSpace(c.Puesto))
            {
                ModelState.AddModelError("Puesto", "El puesto es requerido.");
            }
            else if (!_puestos.Contains(c.Puesto))
            {
                ModelState.AddModelError("Puesto", "Debe seleccionar un puesto válido.");
            }

            // ========== VALIDAR DEPARTAMENTO ==========
            if (string.IsNullOrWhiteSpace(c.Departamento))
            {
                ModelState.AddModelError("Departamento", "El departamento es requerido.");
            }
            else if (!_departamentos.Contains(c.Departamento))
            {
                ModelState.AddModelError("Departamento", "Debe seleccionar un departamento válido.");
            }

            // ========== VALIDAR FECHA DE INGRESO ==========
            if (c.FechaIngreso == default)
            {
                ModelState.AddModelError("FechaIngreso", "La fecha de ingreso es requerida.");
            }
            else if (c.FechaIngreso > DateTime.Today)
            {
                ModelState.AddModelError("FechaIngreso", "La fecha de ingreso no puede ser futura.");
            }

            // ========== VALIDAR SALARIO BASE ==========
            if (c.SalarioBase <= 0)
            {
                ModelState.AddModelError("SalarioBase", "El salario base debe ser mayor a cero.");
            }

            // ========== SI HAY ERRORES ==========
            if (!ModelState.IsValid)
            {
                CargarDropdowns();
                ViewBag.Errores = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToList());

                TempData["Error"] = "Por favor revise los campos con error. Hay formatos incorrectos o campos incompletos.";
                return View(c);
            }

            // ========== ACTUALIZAR ==========
            try
            {
                await _service.Actualizar(c);
                TempData["Success"] = "Colaborador actualizado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                CargarDropdowns();
                TempData["Error"] = "Error al actualizar el colaborador: " + ex.Message;
                return View(c);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var col = await _service.ObtenerPorId(id);
            if (col == null)
            {
                TempData["Error"] = "El colaborador no existe.";
                return RedirectToAction("Index");
            }

            return View(col);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int IdColaborador)
        {
            try
            {
                await _service.Eliminar(IdColaborador);
                TempData["Success"] = "Colaborador desactivado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al desactivar el colaborador: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        // LISTADO + BUSCADOR
        public async Task<IActionResult> Index(int? buscar, int? estado)
        {
            var lista = await _service.ObtenerColaboradores();

            if (buscar.HasValue)
            {
                lista = lista.Where(c => c.IdColaborador == buscar.Value).ToList();

                if (!lista.Any())
                {
                    TempData["Error"] = "El colaborador no existe.";
                }
            }

            // 🔥 FILTRO ESTADO
            if (estado.HasValue)
            {
                bool activo = estado.Value == 1;
                lista = lista.Where(c => c.Estado == activo).ToList();
            }

            ViewBag.Buscar = buscar;
            ViewBag.Total = lista.Count;
            ViewBag.Activos = lista.Count(c => c.Estado);
            ViewBag.Inactivos = lista.Count(c => !c.Estado);

            return View(lista);
        }

        // ENDPOINT AJAX PARA BUSCAR
        [HttpGet]
        public async Task<IActionResult> BuscarColaborador(int id)
        {
            var c = await _service.ObtenerInfoAsync(id);

            if (c == null)
                return NotFound();

            return Json(new
            {
                nombre = $"{c.Nombre} {c.Apellidos}",
                correo = c.Correo,
                puesto = c.Puesto
            });
        }


    }
}