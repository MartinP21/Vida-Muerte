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
        public CitaService(ICitaRepository citaRepository)
        {
            _citaRepository = citaRepository;
        }
        public Task<IEnumerable<Cita>> ObtenerCitasAsync()
        {
            return _citaRepository.ObtenerCitasAsync();
        }
        public Task<Cita> ObtenerCitasPorIdAsync(int Id)
        {
            return _citaRepository.ObtenerCitasPorIdAsync(Id);
        }
        public async Task<IEnumerable<Cita>> ObtenerCitasPorFechaAsync(DateTime fecha)
        {
            var citas = await _citaRepository.ObtenerCitasAsync();
            return citas.Where(c => c.FechaCita.Date == fecha.Date);
        }
        public async Task<bool> ValidarCedulaUnicaAsync(string cedula)
        {
            return !await _citaRepository.ExisteCedulaAsync(cedula);
        }
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

            // Verificar cédula única
            if (!await ValidarCedulaUnicaAsync(cita.Cedula))
            {
                throw new ValidationException("Ya existe una cita registrada con esta cédula.");
            }

            await _citaRepository.CrearCitaAsync(cita);
        }

        public async Task ActualizarCitaAsync(Cita cita)
        {
            await _citaRepository.ActualizarCitaAsync(cita);
        }

        public async Task DeshabilitarCitaAsync(int Id)
        {
            await _citaRepository.DeshabilitarCitaAsync(Id);
        }

        private bool ValidarFormatoCedula(string cedula)
        {
            return !string.IsNullOrWhiteSpace(cedula) &&
                   cedula.Replace("-", "").Length == 11 &&
                   cedula.Replace("-", "").All(char.IsDigit);
        }

        private bool ValidarFormatoTelefono(string telefono)
        {
            var numeroLimpio = new string(telefono.Where(char.IsDigit).ToArray());
            return numeroLimpio.Length == 10 &&
                   (numeroLimpio.StartsWith("809") ||
                    numeroLimpio.StartsWith("829") ||
                    numeroLimpio.StartsWith("849"));
        }
    }
}
