using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDevicesSearcher.Models
{
    public class PortsForDetection
    {
        private List<int> Ports { get; init; }
        public PortsForDetection()
        {
            Ports = new List<int>();
            for (int i = 0; i < 1024; i++)
            {
                Ports.Add(i);
            }
        }
        public List<int> GetPorts()
        {
            return Ports;
        }
    }
}
