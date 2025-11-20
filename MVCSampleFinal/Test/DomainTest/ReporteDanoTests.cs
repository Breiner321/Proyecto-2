using Domain;

namespace DomainTest
{
    [TestFixture]
    public class ReporteDanoTests
    {
        [Test]
        public void ReporteDano_Properties_ShouldBeInitialized()
        {
            // Arrange & Act
            var reporte = new ReporteDano
            {
                Id = Guid.NewGuid(),
                Tipo = "Equipo",
                EquipoId = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Descripcion = "Pantalla rota",
                Fecha = DateTime.Now,
                Estado = "Pendiente",
                Observaciones = null
            };

            // Assert
            Assert.That(reporte.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(reporte.Tipo, Is.EqualTo("Equipo"));
            Assert.That(reporte.EquipoId, Is.Not.Null);
            Assert.That(reporte.SalaId, Is.Null);
            Assert.That(reporte.UsuarioId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(reporte.Descripcion, Is.EqualTo("Pantalla rota"));
            Assert.That(reporte.Fecha, Is.Not.EqualTo(default(DateTime)));
            Assert.That(reporte.Estado, Is.EqualTo("Pendiente"));
        }

        [Test]
        public void ReporteDano_Sala_ShouldSetSalaId()
        {
            // Arrange & Act
            var reporte = new ReporteDano
            {
                Id = Guid.NewGuid(),
                Tipo = "Sala",
                SalaId = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Descripcion = "Aire acondicionado no funciona",
                Fecha = DateTime.Now,
                Estado = "Pendiente"
            };

            // Assert
            Assert.That(reporte.Tipo, Is.EqualTo("Sala"));
            Assert.That(reporte.SalaId, Is.Not.Null);
            Assert.That(reporte.EquipoId, Is.Null);
        }

        [Test]
        public void ReporteDano_Estado_DefaultShouldBePendiente()
        {
            // Arrange & Act
            var reporte = new ReporteDano
            {
                Id = Guid.NewGuid(),
                Tipo = "Equipo",
                UsuarioId = Guid.NewGuid(),
                Descripcion = "Test",
                Fecha = DateTime.Now
            };

            // Assert
            Assert.That(reporte.Estado, Is.EqualTo("Pendiente"));
        }
    }
}

