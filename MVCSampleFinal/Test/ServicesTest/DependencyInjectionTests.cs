using Services;
using Xunit;

namespace ServicesTest
{
    public class DependencyInjectionTests
    {
        [Fact]
        public void DependencyInjection_ClassExists_ShouldCompile()
        {
            // Arrange & Act
            // Verificar que la clase existe y compila
            var type = typeof(Dependencyinjection);

            // Assert
            Assert.NotNull(type);
            Assert.Equal("Dependencyinjection", type.Name);
        }
    }
}
