using LocalDevicesSearcher.Infrastructure.ResultWriter;
using LocalDevicesSearcher.Infrastructure;
using LocalDevicesSearcher.Processing;
using LocalDevicesSearcher.Validations;
using System;
using LocalDevicesSearcher.Models;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace LocalDevicesSearcher
{
    public class Runner
    {
        private readonly ILogger _logger;
        private readonly IResultWriter _resultWriter;
        private readonly IIsConnectedValidator _isConnectedValidator;
        private readonly ISelfIpAddressGetter _selfLocalIpAddressGetter;
        private readonly IConfiguration _configuration;
        private readonly IServiceProviderFactory _serviceProviderFactory;
        static readonly string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss"); // "TestDir/testfilename";
        public Runner(ILogger logger,
            IResultWriter resultWriter,
            IIsConnectedValidator isConnectedValidator,
            ISelfIpAddressGetter selfIpAddressGetter,
            IConfiguration configuration)
        {
            _logger = logger;
            _resultWriter = resultWriter;
            _isConnectedValidator = isConnectedValidator;
            _selfLocalIpAddressGetter = selfIpAddressGetter;
            _configuration = configuration;
        }
        public void Run()
        {
            IServiceProvider serviceProvider = new ServiceProviderFactory(_configuration).ServiceProvider;
            _resultWriter.CreateResultFile(fileName);
            IPAddress selfLocalIp4 = _selfLocalIpAddressGetter.GetSelfIp4Address();
            string selfLocalIp4String = selfLocalIp4.ToString();

            string msg = $"Local device Ip detected: {selfLocalIp4String}";
            _logger.LogInformation(msg);

            bool isConnectedToNetwork = _isConnectedValidator.IsConnectedValidation(selfLocalIp4String);
            if (isConnectedToNetwork)
            {
                IDeviceSearcher deviceSearcher = new DeviceSearcher(_logger, _resultWriter, serviceProvider);
                string subnet = selfLocalIp4String.Substring(0, selfLocalIp4String.LastIndexOf('.') + 1);

                if (_configuration.GetValue<bool>("Constants:Range"))
                {
                    int minSubnetRange = _configuration.GetValue<int>("Constants:MinSubnetRange");
                    int maxSubnetRange = _configuration.GetValue<int>("Constants:MaxSubnetRange");
                    deviceSearcher.DevicesSearch(subnet, minSubnetRange, maxSubnetRange);
                }
                else
                {
                    List<int> subnetCollection = _configuration.GetSection("Constants:SubnetCollection").Get<List<int>>();
                    deviceSearcher.DevicesSearch(subnet, subnetCollection);
                }
            }
            else
            {
                _logger.LogWarning("Your device is not connected to any network");
            }
            NLog.LogManager.Shutdown();
        }
    }
}