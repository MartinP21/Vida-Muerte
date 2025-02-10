using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CitaRepository : ICitaRepository
    {
        private readonly AppDbContext _context;
        public CitaRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<Cita> ActualizarCitaAsync(Cita cita)
        {
            throw new NotImplementedException();
        }

        public Task<Cita> CrearCitaAsync(Cita cita)
        {
            throw new NotImplementedException();
        }

        public Task<Cita> DeshabilitarCitaAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Cita>> ObtenerCitasAsync(Cita cita)
        {
            throw new NotImplementedException();
        }

        public Task<Cita> ObtenerCitasPorIdAsync(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
