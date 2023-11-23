using LocalDevicesSearcher.Infrastructure.Logger;
using LocalDevicesSearcher.Models;
using LocalDevicesSearcher.Processing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LocalDevicesSearcherTest
{
    public class PortsDetectServiceTests
    {
        [Fact]
        public void PortsDetect_ReturnsOpenedPorts()
        {
            // Arrange
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(x => x.Log(It.IsAny<string>()));
            var portsForDetectionMock = new Mock<IPortsForDetection>();
            List<int> testPortsList = new() { 20000, 20001, 20002, 20004 };
            portsForDetectionMock.Setup(pfdm => pfdm.GetPorts()).Returns(testPortsList);
            foreach (var port in testPortsList)
            {
                TcpListener tcpListener = new TcpListener(IPAddress.Any, port);
                tcpListener.Start();
            }
            var portsDetectService = new PortsDetectService(loggerMock.Object, portsForDetectionMock.Object);
            // Act
            var openedPorts = portsDetectService.PortsDetect(ipAddress);
            // Assert
            Assert.NotNull(openedPorts);
            Assert.True(openedPorts.Count > 0);
        }

    }
}
