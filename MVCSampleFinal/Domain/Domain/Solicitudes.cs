using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    internal class Solicitudes
    {
        [Key]
        public Guid Id { get; set; }

        public string Descripcion { get; set; }

        public DateTime Fecha { get; set; }

        public string Estado { get; set; }

        public string Solicitante { get; set; }
    }
}
