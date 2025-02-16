using DataAccessLayer.Models;

namespace DataAccessLayer.Interfaces
{
    public interface ICitaRepository
    {
        Task<IEnumerable<Cita>> ObtenerCitasAsync();
        Task<Cita> ObtenerCitasPorIdAsync(int Id);
        Task<bool> ExisteCedulaAsync(string cedula);
        Task CrearCitaAsync(Cita cita);
        Task ActualizarCitaAsync(Cita cita);
        Task DeshabilitarCitaAsync(int Id);
    }
}
