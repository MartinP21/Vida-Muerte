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
        public async Task<bool> ExisteCedulaAsync(string cedula)
        {
            return await _context.Citas.AnyAsync(c => c.Cedula == cedula);
        }
        public IEnumerable<Cita> ObtenerCitas()
        {
            return _context.Citas.ToList();
        }
        public async Task CrearCitaAsync(Cita cita)
        {
            // Limpiar espacios en blanco
            cita.Nombre = cita.Nombre.Trim();
            cita.Apellidos = cita.Apellidos.Trim();
            cita.Cedula = cita.Cedula.Trim();
            cita.Telefono = cita.Telefono.Trim();

            // Establecer estado predeterminado
            cita.IdEstado = 1; // Pendiente

            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarCitaAsync(Cita cita)
        {
            // Limpiar espacios en blanco
            cita.Nombre = cita.Nombre.Trim();
            cita.Apellidos = cita.Apellidos.Trim();
            cita.Cedula = cita.Cedula.Trim();
            cita.Telefono = cita.Telefono.Trim();

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
