using LocalDevicesSearcher.infrastructure;
using LocalDevicesSearcher.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

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
        public Device(IPAddress ip4,IPAddress ip6, string hostName, string macAddress)
        {
            Ip4 = ip4;
            Ip6 = ip6;
            HostName = hostName;
            MacAddress = macAddress;
        }
    }
}
