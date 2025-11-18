using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Solicitud
    {
        [Key]
        public Guid Id { get; set; }

        public string Descripcion { get; set; } 

        public DateTime Fecha { get; set; } 

        public string Estado { get; set; } 

        public string Solicitante { get; set; }
        
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public Guid SalaId { get; set; }
        // Relación removida para evitar ciclo de cascada
        // Acceder a la Sala a través de consultas manuales si es necesario
        // public Sala Sala { get; set; }
    }

}
