using System.ComponentModel.DataAnnotations;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Vida_Muerte.Controllers
{
    public class CitaController : Controller
    {
        private readonly ILogger<CitaController> _logger;
        private readonly ICitaService _citaService;
        public CitaController(ILogger<CitaController> logger, ICitaService citaService)
        {
            _logger = logger;
            _citaService = citaService;
        }
        public async Task<IActionResult> Index()
        {
            var cita = await _citaService.ObtenerCitasAsync();
            return View(cita);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(Cita cita)
        {
            // Validar que el campo Nombre no esté vacío y no supere los 50 caracteres
            if (String.IsNullOrWhiteSpace(cita.Nombre) || cita.Nombre.Length > 50)
            {
                return BadRequest("El campo 'Nombre' es inválido.");
            }

            // Validar que el campo Apellidos no esté vacío y no supere los 50 caracteres
            if (String.IsNullOrWhiteSpace(cita.Apellidos) || cita.Apellidos.Length > 50)
            {
                return BadRequest("El campo 'Apellidos' es inválido.");
            }

            // Validar que la cita esté dentro del horario permitido (8:00 AM - 5:00 PM)
            if (cita.FechaCita.Hour < 8 || cita.FechaCita.Hour >= 18)
            {
                ModelState.AddModelError("FechaCita", "Las citas solo pueden agendarse entre las 8:00 AM y las 5:00 PM.");
            }

            // Obtener la cantidad de citas que ya existen en la misma fecha
            int citasEnElDia = (await _citaService.ObtenerCitasPorFechaAsync(cita.FechaCita.Date)).Count();

            // Validar que no haya más de 8 citas en un mismo día
            if (citasEnElDia >= 8)
            {
                ModelState.AddModelError("FechaCita", "No se pueden agendar más de 8 citas en un mismo día.");
            }
            // Si no hay errores en ModelState, proceder con la creación de la cita
            if (ModelState.IsValid)
            {
                cita.IdEstado = 1;
                await _citaService.CrearCitaAsync(cita);
                return RedirectToAction("Index");
            }
            return View(cita);
        }

        // Método de acción para mostrar los detalles de una cita
        public async Task<IActionResult> Detalles(int id)
        {
            try
            {
                var cita = await _citaService.ObtenerCitasPorIdAsync(id);
                if (cita == null)
                {
                    return NotFound();
                }
                return View(cita);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los detalles de la cita");
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int Id)
        {
            try
            {
                var cita = await _citaService.ObtenerCitasPorIdAsync(Id);
                if (cita == null)
                {
                    return NotFound();
                }
                return View(cita);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la cita para editar");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, Cita cita)
        {
            try
            {
                if (id != cita.Id)
                {
                    return BadRequest();
                }

                if (String.IsNullOrWhiteSpace(cita.Nombre) || cita.Nombre.Length > 50)
                {
                    ModelState.AddModelError("Nombre", "El campo 'Nombre' es inválido.");
                    return View(cita);
                }

                if (String.IsNullOrWhiteSpace(cita.Apellidos) || cita.Apellidos.Length > 50)
                {
                    ModelState.AddModelError("Apellidos", "El campo 'Apellidos' es inválido.");
                    return View(cita);
                }

                if (cita.FechaCita.Hour < 8 || cita.FechaCita.Hour >= 18)
                {
                    ModelState.AddModelError("FechaCita", "Las citas solo pueden agendarse entre las 8:00 AM y las 5:00 PM.");
                }

                var citasEnElDia = (await _citaService.ObtenerCitasPorFechaAsync(cita.FechaCita.Date)).Count();
                if (citasEnElDia >= 8)
                {
                    ModelState.AddModelError("FechaCita", "No se pueden agendar más de 8 citas en un mismo día.");
                }

                if (ModelState.IsValid)
                {
                    await _citaService.ActualizarCitaAsync(cita);
                    return RedirectToAction("Index");
                }
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la cita");
                ModelState.AddModelError("", "Ha ocurrido un error al procesar su solicitud.");
            }

            return View(cita);
        }

        [HttpGet]
        public async Task<IActionResult> Deshabilitar(int id)
        {
            try
            {
                var cita = await _citaService.ObtenerCitasPorIdAsync(id);
                if (cita == null)
                {
                    return NotFound();
                }
                return View(cita);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la cita para deshabilitar");
                return RedirectToAction("Index");
            }
        }
    }
}

