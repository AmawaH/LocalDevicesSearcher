using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LocalDevicesSearcher.Infrastructure.ResultWriter;
using LocalDevicesSearcher.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LocalDevicesSearcher.Processing
{
    public interface IDeviceSearcher
    {
        void DevicesSearch(string subnet);
    }
    public class DeviceSearcher : IDeviceSearcher
    {
        private ILogger _logger;
        private IResultWriter _resultWriter;
        private IPingingService _pingingService;
        private IPortDetectService _portsDetectService;
        private IConfiguration _configuration;
        public DeviceSearcher(ILogger logger, IResultWriter resultWriter, IConfiguration configuration)
        {
            _logger = logger;
            _resultWriter = resultWriter;
            _pingingService = new PingingService(logger);
            _portsDetectService = new PortsDetectService(logger);
            _configuration = configuration;
        }
        public DeviceSearcher(ILogger logger, IResultWriter resultWriter, IConfiguration configuration, IPingingService pingingService) : this(logger, resultWriter, configuration)
        {
            _pingingService = pingingService;
        }
        public DeviceSearcher(ILogger logger, IResultWriter resultWriter, IPingingService pingingService, IConfiguration configuration, IPortDetectService portsDetectService) : this(logger, resultWriter, configuration, pingingService)
        {
            _portsDetectService = portsDetectService;
        }
        public void DevicesSearch(string subnet)
        {
            IEnumerable<int> subnetCollection;
            if (_configuration.GetValue<bool>("Constants:Range"))
            {
                int minSubnetRange = _configuration.GetValue<int>("Constants:MinSubnetRange");
                int maxSubnetRange = _configuration.GetValue<int>("Constants:MaxSubnetRange");

                string msg = $"Processing IPs: {subnet}{minSubnetRange} - {subnet}{maxSubnetRange} :\n";
                _logger.LogInformation(msg);

                subnetCollection = Enumerable.Range(minSubnetRange, maxSubnetRange - minSubnetRange);
            }
            else
            {
                subnetCollection = _configuration.GetSection("Constants:SubnetCollection").Get<IEnumerable<int>>();

                List<string> ls = subnetCollection.Select(item => subnet + item.ToString()).ToList();
                string s = string.Join(" ,", ls);
                string msg = $"Processing IPs: {s}";
                _logger.LogInformation(msg);

            }
            Parallel.ForEach(subnetCollection, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                i =>
                {
                    string addressInProcess = subnet + i;
                    IPAddress detectedIp = _pingingService.Pinging(addressInProcess);
                    if (detectedIp is not null)
                    {
                        List<int> openedPorts = _portsDetectService.PortsDetect(detectedIp);
                        Device device = new DeviceCalculator().CalculateDevice(detectedIp, openedPorts);
                        _resultWriter.WriteResult(device);
                    }
                });
        }
    }
}