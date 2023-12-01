using System;
using System.Collections.Generic;
using System.Net;
using LocalDevicesSearcher.Infrastructure;
using LocalDevicesSearcher.Models;
using LocalDevicesSearcher.Processing;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LocalDevicesSearcherTest
{
    public class DeviceSearcherTests
    {

        [Fact]
        public void DevicesSearch_ReturnsDeviceList()
        {
            // Arrange
            var minSubnetRange = 1;
            var maxSubnetRange = 3;
            var subnet = "192.168.0.";
            var loggerMock = new Mock<ILogger>();
            var pingingServiceMock = new Mock<IPingingService>();
            ISelfIpAddressGetter selfIpAddressGetter = new SelfIpAddressGetter();
            IPAddress testIp = selfIpAddressGetter.GetSelfIp4Address();
            pingingServiceMock.Setup(ps => ps.Pinging(It.IsAny<string>())).Returns(testIp);
            var portsDetectServiceMock = new Mock<IPortDetectService>();
            List<int> testPortList = new List<int> { 1, 2, 3, 4 };
            portsDetectServiceMock.Setup(pds => pds.PortsDetect(It.IsAny<IPAddress>())).Returns(testPortList);
            IDeviceRepository deviceRepository = new DeviceRepository();
            var deviceSearcher = new DeviceSearcher(loggerMock.Object, pingingServiceMock.Object, portsDetectServiceMock.Object, deviceRepository);

            // Act
            var devices = deviceSearcher.DevicesSearch(minSubnetRange, maxSubnetRange, subnet);

            // Assert
            Assert.NotNull(devices);
            Assert.True(devices.Count > 0);
        }

        [Fact]
        public void DevicesSearch_ReturnsEmptyListForNoDevices()
        {
            // Arrange
            var minSubnetRange = 1;
            var maxSubnetRange = 3;
            var subnet = "192.168.0.";
            var loggerMock = new Mock<ILogger>();
            var pingingServiceMock = new Mock<IPingingService>();
            IPAddress testIp = null;
            pingingServiceMock.Setup(ps => ps.Pinging(It.IsAny<string>())).Returns(testIp);
            var deviceSearcher = new DeviceSearcher(loggerMock.Object, pingingServiceMock.Object);

            // Act
            var devices = deviceSearcher.DevicesSearch(minSubnetRange, maxSubnetRange, subnet);

            // Assert
            Assert.Empty(devices);
        }
    }
}