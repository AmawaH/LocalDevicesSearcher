using LocalDevicesSearcher.Validations;

namespace LocalDevicesSearcherTest
{
    public class ValidatorsTests
    {
        [Theory]
        [InlineData("127.0.0.1", false)]
        [InlineData("192.168.0.1", true)]
        public void IsConnectedValidation_ShouldReturnExpectedResult(string ip, bool expectedResult)
        {
            // Arrange
            IIsConnectedValidator isConnectedValidator = new IsConnectedValidator();

            // Act
            bool result = isConnectedValidator.IsConnectedValidation(ip);

            // Assert
            Assert.Equal(expectedResult, result);
        }
        [Fact]
        public void TryCreateFile_ValidFileName_CreatesFile()
        {
            // Arrange
            ICanCreateFileValidator canCreateFileValidator = new CanCreateFileValidator();
            var fileName = "test.txt";

            // Act
            bool result = canCreateFileValidator.TryCreateFile(fileName);

            // Assert
            Assert.True(result);
            Assert.True(File.Exists(fileName));

            // Clean up (remove the created file)
            File.Delete(fileName);
        }

        [Fact]
        public void TryCreateFile_InvalidFileName_ReturnsFalse()
        {
            // Arrange
            ICanCreateFileValidator canCreateFileValidator = new CanCreateFileValidator();
            var fileName = @"C:\Windows\System32\invalid.txt"; // An invalid path

            // Act
            bool result = canCreateFileValidator.TryCreateFile(fileName);

            // Assert
            Assert.False(result);
        }

    }
}


