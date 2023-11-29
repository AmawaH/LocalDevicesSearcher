using LocalDevicesSearcher.Infrastructure.Logger;
using LocalDevicesSearcher.Infrastructure.ResultWriter;
using LocalDevicesSearcher.Infrastructure;
using LocalDevicesSearcher.Processing;
using LocalDevicesSearcher.Validations;
using System;
using System.Collections.Generic;
using LocalDevicesSearcher.Models;
using System.Net;

namespace LocalDevicesSearcher
{
    public class Runner
    {
        private readonly ILogger _logger;
        private readonly IResultWriter _resultWriter;
        private readonly IIsConnectedValidator _isConnectedValidator;
        private readonly ISelfIpAddressGetter _selfLocalIpAddressGetter;
        private readonly IDeviceSearcher _deviceSearcher;

        const int minSubnetRange = 1;
        const int maxSubnetRange = 256;
        static readonly string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss"); // "TestDir/testfilename";
        public Runner(ILogger logger,
            IResultWriter resultWriter,
            IIsConnectedValidator isConnectedValidator,
            ISelfIpAddressGetter selfIpAddressGetter,
            IDeviceSearcher deviceSearcher)
        {
            _logger = logger;
            _resultWriter = resultWriter;
            _isConnectedValidator = isConnectedValidator;
            _selfLocalIpAddressGetter = selfIpAddressGetter;
            _deviceSearcher = deviceSearcher;
        }
        public void Run()
        {
            _logger.CreateLogFile(fileName);
            _resultWriter.CreateResultFile(fileName);
            IPAddress selfLocalIp4 = _selfLocalIpAddressGetter.GetSelfIp4Address();
            string selfLocalIp4String = selfLocalIp4.ToString();

            string msg = $"Local device Ip detected: {selfLocalIp4String}";
            _logger.Log(msg);

            bool isConnectedToNetwork = _isConnectedValidator.IsConnectedValidation(selfLocalIp4String);
            if (isConnectedToNetwork)
            {
                string subnet = selfLocalIp4String.Substring(0, selfLocalIp4String.LastIndexOf('.') + 1);

                msg = $"Processing subnet {subnet}{minSubnetRange} - {subnet}{maxSubnetRange} :\n";
                _logger.Log(msg);
                List<Device> devices = _deviceSearcher.DevicesSearch(minSubnetRange, maxSubnetRange, subnet);
                _resultWriter.WriteResult(devices);
            }
            else
            {
                _logger.Log("Your device is not connected to any network");
            }
        }
    }
}
