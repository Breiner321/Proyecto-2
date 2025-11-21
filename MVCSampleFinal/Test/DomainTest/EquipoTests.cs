using Domain;
using Xunit;

namespace DomainTest
{
    public class EquipoTests
    {
        [Fact]
        public void Equipo_Constructor_ShouldInitializeSolicitudesEquipoList()
        {
            // Arrange & Act
            var equipo = new Equipo();

            // Assert
            Assert.NotNull(equipo.SolicitudesEquipo);
            Assert.Empty(equipo.SolicitudesEquipo);
        }

        [Fact]
        public void Equipo_Properties_ShouldBeInitialized()
        {
            // Arrange & Act
            var equipo = new Equipo
            {
                Id = Guid.NewGuid(),
                Nombre = "Laptop Dell",
                Estado = "Libre",
                Ubicacion = "Sala A",
                SalaId = Guid.NewGuid(),
                Disponible = true
            };

            // Assert
            Assert.NotEqual(Guid.Empty, equipo.Id);
            Assert.Equal("Laptop Dell", equipo.Nombre);
            Assert.Equal("Libre", equipo.Estado);
            Assert.Equal("Sala A", equipo.Ubicacion);
            Assert.NotEqual(Guid.Empty, equipo.SalaId);
            Assert.True(equipo.Disponible);
        }

        [Fact]
        public void Equipo_Nombre_CanBeNull()
        {
            // Arrange & Act
            var equipo = new Equipo
            {
                Id = Guid.NewGuid(),
                Nombre = null,
                Estado = "Libre"
            };

            // Assert
            Assert.Null(equipo.Nombre);
        }
    }
}
