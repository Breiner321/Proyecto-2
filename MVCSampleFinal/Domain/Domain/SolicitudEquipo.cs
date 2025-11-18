using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class SolicitudEquipo
    {
        [Key]
        public Guid Id { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }

        public string Estado { get; set; } = string.Empty;

        public string Solicitante { get; set; } = string.Empty;
        
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        
        public Guid EquipoId { get; set; }
        public Equipo Equipo { get; set; }
    }
}

