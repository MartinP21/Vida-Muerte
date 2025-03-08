using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface ICitaService
    {
        Task<(IEnumerable<Cita> citas, int totalRegistros)> ObtenerCitasPorEstadoPaginadasAsync(int pagina, int registrosPorPagina, int? idEstado);
        Task<int> ObtenerTotalCitasAsync();
        Task<Cita> ObtenerCitasPorIdAsync(int Id);
        Task CrearCitaAsync(Cita cita);
        Task ActualizarCitaAsync(Cita cita);
        Task DeshabilitarCitaAsync(int Id, string motivo);
    }
}
