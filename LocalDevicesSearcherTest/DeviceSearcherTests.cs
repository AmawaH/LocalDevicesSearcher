using Moq;
using LocalDevicesSearcher.Infrastructure.ResultWriter;
using LocalDevicesSearcher.Models;
using LocalDevicesSearcher.Processing;
using System.Net;
using Microsoft.Extensions.Logging;
using LocalDevicesSearcher.Infrastructure;

namespace LocalDevicesSearcherTest
{

    public class DeviceSearcherTests
    {
        [Fact]
        public void DevicesSearch_PingAndPortDetection_Success()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var resultWriterMock = new Mock<IResultWriter>();
            var pingingServiceMock = new Mock<IPingingService>();
            var portsDetectServiceMock = new Mock<IPortDetectService>();
            var configurationMock = new Mock<IConfig>();

            var deviceSearcher = new DeviceSearcher(loggerMock.Object, resultWriterMock.Object, pingingServiceMock.Object, configurationMock.Object, portsDetectServiceMock.Object);

            string subnet = "192.168.0.";

            // Set up mocks for successful ping and port detection
            pingingServiceMock.Setup(p => p.Pinging(It.IsAny<string>())).Returns(IPAddress.Parse("192.168.0.1"));
            portsDetectServiceMock.Setup(p => p.PortsDetect(It.IsAny<IPAddress>())).Returns(new List<int> { 80, 443 });
            configurationMock.Setup(c => c.Range).Returns(false);
            configurationMock.Setup(c => c.SubnetCollection).Returns(new int[] { 1 });

            // Act
            deviceSearcher.DevicesSearch(subnet);

            // Assert
            resultWriterMock.Verify(r => r.WriteResult(It.IsAny<Device>()), Times.Once);
        }
        [Fact]
        public void DevicesSearch_SubnetCollectionProcessing_Success()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var resultWriterMock = new Mock<IResultWriter>();
            var pingingServiceMock = new Mock<IPingingService>();
            var portsDetectServiceMock = new Mock<IPortDetectService>();
            var configurationMock = new Mock<IConfig>();
            configurationMock.Setup(c => c.Range).Returns(false);
            configurationMock.Setup(c => c.SubnetCollection).Returns(new int[] { 1, 102, 103 });
            pingingServiceMock.Setup(p => p.Pinging(It.IsAny<string>())).Returns(IPAddress.Parse("192.168.0.1"));
            portsDetectServiceMock.Setup(p => p.PortsDetect(It.IsAny<IPAddress>())).Returns(new List<int> { 80, 443 });
            var deviceSearcher = new DeviceSearcher(loggerMock.Object, resultWriterMock.Object, pingingServiceMock.Object, configurationMock.Object, portsDetectServiceMock.Object);

            string subnet = "192.168.0.";

            // Act
            deviceSearcher.DevicesSearch(subnet);

            // Assert
            resultWriterMock.Verify(r => r.WriteResult(It.IsAny<Device>()), Times.Exactly(3));
        }

        [Fact]
        public void DevicesSearch_RangeProcessing_Success()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var resultWriterMock = new Mock<IResultWriter>();
            var pingingServiceMock = new Mock<IPingingService>();
            var portsDetectServiceMock = new Mock<IPortDetectService>();
            var configurationMock = new Mock<IConfig>();
            configurationMock.Setup(c => c.Range).Returns(true);
            configurationMock.Setup(c => c.MinSubnetRange).Returns(5);
            configurationMock.Setup(c => c.MaxSubnetRange).Returns(7);
            pingingServiceMock.Setup(p => p.Pinging(It.IsAny<string>())).Returns(IPAddress.Parse("192.168.0.1"));
            portsDetectServiceMock.Setup(p => p.PortsDetect(It.IsAny<IPAddress>())).Returns(new List<int> { 80, 443 });
            var deviceSearcher = new DeviceSearcher(loggerMock.Object, resultWriterMock.Object, pingingServiceMock.Object, configurationMock.Object, portsDetectServiceMock.Object);
            string subnet = "192.168.0.";

            // Act
            deviceSearcher.DevicesSearch(subnet);

            // Assert
            resultWriterMock.Verify(r => r.WriteResult(It.IsAny<Device>()), Times.Exactly(3));
        }
        [Fact]
        public void DevicesSearch_PingFailure_NoResultWritten()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var resultWriterMock = new Mock<IResultWriter>();
            var pingingServiceMock = new Mock<IPingingService>();
            var portsDetectServiceMock = new Mock<IPortDetectService>();
            var configurationMock = new Mock<IConfig>();
            configurationMock.Setup(c => c.Range).Returns(true);
            configurationMock.Setup(c => c.MinSubnetRange).Returns(1);
            configurationMock.Setup(c => c.MaxSubnetRange).Returns(3);
            var deviceSearcher = new DeviceSearcher(loggerMock.Object, resultWriterMock.Object, pingingServiceMock.Object, configurationMock.Object, portsDetectServiceMock.Object);
            string subnet = "192.168.0.";

            // Set up mock for failed ping
            pingingServiceMock.Setup(p => p.Pinging(It.IsAny<string>())).Returns((IPAddress)null);
            portsDetectServiceMock.Setup(p => p.PortsDetect(It.IsAny<IPAddress>())).Returns((List<int>)null);

            // Act
            deviceSearcher.DevicesSearch(subnet);

            // Assert
            resultWriterMock.Verify(r => r.WriteResult(It.IsAny<Device>()), Times.Never);
        }
    }
}
