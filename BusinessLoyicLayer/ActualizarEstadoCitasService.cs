using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using DataAccessLayer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using DataAccessLayer.Models;

namespace BusinessLogicLayer
{
    // IHostedService permite que el servicio se detenga junto con la aplicación y IDisposable libera recursos cuando el servicio se finaliza
    public class ActualizarEstadoCitasService : IHostedService, IDisposable
    {
        // Temporizador para programar la ejecución periódica de la actualización.
        private Timer _timer;
        // Se crea metodo IServiceScopeFactory que sirve para crear un ambito de servicios y obtener instancias de otros servicios
        private readonly IServiceScopeFactory _scopeFactory;

        // Constructor con inyección de dependencias
        public ActualizarEstadoCitasService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        // Método que se ejecuta al iniciar el servicio
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Configura el temporizador para que se ejecute cada hora
            _timer = new Timer(ActualizarEstados, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        // Método asíncrono que se ejecuta periódicamente para actualizar los estados de las citas
        private async void ActualizarEstados(object state)
        {
            // Crea un nuevo scope para resolver las dependencias de forma aislada.
            using (var scope = _scopeFactory.CreateScope())
            {
                // Obtiene una instancia del repositorio de citas
                var citaRepository = scope.ServiceProvider.GetRequiredService<ICitaRepository>();

                // Variables para la paginación
                int pagina = 1;
                int registrosPorPagina = 100;
                List<Cita> citas = new List<Cita>();

                // Cargamos citas paginadas (estado = 1, es decir, pendientes)
                while (true)
                {
                    var citasPagina = await citaRepository.ObtenerCitasPorEstadoPaginadasAsync(pagina, registrosPorPagina, 1);
                    if (!citasPagina.Any()) break;
                    citas.AddRange(citasPagina);
                    pagina++;
                }

                // Verifica si la lista de citas es nula o está vacía
                if (!citas.Any())
                {
                    return; // Si no hay citas, termina la ejecución del método
                }

                // Itera sobre cada cita obtenida y actualiza el estado si corresponde
                foreach (var cita in citas)
                {
                    // Comprueba si la fecha de la cita es anterior a la fecha actual y si el estado es Pendiente (1)
                    if (cita.FechaCita < DateTime.Now && cita.IdEstado == 1)
                    {
                        // Cambia el estado de la cita a Completada (2)
                        cita.IdEstado = 2;
                        // Actualiza la cita en la base de datos
                        await citaRepository.ActualizarCitaAsync(cita);
                    }
                }
            }
        }

        // Método que se ejecuta al detener el servicio
        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Detiene el temporizador para que no se sigan ejecutando llamadas al método de actualización
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        // Método para liberar recursos.
        public void Dispose()
        {
            _timer?.Dispose();  // Libera el recurso del temporizador
        }
    }
}
