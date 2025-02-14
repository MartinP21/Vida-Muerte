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
            if (ModelState.IsValid)
            {
                await _citaService.CrearCitaAsync(cita);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Editar()
        {
            return View();
        }
        
    }
}
