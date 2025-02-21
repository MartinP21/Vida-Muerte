using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class CitaRepository : ICitaRepository
    {
        private readonly AppDbContext _context;
        // Constructor que inyecta el contexto de la base de datos
        public CitaRepository(AppDbContext context)
        {
            _context = context;
        }

        // Método para obtener las citas filtradas y paginadas
        public async Task<IEnumerable<Cita>> ObtenerCitasPorEstadoPaginadasAsync(int pagina, int registrosPorPagina, int? idEstado)
        {
            // Convierte la colección en una consulta para modificarla dinámicamente
            var query = _context.Citas
                                .Include(c => c.IdEstadoNavigation)
                                .AsQueryable();

            // Filtra las citas por el id del estado
            if (idEstado.HasValue)
            {
                query = query.Where(c => c.IdEstado == idEstado.Value);
            }

            // Ordena las citas por fecha de manera descendiente
            query = query.OrderByDescending(c => c.Id)
                         .Skip((pagina - 1) * registrosPorPagina)
                         .Take(registrosPorPagina);

            return await query.ToListAsync();
        }

        // Método para obtener el total de registro filtrado
        public async Task<int> ObtenerTotalCitasPorEstadoAsync(int? idEstado)
        {
            // Convierte la colección en una consulta para modificarla dinámicamente
            var query = _context.Citas.AsQueryable();

            // Filtra las citas por el id del estado
            if (idEstado.HasValue)
            {
                query = query.Where(c => c.IdEstado == idEstado.Value);
            }

            return await query.CountAsync();
        }

        // Método asincrono para obtener todas las citas con su estado asociado
        public async Task<IEnumerable<Cita>> ObtenerCitasAsync()
        {
            return await _context.Citas
                .Include(c => c.IdEstadoNavigation)
                .OrderByDescending(c => c.Id)
                .ToListAsync();
        }

        // Método asincrono para obtener una cita por su ID
        public async Task<Cita> ObtenerCitasPorIdAsync(int Id)
        {
            var cita = await _context.Citas
                             .Include(c => c.IdEstadoNavigation) // Incluye la relación con Estado
                             .FirstOrDefaultAsync(c => c.Id == Id);
            if (cita == null)
            {
                throw new KeyNotFoundException($"ERROR: No se encontro la cita con ID: {Id}");
            }
            return cita;
        }
        // Método asincrono para verificar si una cédula ya existe en la base de datos
        public async Task<bool> ExisteCedulaAsync(string cedula, int? idCita = null)
        {
            return await _context.Citas.AnyAsync(c =>
                c.Cedula == cedula && (!idCita.HasValue || c.Id != idCita.Value));
        }

        // Método síncrono para obtener todas las citas
        public IEnumerable<Cita> ObtenerCitas()
        {
            return _context.Citas.ToList(); // Retorna todas las citas en la base de datos
        }
        // Método asincrono para crear una nueva cita
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
        // Método asincrono para actualizar una cita existente
        public async Task ActualizarCitaAsync(Cita cita)
        {
            // Limpiar espacios en blanco
            cita.Nombre = cita.Nombre.Trim();
            cita.Apellidos = cita.Apellidos.Trim();
            cita.Cedula = cita.Cedula.Trim();
            cita.Telefono = cita.Telefono.Trim();

            // Desconectar cualquier entidad existente con el mismo ID
            var citaExistente = await _context.Citas.FindAsync(cita.Id);
            if (citaExistente == null)
            {
                throw new Exception($"No se encontró la cita con ID {cita.Id}");
            }

            _context.Entry(citaExistente).State = EntityState.Detached; // Desconecta 'citaExistente' del seguimiento para evitar conflictos.
            _context.Entry(cita).State = EntityState.Modified; // Marca la entidad 'cita' como modificada para que EF actualice sus cambios.

            await _context.SaveChangesAsync();
        }
        // Método asincrono para deshabilitar una cita cambiando su estado a 'Deshabilitado'
        public async Task DeshabilitarCitaAsync(int Id, string motivo)
        {
            var cita = await _context.Citas.FindAsync(Id);
            //Si la cita existe entonces cambia el estado a 'Deshabilitado'
            if (cita != null)
            {
                cita.IdEstado = 3; // Deshabilitado
                cita.Motivo = motivo;
                _context.Citas.Update(cita);
                await _context.SaveChangesAsync();
            }
        }

    }
}
