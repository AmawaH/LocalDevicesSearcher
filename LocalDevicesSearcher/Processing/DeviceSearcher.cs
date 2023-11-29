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
        private ILogger _logger;
        private IPingingService _pingingService;
        private IPortDetectService _portsDetectService;
        private IDeviceRepository _deviceRepository;
        public DeviceSearcher(ILogger logger)
        {
            _logger = logger;
            _pingingService = new PingingService(logger);
            _portsDetectService = new PortsDetectService(logger);
            _deviceRepository = new DeviceRepository();
        }
        public DeviceSearcher(ILogger logger, IPingingService pingingService)
        {
            _logger = logger;
            _pingingService = pingingService;
            _portsDetectService = new PortsDetectService(logger);
            _deviceRepository = new DeviceRepository();
        }
        public DeviceSearcher(ILogger logger, IPingingService pingingService, IPortDetectService portsDetectService, IDeviceRepository deviceRepository)
        {
            _logger = logger;
            _pingingService = pingingService;
            _portsDetectService = portsDetectService;
            _deviceRepository = deviceRepository;
        }
        public List<Device> DevicesSearch(int minSubnetRange, int maxSubnetRange, string subnet)
        {
            Parallel.ForEach(
                    Enumerable.Range(minSubnetRange, maxSubnetRange - minSubnetRange),
                        new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, // Кількість паралельних потоків
                        i =>
                        {
                            string addressInProcess = subnet + i;
                            IPAddress detectedIp = _pingingService.Pinging(addressInProcess);
                            if (detectedIp is not null)
                            {
                                List<int> openedPorts = _portsDetectService.PortsDetect(detectedIp);
                                _deviceRepository.AddDevice(detectedIp, openedPorts);
                            }
                        });
            var devices = _deviceRepository.GetDevices();
            return devices;
        }
    }
}