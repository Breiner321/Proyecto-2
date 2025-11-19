using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class SolicitudAsesoria
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public string TipoAsesoria { get; set; } = string.Empty; // "Técnica", "Equipo", "Sala", etc.

        public DateTime Fecha { get; set; }

        public DateTime? FechaHoraSolicitada { get; set; }

        public string Estado { get; set; } = "Pendiente"; // Pendiente, Aprobada, Rechazada, Finalizada

        public string? Observaciones { get; set; }

        public string Solicitante { get; set; } = string.Empty;
    }
}


