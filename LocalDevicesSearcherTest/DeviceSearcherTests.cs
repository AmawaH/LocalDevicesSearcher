//using System;
//using System.Collections.Generic;
//using System.Net;
//using LocalDevicesSearcher.Infrastructure;
//using LocalDevicesSearcher.Infrastructure.ResultWriter;
//using LocalDevicesSearcher.Models;
//using LocalDevicesSearcher.Processing;
//using Microsoft.Extensions.Logging;
//using Moq;
//using Xunit;

//namespace LocalDevicesSearcherTest
//{
//    public class DeviceSearcherTests
//    {

//        [Fact]
//        public void DevicesSearch_ReturnsDeviceList()
//        {
//            // Arrange
//            var minSubnetRange = 1;
//            var maxSubnetRange = 3;
//            var subnet = "192.168.0.";
//            var loggerMock = new Mock<ILogger>();
//            var resultWriterMock = new Mock<IResultWriter>();
//            var pingingServiceMock = new Mock<IPingingService>();
//            ISelfIpAddressGetter selfIpAddressGetter = new SelfIpAddressGetter();
//            IPAddress testIp = selfIpAddressGetter.GetSelfIp4Address();
//            pingingServiceMock.Setup(ps => ps.Pinging(It.IsAny<string>())).Returns(testIp);
//            var portsDetectServiceMock = new Mock<IPortDetectService>();
//            List<int> testPortList = new List<int> { 1, 2, 3, 4 };
//            portsDetectServiceMock.Setup(pds => pds.PortsDetect(It.IsAny<IPAddress>())).Returns(testPortList);
//            var deviceSearcher = new DeviceSearcher(loggerMock.Object, resultWriterMock.Object ,pingingServiceMock.Object, portsDetectServiceMock.Object, deviceRepository);
//            IDeviceRepository deviceRepository = de
//            // Act
//            deviceSearcher.DevicesSearch(subnet, minSubnetRange, maxSubnetRange);
//            Device device =

//            // Assert
//            Assert.NotNull(devices);
//            Assert.True(devices.Count > 0);
//        }

//        [Fact]
//        public void DevicesSearch_ReturnsEmptyListForNoDevices()
//        {
//            // Arrange
//            var minSubnetRange = 1;
//            var maxSubnetRange = 3;
//            var subnet = "192.168.0.";
//            var loggerMock = new Mock<ILogger>();
//            var pingingServiceMock = new Mock<IPingingService>();
//            IPAddress testIp = null;
//            pingingServiceMock.Setup(ps => ps.Pinging(It.IsAny<string>())).Returns(testIp);
//            IDeviceRepository deviceRepository = new DeviceCalculator();
//            var deviceSearcher = new DeviceSearcher(loggerMock.Object, null ,pingingServiceMock.Object, null, deviceRepository);

//            // Act
//            deviceSearcher.DevicesSearch(minSubnetRange, maxSubnetRange, subnet);
//            var devices = deviceRepository.GetDevices();
            
//            // Assert
//            Assert.Empty(devices);
//        }
//    }
//}