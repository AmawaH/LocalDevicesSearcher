using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LocalDevicesSearcher.Infrastructure.Logger;
using LocalDevicesSearcher.Models;

namespace LocalDevicesSearcher.Processing
{
    public interface IDeviceSearcher
    {
        List<Device> DevicesSearch(int minSubnetRange, int maxSubnetRange, string subnet);
    }
    public class DeviceSearcher : IDeviceSearcher
    { 
        private ILogger logger;
        private IPingingService pingingService;
        private IPortDetectService portsDetectService;
        private IDeviceRepository deviceRepository;
        public DeviceSearcher(ILogger _logger)
        {
            logger = _logger;
            pingingService = new PingingService(logger);
            portsDetectService = new PortsDetectService(logger);
            deviceRepository = new DeviceRepository();
        }
        public DeviceSearcher(ILogger _logger, IPingingService _pingingService)
        {
            logger = _logger;
            pingingService = _pingingService;
            portsDetectService = new PortsDetectService(logger);
            deviceRepository = new DeviceRepository();
        }
        public DeviceSearcher(ILogger _logger, IPingingService _pingingService, IPortDetectService _portsDetectService, IDeviceRepository _deviceRepository)
        {
            logger = _logger;
            pingingService = _pingingService;
            portsDetectService = _portsDetectService;
            deviceRepository = _deviceRepository;
        }
        public List<Device> DevicesSearch(int minSubnetRange, int maxSubnetRange, string subnet)
        {
            Parallel.ForEach(
                    Enumerable.Range(minSubnetRange, maxSubnetRange - minSubnetRange),
                        new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, // Кількість паралельних потоків
                        i =>
                        {
                            string addressInProcess = subnet + i;
                            IPAddress detectedIp = pingingService.Pinging(addressInProcess);
                            if (detectedIp is not null)
                            {
                                List<int> openedPorts = portsDetectService.PortsDetect(detectedIp);
                                deviceRepository.AddDevice(detectedIp, openedPorts);
                            }
                        });
            var devices = deviceRepository.GetDevices();
            return devices;
        }
    }
}