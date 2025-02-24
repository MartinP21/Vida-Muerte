﻿using DataAccessLayer.Models;

namespace DataAccessLayer.Interfaces
{
    public interface ICitaRepository
    {
        Task<IEnumerable<Cita>> ObtenerCitasPorEstadoPaginadasAsync(int pagina, int registrosPorPagina, int? idEstado);
        Task<int> ObtenerTotalCitasPorEstadoAsync(int? idEstado);
        Task<IEnumerable<Cita>> ObtenerCitasAsync();
        Task<Cita> ObtenerCitasPorIdAsync(int Id);
        Task CrearCitaAsync(Cita cita);
        Task ActualizarCitaAsync(Cita cita);
        Task DeshabilitarCitaAsync(int Id, string motivo);
    }
}
