using LocalDevicesSearcher.Infrastructure;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace LocalDevicesSearcher.Models
{
    public class Device
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(15)")]
        [JsonConverter(typeof(IPAddressToJsonConverter))]
        public IPAddress Ip4 { get; set; }
        [Column(TypeName = "nvarchar(45)")]
        [JsonConverter(typeof(IPAddressToJsonConverter))]
        public IPAddress Ip6 { get; set; }
        public string HostName { get; set; }
        public string MacAddress { get; set; }
        public List<OpenedPort> OpenedPorts { get; set; }
    }
    public class OpenedPort
    {
        public int Id { get; set; }
        public int PortNumber { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }
    }
}