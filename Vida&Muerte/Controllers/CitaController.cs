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

        // Método de accion para mostrar una tabla con todas las citas, inicialmente se muestran las citas pendientes y paginadas
        public async Task<IActionResult> Index(int pagina = 1, int registrosPorPagina = 5, int? idEstado = 1)
        {
            // Se invoca al servicio para obtener las citas filtradas y paginadas con el total de registros
            var (citas, totalRegistros) = await _citaService.ObtenerCitasPorEstadoPaginadasAsync(pagina, registrosPorPagina, idEstado);

            // Se calculan y asignan en ViewBag los valores necesarios para la paginacion: La pagina actual, el total de paginas y el filtro aplicado
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);
            ViewBag.RegistrosPorPagina = registrosPorPagina;
            ViewBag.IdEstado = idEstado; // Guardamos el filtro actual

            return View(citas);
        }


        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(Cita cita)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(cita.Nombre) || cita.Nombre.Length > 50)
                {
                    ModelState.AddModelError("Nombre", "El campo 'Nombre' es inválido.");
                }

                if (String.IsNullOrWhiteSpace(cita.Apellidos) || cita.Apellidos.Length > 50)
                {
                    ModelState.AddModelError("Apellidos", "El campo 'Apellidos' es inválido.");
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
                    cita.IdEstado = 1;
                    await _citaService.CrearCitaAsync(cita);
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

        // Método de acción para mostrar los detalles de una cita
        public async Task<IActionResult> Detalles(int id, string returnUrl)
        {
            try
            {
                var cita = await _citaService.ObtenerCitasPorIdAsync(id);
                if (cita == null)
                {
                    return NotFound();
                }
                ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index");
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
                }

                if (String.IsNullOrWhiteSpace(cita.Apellidos) || cita.Apellidos.Length > 50)
                {
                    ModelState.AddModelError("Apellidos", "El campo 'Apellidos' es inválido.");
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
            var cita = await _citaService.ObtenerCitasPorIdAsync(id);
            if (cita == null) return NotFound();
            
            return View(cita);
        }

        [HttpPost]
        public async Task <IActionResult> Deshabilitar(int id, string motivo)
        {
            try
            {
                // Validación
                if (string.IsNullOrWhiteSpace(motivo) || motivo.Length < 20)
                {
                    ModelState.AddModelError("Motivo", "El motivo debe tener al menos 20 caracteres.");

                    // Regresa a la vista de detalles
                    var cita = await _citaService.ObtenerCitasPorIdAsync(id);
                    return View("Detalles", cita);
                }

                if (string.IsNullOrWhiteSpace(motivo) || motivo.Length > 150)
                {
                    ModelState.AddModelError("Motivo", "El motivo es demasiado largo. !No puede tener mas de 150 caracteres.");
                }

                await _citaService.DeshabilitarCitaAsync(id, motivo);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al deshabilitar la cita");
                return RedirectToAction("Index");
            }
        }
    }
}

