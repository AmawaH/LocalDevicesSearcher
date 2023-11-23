using LocalDevicesSearcher.Infrastructure.Logger;
using LocalDevicesSearcher.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LocalDevicesSearcher.Processing
{
    public interface IPortDetectService
    {
        List<int> PortsDetect(IPAddress ipAddress);
    }
    public class PortsDetectService : IPortDetectService
    {
        private ILogger logger;
        private IPortsForDetection portsForDetection;

        public PortsDetectService(ILogger _logger)
        {
            logger = _logger;
            portsForDetection = new PortsForDetection();
        }
        public PortsDetectService(ILogger logger, IPortsForDetection portsForDetection) : this(logger)
        {
            this.portsForDetection = portsForDetection;
        }
        public List<int> PortsDetect(IPAddress ipAddress)
        {
            List<int> portsForDetectionList = portsForDetection.GetPorts();
            List<int> openedPorts = new();
            int maxPortIndex = portsForDetectionList.Count;
            int numThreads = 10;
            int portsPerThread = maxPortIndex / numThreads;
            List<Thread> threads = new();

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
        private int CheckPorts(int startPortindex, int endPortindex, IPAddress ipAddress, List<int> portsForDetectionList)
        {
            for (int i = startPortindex; i <= endPortindex; i++)
            {
                int port = portsForDetectionList[i - 1];

                string msg = $"Trying port {ipAddress}:{port}";
                logger.Log(msg);

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
