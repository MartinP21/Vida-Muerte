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
        Task<IEnumerable<Cita>> ObtenerCitasAsync();
        Task<Cita> ObtenerCitasPorIdAsync(int Id);
        Task CrearCitaAsync(Cita cita);
        Task ActualizarCitaAsync(Cita cita);
        Task DeshabilitarCitaAsync(int Id);
    }
}
