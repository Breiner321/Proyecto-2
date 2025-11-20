using Domain;

namespace DomainTest
{
    [TestFixture]
    public class EquipoTests
    {
        [Test]
        public void Equipo_Constructor_ShouldInitializeSolicitudesEquipoList()
        {
            // Arrange & Act
            var equipo = new Equipo();

            // Assert
            Assert.That(equipo.SolicitudesEquipo, Is.Not.Null);
            Assert.That(equipo.SolicitudesEquipo, Is.Empty);
        }

        [Test]
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
            Assert.That(equipo.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(equipo.Nombre, Is.EqualTo("Laptop Dell"));
            Assert.That(equipo.Estado, Is.EqualTo("Libre"));
            Assert.That(equipo.Ubicacion, Is.EqualTo("Sala A"));
            Assert.That(equipo.SalaId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(equipo.Disponible, Is.True);
        }

        [Test]
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
            Assert.That(equipo.Nombre, Is.Null);
        }
    }
}

