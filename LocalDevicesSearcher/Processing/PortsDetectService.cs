using LocalDevicesSearcher.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LocalDevicesSearcher.Processing
{
    public interface IPortDetectService
    {
        List<int> PortsDetect(IPAddress ipAddress);
    }
    public class PortsDetectService : IPortDetectService
    {
        private ILogger _logger;
        private IPortsForDetection _portsForDetection;
        public PortsDetectService(ILogger logger)
        {
            _logger = logger;
            _portsForDetection = new PortsForDetection();
        }
        public PortsDetectService(ILogger logger, IPortsForDetection portsForDetection) : this(logger)
        {
            _portsForDetection = portsForDetection;
        }
        public List<int> PortsDetect(IPAddress ipAddress)
        {
            List<int> portsForDetectionList = _portsForDetection.GetPorts();
            List<int> openedPorts = new();
            int maxPortIndex = portsForDetectionList.Count;
            int numThreads = 10;
            int portsPerThread = maxPortIndex / numThreads;
            List<Thread> threads = new();

            string msg = $"Detecting opened ports for {ipAddress}";
            _logger.LogInformation(msg);

            for (int i = 0; i < numThreads; i++)
            {
                int startPortIndex = i * portsPerThread + 1;
                int endPortIndex = (i == numThreads - 1) ? maxPortIndex : (i + 1) * portsPerThread;
                Thread thread = new Thread(
                    () =>
                    {
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
            _logger.LogInformation(msg);

            return openedPorts;
        }
        private int CheckPorts(int startPortindex, int endPortindex, IPAddress ipAddress, List<int> portsForDetectionList)
        {
            for (int i = startPortindex; i <= endPortindex; i++)
            {
                int port = portsForDetectionList[i - 1];

                string msg = $"Trying port {ipAddress}:{port}";
                _logger.LogTrace(msg);

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
            TcpClient client = new TcpClient();
            var result = client.BeginConnect(ipAddress, port, null, null);
            var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
            if (!success)
            {
                return false;
            }
            client.EndConnect(result);
            return true;
        }
    }
}