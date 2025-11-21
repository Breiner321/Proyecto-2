using Domain;

namespace DomainTest
{
    [TestFixture]
    public class SolicitudEquipoTests
    {
        [Test]
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
            Assert.That(solicitud.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(solicitud.Descripcion, Is.EqualTo("Reserva de laptop para presentación"));
            Assert.That(solicitud.Fecha, Is.Not.EqualTo(default(DateTime)));
            Assert.That(solicitud.FechaHoraInicio, Is.Not.Null);
            Assert.That(solicitud.FechaHoraFin, Is.Not.Null);
            Assert.That(solicitud.Estado, Is.EqualTo("Pendiente"));
            Assert.That(solicitud.Solicitante, Is.EqualTo("María García"));
            Assert.That(solicitud.UsuarioId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(solicitud.EquipoId, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
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
            Assert.That(solicitud.FechaHoraInicio, Is.Null);
            Assert.That(solicitud.FechaHoraFin, Is.Null);
        }
    }
}


