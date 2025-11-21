using Domain;
using Xunit;

namespace DomainTest
{
    public class ReporteDanoTests
    {
        [Fact]
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
            Assert.NotEqual(Guid.Empty, reporte.Id);
            Assert.Equal("Equipo", reporte.Tipo);
            Assert.NotNull(reporte.EquipoId);
            Assert.Null(reporte.SalaId);
            Assert.NotEqual(Guid.Empty, reporte.UsuarioId);
            Assert.Equal("Pantalla rota", reporte.Descripcion);
            Assert.NotEqual(default(DateTime), reporte.Fecha);
            Assert.Equal("Pendiente", reporte.Estado);
        }

        [Fact]
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
            Assert.Equal("Sala", reporte.Tipo);
            Assert.NotNull(reporte.SalaId);
            Assert.Null(reporte.EquipoId);
        }

        [Fact]
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
            Assert.Equal("Pendiente", reporte.Estado);
        }
    }
}
