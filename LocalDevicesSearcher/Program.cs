using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace LocalDevicesSearcher
{
    public class Program
    {
        const int timeout = 100;
        static string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        public static void Main()
        {
            var logger = new Logger(new SimpleLogService());
            var resultWriter = new ResultWriter(new ResultWriterService());
            logger.CreateLogFile(fileName);
            resultWriter.CreateResultFile(fileName);


            string selfLocalIpAddress = GetSelfLocalIpAddress();

            if (Validator(selfLocalIpAddress))
            {
                string subnet = selfLocalIpAddress.Substring(0, selfLocalIpAddress.LastIndexOf('.') + 1);
                byte[] buffer = Encoding.ASCII.GetBytes("PingingMessage");
                var pinger = new Ping();

                string msg = $"Pinging subnet {subnet}0 - {subnet}255 :\n";
                logger.Log(msg);

                for (int i = 0; i < 255; i++) 
                {
                    IPAddress.TryParse(subnet + i, out IPAddress pingedAddress);
                    PingReply reply = pinger.Send(pingedAddress, timeout, buffer);
                    if (reply.Status == IPStatus.Success)
                    {
                        msg = $"Address: {reply.Address} Found something!";
                        logger.Log(msg);

                        Device foundDevice = new Device(pingedAddress.ToString());
                        resultWriter.WriteResult(foundDevice);
                    }
                    else
                    {
                        msg = $"Address: {reply.Address} Nothing here...";
                        logger.Log(msg);
                    }
                }

            }
            else
            {
               logger.Log("Your device is not connected to any network");
            }
        }


        public static bool Validator(string address)
        {
            return (address != "127.0.0.1");
        }
    

        private static string GetSelfLocalIpAddress()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry ipHostEntry = Dns.GetHostEntry(hostName);

            IPAddress ipAddress = ipHostEntry.AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            Console.WriteLine($"Local IP Address: {ipAddress}");
            IPAddress ipAddressV6 = ipHostEntry.AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetworkV6);

            Console.WriteLine($"Local IPv6 Address: {ipAddressV6}");

            return ipAddress.ToString();
        }


    }

}

