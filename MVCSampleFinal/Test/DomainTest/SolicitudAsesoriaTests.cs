using Domain;
using Xunit;

namespace DomainTest
{
    public class SolicitudAsesoriaTests
    {
        [Fact]
        public void SolicitudAsesoria_Properties_ShouldBeInitialized()
        {
            // Arrange & Act
            var solicitud = new SolicitudAsesoria
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Descripcion = "Necesito ayuda con el software",
                TipoAsesoria = "Técnica",
                Fecha = DateTime.Now,
                FechaHoraSolicitada = DateTime.Now.AddDays(1),
                Estado = "Pendiente",
                Observaciones = null,
                Solicitante = "Carlos López"
            };

            // Assert
            Assert.NotEqual(Guid.Empty, solicitud.Id);
            Assert.NotEqual(Guid.Empty, solicitud.UsuarioId);
            Assert.Equal("Necesito ayuda con el software", solicitud.Descripcion);
            Assert.Equal("Técnica", solicitud.TipoAsesoria);
            Assert.NotEqual(default(DateTime), solicitud.Fecha);
            Assert.NotNull(solicitud.FechaHoraSolicitada);
            Assert.Equal("Pendiente", solicitud.Estado);
            Assert.Equal("Carlos López", solicitud.Solicitante);
        }

        [Fact]
        public void SolicitudAsesoria_FechaHoraSolicitada_CanBeNull()
        {
            // Arrange & Act
            var solicitud = new SolicitudAsesoria
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Descripcion = "Consulta general",
                TipoAsesoria = "Técnica",
                Fecha = DateTime.Now,
                FechaHoraSolicitada = null,
                Estado = "Pendiente"
            };

            // Assert
            Assert.Null(solicitud.FechaHoraSolicitada);
        }

        [Fact]
        public void SolicitudAsesoria_Estado_DefaultShouldBePendiente()
        {
            // Arrange & Act
            var solicitud = new SolicitudAsesoria
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Descripcion = "Test",
                TipoAsesoria = "Técnica",
                Fecha = DateTime.Now
            };

            // Assert
            Assert.Equal("Pendiente", solicitud.Estado);
        }
    }
}
