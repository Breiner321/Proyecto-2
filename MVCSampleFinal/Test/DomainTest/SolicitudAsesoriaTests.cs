using Domain;

namespace DomainTest
{
    [TestFixture]
    public class SolicitudAsesoriaTests
    {
        [Test]
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
            Assert.That(solicitud.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(solicitud.UsuarioId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(solicitud.Descripcion, Is.EqualTo("Necesito ayuda con el software"));
            Assert.That(solicitud.TipoAsesoria, Is.EqualTo("Técnica"));
            Assert.That(solicitud.Fecha, Is.Not.EqualTo(default(DateTime)));
            Assert.That(solicitud.FechaHoraSolicitada, Is.Not.Null);
            Assert.That(solicitud.Estado, Is.EqualTo("Pendiente"));
            Assert.That(solicitud.Solicitante, Is.EqualTo("Carlos López"));
        }

        [Test]
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
            Assert.That(solicitud.FechaHoraSolicitada, Is.Null);
        }

        [Test]
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
            Assert.That(solicitud.Estado, Is.EqualTo("Pendiente"));
        }
    }
}

