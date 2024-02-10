using LocalDevicesSearcher.Infrastructure.ResultWriter;
using LocalDevicesSearcher.Infrastructure;
using LocalDevicesSearcher.Processing;
using LocalDevicesSearcher.Validations;
using System;
using LocalDevicesSearcher.Models;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LocalDevicesSearcher
{
    public class Runner
    {
        private readonly ILogger _logger;
        private readonly IIsConnectedValidator _isConnectedValidator;
        private readonly ISelfIpAddressGetter _selfLocalIpAddressGetter;
        private readonly IConfiguration _configuration;

        static readonly string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss"); // "TestDir/testfilename";
        public Runner(ILogger logger,
            IIsConnectedValidator isConnectedValidator,
            ISelfIpAddressGetter selfIpAddressGetter,
            IConfiguration configuration)
        {
            _logger = logger;
            _isConnectedValidator = isConnectedValidator;
            _selfLocalIpAddressGetter = selfIpAddressGetter;
            _configuration = configuration;
        }
        public void Run()
        {
            IServiceProviderFactory serviceProviderFactory = new ServiceProviderFactory(_configuration);
            IDeviceRepository repository = serviceProviderFactory.ServiceProvider.GetService<IDeviceRepository>();
            IResultWriter resultWriter = new ResultWriter(repository);
            resultWriter.CreateResultFile(fileName);
            IPAddress selfLocalIp4 = _selfLocalIpAddressGetter.GetSelfIp4Address();
            string selfLocalIp4String = selfLocalIp4.ToString();

            string msg = $"Local device Ip detected: {selfLocalIp4String}";
            _logger.LogInformation(msg);

            bool isConnectedToNetwork = _isConnectedValidator.IsConnectedValidation(selfLocalIp4String);
            if (isConnectedToNetwork)
            {
                IDeviceSearcher deviceSearcher = new DeviceSearcher(_logger, resultWriter, _configuration);
                string subnet = selfLocalIp4String.Substring(0, selfLocalIp4String.LastIndexOf('.') + 1);
                deviceSearcher.DevicesSearch(subnet);
            }
            else
            {
                _logger.LogWarning("Your device is not connected to any network");
            }
            NLog.LogManager.Shutdown();
        }
    }
}