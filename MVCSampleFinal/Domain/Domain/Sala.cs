using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Sala
    {
        [Key]
        public Guid Id { get; set; }

        public string Numero { get; set; }

        public int Capacidad { get; set; }

        public string Ubicacion { get; set; }

        public string Estado { get; set; }

        public List<Equipo> Equipos { get; set; }  
        
        public void AdicionarEquipo(Equipo equipo)
        {
            Equipos.Add(equipo);
        }
        }
}
