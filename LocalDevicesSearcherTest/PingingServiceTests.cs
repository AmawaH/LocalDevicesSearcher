using LocalDevicesSearcher.Infrastructure.Logger;
using LocalDevicesSearcher.Processing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
            loggerMock.Setup(x => x.Log(It.IsAny<string>()));
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
            loggerMock.Setup(x => x.Log(It.IsAny<string>()));
            var pingingService = new PingingService(loggerMock.Object);
            // Act
            var result = pingingService.Pinging(pingedAddress);
            // Assert
            Assert.Null(result);
        }


    }
}
