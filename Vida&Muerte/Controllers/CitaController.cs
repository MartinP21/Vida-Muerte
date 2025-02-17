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
            if (String.IsNullOrWhiteSpace(cita.Nombre) || cita.Nombre.Length > 50)
            {
                return BadRequest("El campo 'Nombre' es inválido.");
            }

            if (String.IsNullOrWhiteSpace(cita.Apellidos) || cita.Apellidos.Length > 50)
            {
                return BadRequest("El campo 'Apellidos' es inválido.");
            }

            if (cita.FechaCita.Hour < 8 || cita.FechaCita.Hour >= 18)
            {
                ModelState.AddModelError("FechaCita", "Las citas solo pueden agendarse entre las 8:00 AM y las 5:00 PM.");
            }

            // Obtener la cantidad de citas en la misma fecha
            int citasEnElDia = (await _citaService.ObtenerCitasPorFechaAsync(cita.FechaCita.Date)).Count();

            if (citasEnElDia >= 8)
            {
                ModelState.AddModelError("FechaCita", "No se pueden agendar más de 8 citas en un mismo día.");
            }

            if (ModelState.IsValid)
            {
                cita.IdEstado = 1;
                await _citaService.CrearCitaAsync(cita);
                return RedirectToAction("Index");
            }
            return View(cita);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int Id)
        {
            var cita = await _citaService.ObtenerCitasPorIdAsync(Id);
            if (cita == null) return NotFound();
            return View(cita);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Cita cita)
        {
            if (String.IsNullOrWhiteSpace(cita.Nombre) || cita.Nombre.Length > 50)
            {
                return BadRequest("El campo 'Nombre' es inválido.");
            }

            if (String.IsNullOrWhiteSpace(cita.Apellidos) || cita.Apellidos.Length > 50)
            {
                return BadRequest("El campo 'Apellidos' es inválido.");
            }

            if (cita.FechaCita.Hour < 8 || cita.FechaCita.Hour >= 18)
            {
                ModelState.AddModelError("FechaCita", "Las citas solo pueden agendarse entre las 8:00 AM y las 5:00 PM.");
            }

            // Obtener la cantidad de citas en la misma fecha
            int citasEnElDia = (await _citaService.ObtenerCitasPorFechaAsync(cita.FechaCita.Date)).Count();

            if (citasEnElDia >= 8)
            {
                ModelState.AddModelError("FechaCita", "No se pueden agendar más de 8 citas en un mismo día.");
            }

            if (ModelState.IsValid)
            {
                await _citaService.ActualizarCitaAsync(cita);
                return RedirectToAction("Index");
            }
            return View(cita);
        }

        public async Task<IActionResult> Deshabilitar(int Id)
        {
            var cita = await _citaService.ObtenerCitasPorIdAsync(Id);
            return View(cita);
        }
        
    }
}
