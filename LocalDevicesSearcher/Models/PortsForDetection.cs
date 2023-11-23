using System;
using System.Collections.Generic;

namespace LocalDevicesSearcher.Models
{
    public interface IPortsForDetection
    {
        List<int> GetPorts();
    }
    public class PortsForDetection : IPortsForDetection
    {
        private List<int> Ports { get; init; }
        public PortsForDetection()
        {
            Ports = new List<int>();
            for (int i = 1; i < 1024; i++)
            {
                Ports.Add(i);
            }
        }
        public PortsForDetection(List<int> ports)
        {
            Ports = ports;
        }
        public List<int> GetPorts()
        {
            return Ports;
        }
    }
}
