using Services;

namespace ServicesTest
{
    [TestFixture]
    public class DependencyInjectionTests
    {
        [Test]
        public void DependencyInjection_ClassExists_ShouldCompile()
        {
            // Arrange & Act
            // Verificar que la clase existe y compila
            var type = typeof(Dependencyinjection);

            // Assert
            Assert.That(type, Is.Not.Null);
            Assert.That(type.Name, Is.EqualTo("Dependencyinjection"));
        }
    }
}

