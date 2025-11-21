using Domain;
using Xunit;

namespace DomainTest
{
    public class SolicitudEquipoTests
    {
        [Fact]
        public void SolicitudEquipo_Properties_ShouldBeInitialized()
        {
            // Arrange & Act
            var solicitud = new SolicitudEquipo
            {
                Id = Guid.NewGuid(),
                Descripcion = "Reserva de laptop para presentación",
                Fecha = DateTime.Now,
                FechaHoraInicio = DateTime.Now.AddHours(2),
                FechaHoraFin = DateTime.Now.AddHours(4),
                Estado = "Pendiente",
                Solicitante = "María García",
                UsuarioId = Guid.NewGuid(),
                EquipoId = Guid.NewGuid()
            };

            // Assert
            Assert.NotEqual(Guid.Empty, solicitud.Id);
            Assert.Equal("Reserva de laptop para presentación", solicitud.Descripcion);
            Assert.NotEqual(default(DateTime), solicitud.Fecha);
            Assert.NotNull(solicitud.FechaHoraInicio);
            Assert.NotNull(solicitud.FechaHoraFin);
            Assert.Equal("Pendiente", solicitud.Estado);
            Assert.Equal("María García", solicitud.Solicitante);
            Assert.NotEqual(Guid.Empty, solicitud.UsuarioId);
            Assert.NotEqual(Guid.Empty, solicitud.EquipoId);
        }

        [Fact]
        public void SolicitudEquipo_FechaHoraInicio_CanBeNull()
        {
            // Arrange & Act
            var solicitud = new SolicitudEquipo
            {
                Id = Guid.NewGuid(),
                Descripcion = "Reserva sin fecha específica",
                Fecha = DateTime.Now,
                FechaHoraInicio = null,
                FechaHoraFin = null,
                Estado = "Pendiente"
            };

            // Assert
            Assert.Null(solicitud.FechaHoraInicio);
            Assert.Null(solicitud.FechaHoraFin);
        }
    }
}
