using LocalDevicesSearcher.infrastructure;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace LocalDevicesSearcher
{
    public class Device
    {
        [JsonConverter(typeof(IPAddressToJsonConverter))]
        public IPAddress Ip4 { get; init; }
        [JsonConverter(typeof(IPAddressToJsonConverter))]
        public IPAddress Ip6 { get; init; } 
        public string HostName { get; init; } 
        public string MacAddress { get; init; }
        public List<int> OpenedPorts { get; init; }
        public Device(IPAddress ip4,IPAddress ip6, string hostName, string macAddress, List<int> detectedPorts)
        {
            Ip4 = ip4;
            Ip6 = ip6;
            HostName = hostName;
            MacAddress = macAddress;
            OpenedPorts = detectedPorts;
        }
    }
}
