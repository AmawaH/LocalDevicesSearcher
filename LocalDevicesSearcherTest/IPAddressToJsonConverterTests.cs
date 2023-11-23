using System;
using System.Net;
using LocalDevicesSearcher.Infrastructure;
using Moq;
using Newtonsoft.Json;

namespace LocalDevicesSearcherTest
{
    public class IPAddressToJsonConverterTests
    {
        [Fact]
        public void CanConvert_IPAddressType_ReturnsTrue()
        {
            // Arrange
            IPAddressToJsonConverter converter = new();

            // Act
            bool canConvert = converter.CanConvert(typeof(IPAddress));

            // Assert
            Assert.True(canConvert);
        }

        [Fact]
        public void WriteJson_WritesIPAddressAsString()
        {
            // Arrange
            IPAddressToJsonConverter converter = new();
            IPAddress ipAddress = IPAddress.Parse("192.168.1.1");
            var writer = new Mock<JsonWriter>();

            // Act
            converter.WriteJson(writer.Object, ipAddress, It.IsAny<JsonSerializer>());

            // Assert
            writer.Verify(w => w.WriteValue("192.168.1.1"), Times.Once());
        }

        [Fact]
        public void ReadJson_ParsesIPAddressFromString()
        {
            // Arrange
            IPAddressToJsonConverter converter = new();
            string ipAddressString = "192.168.1.2";
            var reader = new Mock<JsonReader>();
            reader.Setup(r => r.Value).Returns(ipAddressString);

            // Act
            var result = converter.ReadJson(reader.Object, typeof(IPAddress), null, It.IsAny<JsonSerializer>());

            // Assert
            Assert.IsType<IPAddress>(result);
            Assert.Equal(IPAddress.Parse(ipAddressString), result);
        }
    }
}
