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
        public async Task<IActionResult> Index(int pagina = 1, int registrosPorPagina = 100, int? idEstado = 1)
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

        // Método para llamar a la vista que contiene el formulario 'Crear'
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        // Método 'POST' para crear nuevas citas
        [HttpPost]
        public async Task<IActionResult> Crear(Cita cita)
        {
            try
            {
                // Condición que valida si no tiene un nombre o si el nombre es mayor a 50
                if (String.IsNullOrWhiteSpace(cita.Nombre) || cita.Nombre.Length > 50)
                {
                    ModelState.AddModelError("Nombre", "El campo 'Nombre' es inválido.");
                }

                // Condición que valida si no tiene un apellido o si el apellido es mayor a 50
                if (String.IsNullOrWhiteSpace(cita.Apellidos) || cita.Apellidos.Length > 50)
                {
                    ModelState.AddModelError("Apellidos", "El campo 'Apellidos' es inválido.");
                }

                // Condición que valida si la hora de la cita es menor a las 8:00 AM o Mayor a las 5:00 PM
                if (cita.FechaCita.Hour < 8 || cita.FechaCita.Hour >= 18)
                {
                    ModelState.AddModelError("FechaCita", "Las citas solo pueden agendarse entre las 8:00 AM y las 5:00 PM.");
                }


                // Si todas las condiciónes se cumplen entonces se crea la cita

                if (ModelState.IsValid)
                {
                    cita.IdEstado = 1; // Le asigna el valor de '1 = Pendiente'
                    await _citaService.CrearCitaAsync(cita); // Resive la cita y la crea
                    return RedirectToAction("Index"); // Redirecciona a la página principal
                }
            }
            // Captura el error
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
                var cita = await _citaService.ObtenerCitasPorIdAsync(id); // Obtiene la cita por su 'ID'
                // Válida que la cita no sea null
                if (cita == null) 
                {
                    return NotFound();
                }
                ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index");
                return View(cita);
            }
            // Captura el error
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los detalles de la cita");
                return RedirectToAction("Index");
            }
        }

        // Método que retorna la vista con el formulario para editar una cita
        [HttpGet]
        public async Task<IActionResult> Editar(int Id)
        {
            try
            {
                // Obtiene la cita por su 'ID'
                var cita = await _citaService.ObtenerCitasPorIdAsync(Id);

                // Válida que la cita exista
                if (cita == null)
                {
                    return NotFound();
                }
                return View(cita);
            }
            // Captura el error
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la cita para editar");
                return RedirectToAction("Index");
            }
        }

        // Método 'POST' para editar una cita
        [HttpPost]
        public async Task<IActionResult> Editar(int id, Cita cita)
        {
            try
            {
                // Válida que el 'ID' que se esta guardando sea igual al 'ID' de la cita existente
                if (id != cita.Id)
                {
                    return BadRequest();
                }

                // Válida que el input del nombre no este vacio ni contenga espacios indebidos o que el nombre sea mayor a 50 caracteres
                if (String.IsNullOrWhiteSpace(cita.Nombre) || cita.Nombre.Length > 50)
                {
                    ModelState.AddModelError("Nombre", "El campo 'Nombre' es inválido.");
                }

                // Válida que el input del apellido no este vacio ni contenga espacios indebidos o que el apellido sea mayor a 50 caracteres
                if (String.IsNullOrWhiteSpace(cita.Apellidos) || cita.Apellidos.Length > 50)
                {
                    ModelState.AddModelError("Apellidos", "El campo 'Apellidos' es inválido.");
                }

                // Válida que la se guarde entre las 8:00 AM y las 5:00 PM
                if (cita.FechaCita.Hour < 8 || cita.FechaCita.Hour >= 18)
                {
                    ModelState.AddModelError("FechaCita", "Las citas solo pueden agendarse entre las 8:00 AM y las 5:00 PM.");

                }

                // Si todas las condiciónes se cumplen entonces se edita la cita

                if (ModelState.IsValid)
                {
                    await _citaService.ActualizarCitaAsync(cita); // Actualiza la cita
                    return RedirectToAction("Index"); // Retorna a la pagina principal
                }
            }
            // Captura el error
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

        // Método 'POST' para deshabilitar una cita
        [HttpPost]
        public async Task <IActionResult> Deshabilitar(int id, string motivo)
        {
            try
            {
                // Válida que el motivo no contenga espacios inapropiados o que sea menor a 5 caracteres
                if (string.IsNullOrWhiteSpace(motivo) || motivo.Length < 5)
                {
                    ModelState.AddModelError("Motivo", "El motivo debe tener al menos 5 caracteres.");

                    // Regresa a la vista de detalles
                    var cita = await _citaService.ObtenerCitasPorIdAsync(id);
                    return View("Detalles", cita);
                }

                // Válida que el motivo no contenga espacios inapropiados o que sea mayor a 150 caracteres
                if (string.IsNullOrWhiteSpace(motivo) || motivo.Length > 150)
                {
                    ModelState.AddModelError("Motivo", "El motivo es demasiado largo. !No puede tener mas de 150 caracteres.");
                }

                await _citaService.DeshabilitarCitaAsync(id, motivo); // Deshabilita la cita con el id y el motivo por el cual se deshabilita
                return RedirectToAction("Index"); // Redirecciona a la página principal
            }
            // Captura el error
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al deshabilitar la cita");
                return RedirectToAction("Index");
            }
        }
    }
}

