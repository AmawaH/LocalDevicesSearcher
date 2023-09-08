﻿using System;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;

namespace LocalDevicesSearcher.Models
{
    public class DeviceDataCalculator 
    {
        private readonly Device device;
        public DeviceDataCalculator(IPAddress address)
        {
            IPAddress ip4 = address;
            IPAddress ip6 = GetIp6(address);
            string hostName = GetHostName(address);
            string macAddress = GetMacAddress(address);
            device = new Device(ip4, ip6, hostName, macAddress);
        }
        public Device GetDevice()
        {
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
            catch (Exception ex)
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