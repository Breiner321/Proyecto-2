using Domain;
using Xunit;

namespace DomainTest
{
    public class SolicitudTests
    {
        [Fact]
        public void Solicitud_Properties_ShouldBeInitialized()
        {
            // Arrange & Act
            var solicitud = new Solicitud
            {
                Id = Guid.NewGuid(),
                Descripcion = "Reserva de sala para proyecto",
                Fecha = DateTime.Now,
                FechaHoraInicio = DateTime.Now.AddHours(1),
                FechaHoraFin = DateTime.Now.AddHours(3),
                Estado = "Pendiente",
                Solicitante = "Juan Pérez",
                UsuarioId = Guid.NewGuid(),
                SalaId = Guid.NewGuid()
            };

            // Assert
            Assert.NotEqual(Guid.Empty, solicitud.Id);
            Assert.Equal("Reserva de sala para proyecto", solicitud.Descripcion);
            Assert.NotEqual(default(DateTime), solicitud.Fecha);
            Assert.NotNull(solicitud.FechaHoraInicio);
            Assert.NotNull(solicitud.FechaHoraFin);
            Assert.Equal("Pendiente", solicitud.Estado);
            Assert.Equal("Juan Pérez", solicitud.Solicitante);
            Assert.NotEqual(Guid.Empty, solicitud.UsuarioId);
            Assert.NotEqual(Guid.Empty, solicitud.SalaId);
        }

        [Fact]
        public void Solicitud_FechaHoraInicio_CanBeNull()
        {
            // Arrange & Act
            var solicitud = new Solicitud
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
