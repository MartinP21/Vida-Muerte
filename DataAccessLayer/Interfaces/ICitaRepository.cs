using DataAccessLayer.Models;

namespace DataAccessLayer.Interfaces
{
    public interface ICitaRepository
    {
        Task<IEnumerable<Cita>> ObtenerCitasPorEstadoPaginadasAsync(int pagina, int registrosPorPagina, int? idEstado);
        Task<int> ObtenerTotalCitasAsync();
        Task<int> ObtenerTotalCitasPorEstadoAsync(int? idEstado);
        Task<Cita> ObtenerCitasPorIdAsync(int Id);
        Task CrearCitaAsync(Cita cita);
        Task ActualizarCitaAsync(Cita cita);
        Task DeshabilitarCitaAsync(int Id, string motivo);
    }
}
