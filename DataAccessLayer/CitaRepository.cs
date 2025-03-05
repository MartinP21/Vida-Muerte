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
            // Calcula el número de registros a saltar
            int offset = (pagina - 1) * registrosPorPagina;

            // Crea una sonsulta sobre la tabla Citas
            IQueryable<Cita> query = _context.Citas
                // Si 'idEstado' tiene un valor, filtra por ese estado. Si es 'NULL', trae todas las citas
                .Where(c => !idEstado.HasValue || c.IdEstado == idEstado.Value) // Filtrar antes de paginar
                .OrderByDescending(c => c.Id) // Ordena por el 'ID' de la cita de manera descendente
                .Skip(offset) // Salta los primeros registros para ir a la página deseada
                .Take(registrosPorPagina); // Toma la cantidad de registros especificados por 'registrosPorPagina'

            // Pasa la consulta y la convierte en una lista y la asigna a la variable 'citas'
            var citas = await query.ToListAsync();

            foreach (var cita in citas)
            {
                await _context.Entry(cita).Reference(c => c.IdEstadoNavigation).LoadAsync();
            }

            return citas;
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
