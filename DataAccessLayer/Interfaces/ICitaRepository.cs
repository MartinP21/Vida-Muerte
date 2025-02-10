using DataAccessLayer.Models;

namespace DataAccessLayer.Interfaces
{
    public interface ICitaRepository
    {
        Task<IEnumerable<Cita>> ObtenerCitasAsync(Cita cita);
        Task<Cita> ObtenerCitasPorIdAsync(int Id);
        Task<Cita> CrearCitaAsync(Cita cita);
        Task<Cita> ActualizarCitaAsync(Cita cita);
        Task<Cita> DeshabilitarCitaAsync(int Id);
    }
}
