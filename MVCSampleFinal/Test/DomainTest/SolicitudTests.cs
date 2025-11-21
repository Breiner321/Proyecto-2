using Domain;

namespace DomainTest
{
    [TestFixture]
    public class SolicitudTests
    {
        [Test]
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
            Assert.That(solicitud.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(solicitud.Descripcion, Is.EqualTo("Reserva de sala para proyecto"));
            Assert.That(solicitud.Fecha, Is.Not.EqualTo(default(DateTime)));
            Assert.That(solicitud.FechaHoraInicio, Is.Not.Null);
            Assert.That(solicitud.FechaHoraFin, Is.Not.Null);
            Assert.That(solicitud.Estado, Is.EqualTo("Pendiente"));
            Assert.That(solicitud.Solicitante, Is.EqualTo("Juan Pérez"));
            Assert.That(solicitud.UsuarioId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(solicitud.SalaId, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
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
            Assert.That(solicitud.FechaHoraInicio, Is.Null);
            Assert.That(solicitud.FechaHoraFin, Is.Null);
        }
    }
}


