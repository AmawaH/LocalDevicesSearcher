using LocalDevicesSearcher.Processing;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace LocalDevicesSearcherTest
{
    public class PingingServiceTests
    {
        [Fact]
        public void Pinging_ReturnsIpAddress_WhenPingIsSuccessful()
        {
            // Arrange
            var pingedAddress = "127.0.0.1";
            var expectedIpAddress = IPAddress.Parse(pingedAddress);
            var loggerMock = new Mock<ILogger>();
            var pingingService = new PingingService(loggerMock.Object);
            // Act
            var result = pingingService.Pinging(pingedAddress);
            // Assert
            Assert.Equal(expectedIpAddress, result);
        }

        [Fact]
        public void Pinging_ReturnsNull_WhenPingFails()
        {
            // Arrange
            var pingedAddress = "invalidaddress";
            var loggerMock = new Mock<ILogger>();
            var pingingService = new PingingService(loggerMock.Object);
            // Act
            var result = pingingService.Pinging(pingedAddress);
            // Assert
            Assert.Null(result);
        }


    }
}
