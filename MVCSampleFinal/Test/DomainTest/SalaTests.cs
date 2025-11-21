using Domain;
using Xunit;

namespace DomainTest
{
    public class SalaTests
    {
        [Fact]
        public void Sala_Constructor_ShouldInitializeEquiposList()
        {
            // Arrange & Act
            var sala = new Sala();

            // Assert
            Assert.NotNull(sala.Equipos);
            Assert.Empty(sala.Equipos);
        }

        [Fact]
        public void Sala_Properties_ShouldBeInitialized()
        {
            // Arrange & Act
            var sala = new Sala
            {
                Id = Guid.NewGuid(),
                Numero = "A",
                Capacidad = 30,
                Ubicacion = "Edificio Principal",
                Estado = "Disponible"
            };

            // Assert
            Assert.NotEqual(Guid.Empty, sala.Id);
            Assert.Equal("A", sala.Numero);
            Assert.Equal(30, sala.Capacidad);
            Assert.Equal("Edificio Principal", sala.Ubicacion);
            Assert.Equal("Disponible", sala.Estado);
        }

        [Fact]
        public void AdicionarEquipo_ShouldAddEquipoToEquiposList()
        {
            // Arrange
            var sala = new Sala { Id = Guid.NewGuid() };
            var equipo = new Equipo 
            { 
                Id = Guid.NewGuid(), 
                Nombre = "Laptop 1",
                SalaId = sala.Id
            };

            // Act
            sala.AdicionarEquipo(equipo);

            // Assert
            Assert.Single(sala.Equipos);
            Assert.Equal(equipo, sala.Equipos[0]);
        }

        [Fact]
        public void AdicionarEquipo_MultipleEquipos_ShouldAddAllEquipos()
        {
            // Arrange
            var sala = new Sala { Id = Guid.NewGuid() };
            var equipo1 = new Equipo { Id = Guid.NewGuid(), Nombre = "Laptop 1", SalaId = sala.Id };
            var equipo2 = new Equipo { Id = Guid.NewGuid(), Nombre = "Laptop 2", SalaId = sala.Id };

            // Act
            sala.AdicionarEquipo(equipo1);
            sala.AdicionarEquipo(equipo2);

            // Assert
            Assert.Equal(2, sala.Equipos.Count);
        }
    }
}
