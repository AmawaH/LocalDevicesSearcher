using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LocalDevicesSearcher
{
    public class Device
    {
        public string Ip4 { get; set; }
        public string Ip6 { get; set; }
        public string HostName { get; set; }
        public string MacAddress { get; set; }
        public Device(string ip4)
        {
            Ip4 = ip4;
            Ip6 = SetIp6();
            HostName = SetHostName();
            MacAddress = SetMacAddress();

        }
        private string SetIp6()
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(Ip4);
                IPAddress ip6 = hostEntry.AddressList
                    .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetworkV6);
                string result = ip6.ToString();
                return result;
            }
            catch (Exception ex) 
            {
                if (ex is SocketException)
                {
                   string result = "Cannot detect IPv6 for this host";
                   return result;
                }
                else
                {
                   string result = "Unhandled error";
                   return result;
                }
            }
        }
        private string SetHostName()
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(Ip4);
                string result = hostEntry.HostName;
                return result;
            }
            catch (Exception ex)
            {
                if (ex is SocketException)
                {
                    string result = "Cannot detect name for this host";
                    return result;
                }
                else
                {
                    string result = "Unhandled error";
                    return result;
                }
            }
        }
        private string SetMacAddress()
        {
            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;
            IPAddress.TryParse(Ip4,out IPAddress ip4);
            if (SendARP(BitConverter.ToInt32(ip4.GetAddressBytes(), 0), 0, macAddr, ref macAddrLen) != 0)
                return "SendARP failed";

            string[] str = new string[(int)macAddrLen];
            for (int i = 0; i < macAddrLen; i++)
                str[i] = macAddr[i].ToString("x2");

            string result = string.Join(":", str);
            return result;
        }
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(int DestinationIP, int SourceIP, [Out] byte[] pMacAddr, ref uint PhyAddrLen);

    }
}
