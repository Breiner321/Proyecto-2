using Domain;
using Xunit;

namespace DomainTest
{
    public class UsuarioTests
    {
        [Fact]
        public void Usuario_Constructor_ShouldInitializeLists()
        {
            // Arrange & Act
            var usuario = new Usuario();

            // Assert
            Assert.NotNull(usuario.Solicitudes);
            Assert.Empty(usuario.Solicitudes);
            Assert.NotNull(usuario.SolicitudesEquipo);
            Assert.Empty(usuario.SolicitudesEquipo);
        }

        [Fact]
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
            Assert.NotEqual(Guid.Empty, usuario.Id);
            Assert.Equal("Juan Pérez", usuario.Nombre);
            Assert.Equal("juan@example.com", usuario.Correo);
            Assert.Equal("password123", usuario.Contraseña);
            Assert.Equal("Estudiante", usuario.Rol);
        }

        [Fact]
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
            Assert.Single(usuario.Solicitudes);
            Assert.Equal(solicitud, usuario.Solicitudes[0]);
        }

        [Fact]
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
            Assert.Single(usuario.SolicitudesEquipo);
            Assert.Equal(solicitudEquipo, usuario.SolicitudesEquipo[0]);
        }
    }
}
