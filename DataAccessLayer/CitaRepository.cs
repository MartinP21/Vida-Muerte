using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class CitaRepository : ICitaRepository
    {
        private readonly AppDbContext _context;
        public CitaRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Cita>> ObtenerCitasAsync()
        {
            return await _context.Citas.Include(c => c.IdEstadoNavigation).ToListAsync();
        }

        public async Task<Cita> ObtenerCitasPorIdAsync(int Id)
        {
            var cita = await _context.Citas.FindAsync();
            if (cita == null)
            {
                throw new KeyNotFoundException($"ERROR: No se encontro la cita con ID: {Id}");
            }
            return cita;
        }
        public async Task CrearCitaAsync(Cita cita)
        {
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarCitaAsync(Cita cita)
        {
            _context.Citas.Update(cita);
            await _context.SaveChangesAsync();
        }

        public async Task DeshabilitarCitaAsync(int Id)
        {
            var cita = await _context.Citas.FindAsync();
            if (cita != null)
            {
                cita.IdEstado = 3;
                _context.Citas.Update(cita);
                await _context.SaveChangesAsync();
            }
        }

    }
}
