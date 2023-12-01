using LocalDevicesSearcher.Models;
using LocalDevicesSearcher.Processing;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Net.Sockets;

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
