using System;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace LocalDevicesSearcher.Models
{
    public interface IDeviceCalculator
    {
        Device CalculateDevice(IPAddress address, List<int> openedPorts);
    }
    public class DeviceCalculator : IDeviceCalculator
    {
        public Device CalculateDevice(IPAddress address, List<int> openedPorts)
        {
            Device device = new Device();
            device.Ip4 = address;
            device.Ip6 = GetIp6(address);
            device.HostName = GetHostName(address);
            device.MacAddress = GetMacAddress(address);
            device.OpenedPorts = openedPorts.Select(portNumber => new OpenedPort { PortNumber = portNumber }).ToList();
            return device;
        }
        private static IPAddress GetIp6(IPAddress address)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(address);
                IPAddress ip6 = hostEntry.AddressList
                    .FirstOrDefault(ip => ip.AddressFamily.ToString() == ProtocolFamily.InterNetworkV6.ToString());
                return ip6;
            }
            catch
            {
                return null;
            }
        }
        private static string GetHostName(IPAddress address)
        {
            string result;
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(address);
                result = hostEntry.HostName;
            }
            catch (Exception ex)
            {
                result = (ex is SocketException) ? "Cannot detect name for this host" : "Unhandled error";
            }
            return result;
        }
        private static string GetMacAddress(IPAddress address)
        {
            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;
            if (SendARP(BitConverter.ToInt32(address.GetAddressBytes(), 0), 0, macAddr, ref macAddrLen) != 0)
                return null;    // "SendARP failed";
            string[] str = new string[(int)macAddrLen];
            for (int i = 0; i < macAddrLen; i++)
                str[i] = macAddr[i].ToString("x2");
            string result = string.Join(":", str);
            return result;
        }
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        private static extern int SendARP(int DestinationIP, int SourceIP, [Out] byte[] pMacAddr, ref uint PhyAddrLen);
    }
}