using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Equipo
    {
        [Key]
        public Guid Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Estado { get; set; } = string.Empty;

        public string Ubicacion { get; set; } = string.Empty;
        public Guid SalaId { get; set; }
        public sala Sala { get; set; }

        public bool Disponible { get; set; }
    }
}
