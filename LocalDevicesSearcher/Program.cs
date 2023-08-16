using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace LocalDevicesSearcher
{
    class Program
    {
        public static void Main()
        {
            List<Device> foundDeviceCollection = new();
            string selfLocalIpAddress = GetSelfLocalIpAddress();
            if (Validator(selfLocalIpAddress))
            {
                string subnet = selfLocalIpAddress.Substring(0, selfLocalIpAddress.LastIndexOf('.') + 1);
                byte[] buffer = Encoding.ASCII.GetBytes("PingingMessage");
                int timeout = 100;
                string pingedAddress;
                Ping pinger = new Ping();
                Logger($"Pinging subnet {subnet}0 - {subnet}255 :");
                for (int i = 90; i < 105; i++) //temporal range. The original is from 0 to 255
                {
                    pingedAddress = subnet + i;
                    var reply = pinger.Send(pingedAddress, timeout, buffer);
                    if (reply.Status == IPStatus.Success)
                    {
                        Logger($"Address: {reply.Address} Found something!");
                        Device foundDevice = new Device(pingedAddress);
                        foundDeviceCollection.Add(foundDevice);
                    }
                    else
                    {
                        Logger($"Address: {reply.Address} Nothing here...");
                    }
                }
                DisplayFoundDevices(foundDeviceCollection);

            }
            else
            {
                Console.WriteLine("Your device is not connected to any network");
            }
        }

        public static void Logger(string msg)
        {
            Console.WriteLine(msg);
            //logging into the file will be here
        }

        public static bool Validator(string address)
        {
            return (address != "127.0.0.1");
        }
    
        public static void DisplayFoundDevices(List<Device> foundDevices)
        {
            // writing the results to a JSON file will be here
            Console.WriteLine("\n\nData of connected devices:");
            foreach (var dev in foundDevices)
            {
                Console.WriteLine($"\nIp4: {dev.GetIp4()}");
                Console.WriteLine($"Ip6: {dev.GetIp6()}");
                Console.WriteLine($"HostName: {dev.GetHostName()}");
                Console.WriteLine($"MAC-Address: {dev.GetMacAddress()}");
            }
        }

        private static string GetSelfLocalIpAddress()
        {
            var hostName = Dns.GetHostName();
            var ipHostEntry = Dns.GetHostEntry(hostName);

            var ipAddress = ipHostEntry.AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            Console.WriteLine($"Local IP Address: {ipAddress}");
            return ipAddress.ToString();
        }
    }

    public class Device 
    {
        // more data such as mac-address, ip6, and hostname will be added later
        private string Ip4 { get; set; }
        private string Ip6 { get; set; } = "Empty yet. Will be later...";
        private string HostName { get; set; } = "Empty yet. Will be later...";
        private string MacAddress { get; set; } = "Empty yet. Will be later...";
        public Device(string ip4)
        {
            Ip4 = ip4;
        }
        public string GetIp4()
        {
            return Ip4;
        }
        public string GetIp6()
        {
            return Ip6;
        }
        public string GetHostName()
        {
            return HostName;
        }
        public string GetMacAddress()
        {
            return MacAddress;
        }
    }
}

