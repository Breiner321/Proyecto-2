using Domain;

namespace DomainTest
{
    [TestFixture]
    public class SalaTests
    {
        [Test]
        public void Sala_Constructor_ShouldInitializeEquiposList()
        {
            // Arrange & Act
            var sala = new Sala();

            // Assert
            Assert.That(sala.Equipos, Is.Not.Null);
            Assert.That(sala.Equipos, Is.Empty);
        }

        [Test]
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
            Assert.That(sala.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(sala.Numero, Is.EqualTo("A"));
            Assert.That(sala.Capacidad, Is.EqualTo(30));
            Assert.That(sala.Ubicacion, Is.EqualTo("Edificio Principal"));
            Assert.That(sala.Estado, Is.EqualTo("Disponible"));
        }

        [Test]
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
            Assert.That(sala.Equipos, Has.Count.EqualTo(1));
            Assert.That(sala.Equipos[0], Is.EqualTo(equipo));
        }

        [Test]
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
            Assert.That(sala.Equipos, Has.Count.EqualTo(2));
        }
    }
}

