using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class ReporteDano
    {
        [Key]
        public Guid Id { get; set; }

        public string Tipo { get; set; } = string.Empty; // "Equipo" o "Sala"

        public Guid? EquipoId { get; set; }
        public Equipo? Equipo { get; set; }

        public Guid? SalaId { get; set; }
        // No agregamos navegación a Sala para evitar problemas de cascada

        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }

        public string Estado { get; set; } = "Pendiente"; // Pendiente, En Revisión, Resuelto, Rechazado

        public string? Observaciones { get; set; }
    }
}


