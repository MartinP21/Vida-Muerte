using DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public partial class Estado
    {
        public Estado()
        {
            Cita = new HashSet<Cita>();
        }

        public int IdEstado { get; set; }
        public string? NombreEstado { get; set; }
        public string? Descripcion { get; set; }

        public virtual ICollection<Cita> Cita { get; set; }
    }
}
