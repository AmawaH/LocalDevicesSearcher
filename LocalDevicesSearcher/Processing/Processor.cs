using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using LocalDevicesSearcher.infrastructure.Logger;
using LocalDevicesSearcher.infrastructure.ResultWriter;
using LocalDevicesSearcher.Models;

namespace LocalDevicesSearcher.Processing
{
    public class Processor
    {
        private Logger logger;
        private ResultWriter resultWriter;
        private const int pingTimeout = 100;
        public Processor(Logger _logger, ResultWriter _resultWriter)
        {
            logger = _logger;
            resultWriter = _resultWriter;
        }
        public IPAddress Pinging(string pingedAddress)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("PingingMessage");
            Ping pinger = new Ping();
            PingReply reply = pinger.Send(pingedAddress, pingTimeout, buffer);
            IPAddress replyAddress = reply.Address;
            if (reply.Status == IPStatus.Success)
            {
                string msg = $"Pinged address: {replyAddress} Found something!";
                logger.Log(msg);

                return replyAddress;
            }
            else
            {
                string msg = $"Pinged address: {replyAddress} Nothing here...";
                logger.Log(msg);
                return null;
            }
        }

        public List<int> PortsDetect(IPAddress ipAddress)
        {
            PortsForDetection portsForDetection = new PortsForDetection();
            List<int> portsForDetectionList = portsForDetection.GetPorts();
            List<int> openedPorts = new List<int> { };
            int maxPortIndex = portsForDetectionList.Count;
            int numThreads = 10;
            int portsPerThread = maxPortIndex / numThreads;
            List<Thread> threads = new List<Thread>();

            string msg = $"Detecting opened ports for {ipAddress}";
            logger.Log(msg);

            for (int i = 0; i < numThreads; i++)
            {
                int startPortIndex = i * portsPerThread + 1;
                int endPortIndex = (i == numThreads - 1) ? maxPortIndex : (i + 1) * portsPerThread;

                Thread thread = new Thread(
                    () => {
                    int port = CheckPorts(startPortIndex, endPortIndex, ipAddress, portsForDetectionList);
                    if (port != 0)
                        {
                            openedPorts.Add(port);
                        }
                    });
                threads.Add(thread);
                thread.Start();
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            List<string> portsString = new List<string>();
            foreach (int port in openedPorts)
            {
                portsString.Add(port.ToString());
            }
            msg = $"Opened ports for {ipAddress}: ";
            msg += String.Join(", ", portsString.ToArray());   
            logger.Log(msg); 
            return openedPorts;
        }
        public int CheckPorts(int startPortindex, int endPortindex, IPAddress ipAddress, List<int> portsForDetectionList)
        {
            for (int i = startPortindex; i <= endPortindex; i++)
            {
                int port = portsForDetectionList[i-1];
                Console.WriteLine($"Trying port {port}");
                bool isOpen = IsPortOpen(ipAddress, port);
                if (isOpen)
                {
                        return port;
                }
            }
            return 0;
        }
        private static bool IsPortOpen(IPAddress ipAddress, int port)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(ipAddress, port);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}