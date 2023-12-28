using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LocalDevicesSearcher.Infrastructure.ResultWriter;
using LocalDevicesSearcher.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LocalDevicesSearcher.Processing
{
    public interface IDeviceSearcher
    {
        void DevicesSearch(string subnet, int minSubnetRange, int maxSubnetRange);
        void DevicesSearch(string subnet, IEnumerable<int> subnetCollection);
    }
    public class DeviceSearcher : IDeviceSearcher
    {
        private ILogger _logger;
        private IResultWriter _resultWriter;
        private IPingingService _pingingService;
        private IPortDetectService _portsDetectService;
        private IDeviceRepository _deviceRepository;
        public DeviceSearcher(ILogger logger, IResultWriter resultWriter, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _resultWriter = resultWriter;
            _pingingService = new PingingService(logger);
            _portsDetectService = new PortsDetectService(logger);
            _deviceRepository = serviceProvider.GetRequiredService<IDeviceRepository>();
        }
        public DeviceSearcher(ILogger logger, IResultWriter resultWriter, IDeviceRepository deviceRepository)
        {
            _logger = logger;
            _resultWriter = resultWriter;
            _pingingService = new PingingService(logger);
            _portsDetectService = new PortsDetectService(logger);
            _deviceRepository = deviceRepository;
        }
        public DeviceSearcher(ILogger logger, IResultWriter resultWriter, IPingingService pingingService, IDeviceRepository deviceRepository) : this(logger, resultWriter, deviceRepository)
        {
            _pingingService = pingingService;
        }
        public DeviceSearcher(ILogger logger, IResultWriter resultWriter, IPingingService pingingService, IPortDetectService portsDetectService, IDeviceRepository deviceRepository) : this(logger, resultWriter, pingingService, deviceRepository)
        {
            _portsDetectService = portsDetectService;
        }
        public void DevicesSearch(string subnet, int minSubnetRange, int maxSubnetRange)
        {
            string msg = $"Processing subnet {subnet}{minSubnetRange} - {subnet}{maxSubnetRange} :\n";
            _logger.LogInformation(msg);

            IEnumerable<int> subnetCollection = Enumerable.Range(minSubnetRange, maxSubnetRange - minSubnetRange);
            Searching(subnetCollection, subnet);
        }
        public void DevicesSearch(string subnet, IEnumerable<int> subnetCollection)
        {

            List<string> ls = subnetCollection.Select(item => subnet + item.ToString()).ToList();
            string s = string.Join(" ,", ls);
            string msg = $"Processing subnets: {s}";
            _logger.LogInformation(msg);

            Searching(subnetCollection, subnet);
        }
        private void Searching(IEnumerable<int> subnetCollection, string subnet)
        {
            Parallel.ForEach(
                subnetCollection,
                new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                i =>
                {
                    string addressInProcess = subnet + i;
                    IPAddress detectedIp = _pingingService.Pinging(addressInProcess);
                    if (detectedIp is not null)
                    {
                        List<int> openedPorts = _portsDetectService.PortsDetect(detectedIp);
                        Device device = new DeviceCalculator().CalculateDevice(detectedIp, openedPorts);
                        _resultWriter.WriteResult(device);
                        _deviceRepository.AddDevice(device);
                    }
                });

        }
    }
}