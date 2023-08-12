using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

class Program
{
    public static void Main()
    {
        List<string> foundAddressesList = new List<string>();
        string selfLocalIpAddress = GetSelfLocalIpAddress();
        Console.WriteLine($"Local IP Address: {selfLocalIpAddress}");
        if (selfLocalIpAddress != "127.0.0.1")
        {
            string subnet = selfLocalIpAddress.Substring(0, selfLocalIpAddress.LastIndexOf('.') + 1);
            byte[] buffer = Encoding.ASCII.GetBytes("PingingMessage");
            int timeout = 100;
            string pingedAddress;
            Ping pinger = new Ping();
            Console.WriteLine($"Pinging subnet {subnet}0 - {subnet}255 :");
            for (int i = 0; i < 255; i++)
            {
                pingedAddress = subnet + i;
                var reply = pinger.Send(pingedAddress, timeout, buffer);
                string msg;
                if (reply.Status == IPStatus.Success)
                {
                    Console.WriteLine($"Address: {reply.Address} Found something!");
                    foundAddressesList.Add(pingedAddress);
                }
                else
                {
                    Console.WriteLine($"Address: {reply.Address} Nothing here...");
                }
            }
            Console.WriteLine("\n\n IP-Addresses with connected devices:");
            foreach (var addr in foundAddressesList)
            {
                Console.WriteLine(addr);
            }
        }
        else
        {
            Console.WriteLine("Your device is not connected to any network");
        }
    }

    private static string GetSelfLocalIpAddress()
    {
        var hostName = Dns.GetHostName();
        var ipHostEntry = Dns.GetHostEntry(hostName);

        var ipAddress = ipHostEntry.AddressList
            .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

        return ipAddress.ToString();
    }
}
