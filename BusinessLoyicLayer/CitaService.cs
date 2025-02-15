using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
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
        public async Task CrearCitaAsync(Cita cita)
        {
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
    }
}
