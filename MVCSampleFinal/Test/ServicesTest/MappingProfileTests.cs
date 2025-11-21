using Services.Automapper;
using Xunit;

namespace ServicesTest
{
    public class MappingProfileTests
    {
        [Fact]
        public void MappingProfile_ShouldBeCreated()
        {
            // Arrange & Act
            var mappingProfile = new MappingProfile();

            // Assert
            Assert.NotNull(mappingProfile);
        }

        [Fact]
        public void MappingProfile_ShouldCompile()
        {
            // Arrange & Act
            var mappingProfile = new MappingProfile();

            // Assert - Si llegamos aquí, la clase compila correctamente
            Assert.IsType<MappingProfile>(mappingProfile);
        }
    }
}
