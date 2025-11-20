using Services.Automapper;

namespace ServicesTest
{
    [TestFixture]
    public class MappingProfileTests
    {
        [Test]
        public void MappingProfile_ShouldBeCreated()
        {
            // Arrange & Act
            var mappingProfile = new MappingProfile();

            // Assert
            Assert.That(mappingProfile, Is.Not.Null);
        }

        [Test]
        public void MappingProfile_ShouldCompile()
        {
            // Arrange & Act
            var mappingProfile = new MappingProfile();

            // Assert - Si llegamos aquí, la clase compila correctamente
            Assert.That(mappingProfile, Is.InstanceOf<MappingProfile>());
        }
    }
}

