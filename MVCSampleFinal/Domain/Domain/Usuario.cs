using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{

        public class Usuario
        {
            public Guid Id { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public string Correo { get; set; } = string.Empty;
            public string Contraseña { get; set; } = string.Empty;
            public string Rol { get; set; } = string.Empty;
            public List<Solicitud> Solicitudes { get; set; } = new();

        public void AdicionarSolicitud(Solicitud solicitud)
            {
               Solicitudes.Add(solicitud);

            }

        }


}

