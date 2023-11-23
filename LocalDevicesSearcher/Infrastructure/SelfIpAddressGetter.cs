using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace LocalDevicesSearcher.Infrastructure
{
    public interface ISelfIpAddressGetter
    {
        IPAddress GetSelfIp4Address();
    }
    public class SelfIpAddressGetter : ISelfIpAddressGetter
    {
        public IPAddress GetSelfIp4Address()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry ipHostEntry = Dns.GetHostEntry(hostName);
            IPAddress ip4 = ipHostEntry.AddressList
                .FirstOrDefault(ip => ip.AddressFamily.ToString() == ProtocolFamily.InterNetwork.ToString());
            return ip4;
        }
    }
}