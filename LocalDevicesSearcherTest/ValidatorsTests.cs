using LocalDevicesSearcher.Validations;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var validator = new Validators();

            // Act
            bool result = validator.IsConnectedValidation(ip);

            // Assert
            Assert.Equal(expectedResult, result);
        }
        [Fact]
        public void TryCreateFile_ValidFileName_CreatesFile()
        {
            // Arrange
            var validator = new Validators();
            var fileName = "test.txt";

            // Act
            bool result = validator.TryCreateFile(fileName);

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
            var validator = new Validators();
            var fileName = @"C:\Windows\System32\invalid.txt"; // An invalid path

            // Act
            bool result = validator.TryCreateFile(fileName);

            // Assert
            Assert.False(result);
        }

    }
}


