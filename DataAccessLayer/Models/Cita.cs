using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Index("Cedula", Name = "UQ__Citas__B4ADFE38FCBED69B", IsUnique = true)]
    public partial class Cita
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string Nombre { get; set; } = null!;
        [StringLength(100)]
        public string Apellidos { get; set; } = null!;
        [StringLength(15)]
        [Unicode(false)]
        public string Cedula { get; set; } = null!;
        [StringLength(15)]
        [Unicode(false)]
        public string Telefono { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime FechaCita { get; set; }
        public int IdEstado { get; set; }

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
