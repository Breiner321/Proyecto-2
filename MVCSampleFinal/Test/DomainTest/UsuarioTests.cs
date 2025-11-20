using Domain;

namespace DomainTest
{
    [TestFixture]
    public class UsuarioTests
    {
        [Test]
        public void Usuario_Constructor_ShouldInitializeLists()
        {
            // Arrange & Act
            var usuario = new Usuario();

            // Assert
            Assert.That(usuario.Solicitudes, Is.Not.Null);
            Assert.That(usuario.Solicitudes, Is.Empty);
            Assert.That(usuario.SolicitudesEquipo, Is.Not.Null);
            Assert.That(usuario.SolicitudesEquipo, Is.Empty);
        }

        [Test]
        public void Usuario_Properties_ShouldBeInitialized()
        {
            // Arrange & Act
            var usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Nombre = "Juan Pérez",
                Correo = "juan@example.com",
                Contraseña = "password123",
                Rol = "Estudiante"
            };

            // Assert
            Assert.That(usuario.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(usuario.Nombre, Is.EqualTo("Juan Pérez"));
            Assert.That(usuario.Correo, Is.EqualTo("juan@example.com"));
            Assert.That(usuario.Contraseña, Is.EqualTo("password123"));
            Assert.That(usuario.Rol, Is.EqualTo("Estudiante"));
        }

        [Test]
        public void AdicionarSolicitud_ShouldAddSolicitudToSolicitudesList()
        {
            // Arrange
            var usuario = new Usuario { Id = Guid.NewGuid() };
            var solicitud = new Solicitud
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuario.Id,
                Descripcion = "Reserva de sala",
                Fecha = DateTime.Now,
                Estado = "Pendiente"
            };

            // Act
            usuario.AdicionarSolicitud(solicitud);

            // Assert
            Assert.That(usuario.Solicitudes, Has.Count.EqualTo(1));
            Assert.That(usuario.Solicitudes[0], Is.EqualTo(solicitud));
        }

        [Test]
        public void AdicionarSolicitudEquipo_ShouldAddSolicitudEquipoToSolicitudesEquipoList()
        {
            // Arrange
            var usuario = new Usuario { Id = Guid.NewGuid() };
            var solicitudEquipo = new SolicitudEquipo
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuario.Id,
                Descripcion = "Reserva de equipo",
                Fecha = DateTime.Now,
                Estado = "Pendiente"
            };

            // Act
            usuario.AdicionarSolicitudEquipo(solicitudEquipo);

            // Assert
            Assert.That(usuario.SolicitudesEquipo, Has.Count.EqualTo(1));
            Assert.That(usuario.SolicitudesEquipo[0], Is.EqualTo(solicitudEquipo));
        }
    }
}

