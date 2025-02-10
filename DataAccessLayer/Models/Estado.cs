using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Table("Estado")]
    public partial class Estado
    {
        public Estado()
        {
            Cita = new HashSet<Cita>();
        }

        [Key]
        public int IdEstado { get; set; }
        [StringLength(14)]
        public string? NombreEstado { get; set; }
        [StringLength(100)]
        public string? Descripcion { get; set; }

        [InverseProperty("IdEstadoNavigation")]
        public virtual ICollection<Cita> Cita { get; set; }
    }
}
