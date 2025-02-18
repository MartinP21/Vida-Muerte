using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    public partial class Cita
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Cedula { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public DateTime FechaCita { get; set; }
        public int IdEstado { get; set; }
        public string? Motivo { get; set; }

        [ForeignKey("IdEstado")]
        [InverseProperty("Cita")]
        public virtual Estado? IdEstadoNavigation { get; set; } = null!;

        [NotMapped]
        public string NombreEstado => IdEstadoNavigation?.NombreEstado ?? "Desconocido";
        public void LimpiarCampos()
        {
            Nombre = string.IsNullOrWhiteSpace(Nombre) ? null : Nombre.Trim();
            Apellidos = string.IsNullOrWhiteSpace(Apellidos) ? null : Apellidos.Trim();
        }
    }
}
