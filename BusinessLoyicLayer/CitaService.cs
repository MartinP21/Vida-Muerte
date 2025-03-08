using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class CitaService : ICitaService
    {
        private readonly ICitaRepository _citaRepository;
        // Constructor con inyección de dependencia del repositorio
        public CitaService(ICitaRepository citaRepository)
        {
            _citaRepository = citaRepository;
        }

        // Método asincrono para obtener citas paginadas según el estado
        public async Task<(IEnumerable<Cita> citas, int totalRegistros)> ObtenerCitasPorEstadoPaginadasAsync(int pagina, int registrosPorPagina, int? idEstado)
        {
            var citas = await _citaRepository.ObtenerCitasPorEstadoPaginadasAsync(pagina, registrosPorPagina, idEstado);
            var totalRegistros = await _citaRepository.ObtenerTotalCitasPorEstadoAsync(idEstado);
            return (citas, totalRegistros);
        }

        // Método que obtiene el total de las citas
        public async Task<int> ObtenerTotalCitasAsync()
        {
            return await _citaRepository.ObtenerTotalCitasAsync();
        }

        // Metodo asincrono para obtener una cita por su ID
        public Task<Cita> ObtenerCitasPorIdAsync(int Id)
        {
            return _citaRepository.ObtenerCitasPorIdAsync(Id);
        }

        // Metodo asincrono para crear una nueva cita con validaciones
        public async Task CrearCitaAsync(Cita cita)
        {
            // Validar formato de cédula
            if (!ValidarFormatoCedula(cita.Cedula))
            {
                throw new ValidationException("La cédula debe contener exactamente 11 números.");
            }

            // Validar formato de teléfono
            if (!ValidarFormatoTelefono(cita.Telefono))
            {
                throw new ValidationException("El teléfono debe tener un formato válido de República Dominicana.");
            }

            // Llama al metodo limpiarCampos que eliminar los espacios en bancos de Nombre y Apellidos
            cita.LimpiarCampos();
            await _citaRepository.CrearCitaAsync(cita);
        }
        // Metodo asincrono para actualizar una cita
        public async Task ActualizarCitaAsync(Cita cita)
        {
            // Primero limpia todos los campos
            cita.LimpiarCampos();

            // Valida el formato de la cédula y del teléfono
            if (!ValidarFormatoCedula(cita.Cedula))
            {
                throw new ValidationException("La cédula debe contener exactamente 11 números.");
            }
            if (!ValidarFormatoTelefono(cita.Telefono))
            {
                throw new ValidationException("El teléfono debe tener un formato válido de República Dominicana.");
            }

            await _citaRepository.ActualizarCitaAsync(cita);
        }

        // Metodo asincrono para deshabilitar una cita por su ID
        public async Task DeshabilitarCitaAsync(int Id, string motivo)
        {
            await _citaRepository.DeshabilitarCitaAsync(Id, motivo);
        }
        // Metodo privado para validar el formato de la cédula (11 dígitos numéricos)
        private bool ValidarFormatoCedula(string cedula)
        {
            return !string.IsNullOrWhiteSpace(cedula) && // Verifica que no esté vacía
                   cedula.Replace("-", "").Length == 11 && // Elimina guiones y verifica que tenga 11 dígitos
                   cedula.Replace("-", "").All(char.IsDigit); // Verifica que todos los caracteres sean números
        }
        // Metodo privado para validar el formato del teléfono (809, 829 o 849 con 10 dígitos)
        private bool ValidarFormatoTelefono(string telefono)
        {
            var numeroLimpio = new string(telefono.Where(char.IsDigit).ToArray()); // Extrae solo los números
            return numeroLimpio.Length == 10 && // Verifica que tenga exactamente 10 dígitos
                   (numeroLimpio.StartsWith("809") ||
                    numeroLimpio.StartsWith("829") ||
                    numeroLimpio.StartsWith("849")); // Verifica si comienza con un código válido
        }
    }
}
