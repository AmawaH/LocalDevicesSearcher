using LocalDevicesSearcher.Infrastructure.ResultWriter;
using LocalDevicesSearcher.Infrastructure;
using LocalDevicesSearcher.Processing;
using LocalDevicesSearcher.Validations;
using System;
using LocalDevicesSearcher.Models;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using LocalDevicesSearcher.Models.EFDB;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LocalDevicesSearcher
{
    public class Runner
    {
        private readonly ILogger _logger;
        private readonly IResultWriter _resultWriter;
        private readonly IIsConnectedValidator _isConnectedValidator;
        private readonly ISelfIpAddressGetter _selfLocalIpAddressGetter;
        const int minSubnetRange = 1;
        const int maxSubnetRange = 256;
        static readonly string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss"); // "TestDir/testfilename";
        public Runner(ILogger logger,
            IResultWriter resultWriter,
            IIsConnectedValidator isConnectedValidator,
            ISelfIpAddressGetter selfIpAddressGetter)
        {
            _logger = logger;
            _resultWriter = resultWriter;
            _isConnectedValidator = isConnectedValidator;
            _selfLocalIpAddressGetter = selfIpAddressGetter;
        }
        public void Run()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .Build();
            var services = new ServiceCollection();
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            //services for EF:
            services.AddDbContext<DeviceDbContext>(options => options.UseSqlServer(connectionString))
                    .AddTransient<IDeviceRepository, EFDeviceRepository>();
            var serviceProvider = services.BuildServiceProvider();
            _resultWriter.CreateResultFile(fileName);
            IPAddress selfLocalIp4 = _selfLocalIpAddressGetter.GetSelfIp4Address();
            string selfLocalIp4String = selfLocalIp4.ToString();

            string msg = $"Local device Ip detected: {selfLocalIp4String}";
            _logger.LogInformation(msg);

            bool isConnectedToNetwork = _isConnectedValidator.IsConnectedValidation(selfLocalIp4String);
            if (isConnectedToNetwork)
            {
                string subnet = selfLocalIp4String.Substring(0, selfLocalIp4String.LastIndexOf('.') + 1);

                msg = $"Processing subnet {subnet}{minSubnetRange} - {subnet}{maxSubnetRange} :\n";
                _logger.LogInformation(msg);

                IDeviceSearcher deviceSearcher = new DeviceSearcher(_logger, _resultWriter, serviceProvider);
                deviceSearcher.DevicesSearch(minSubnetRange, maxSubnetRange, subnet);
            }
            else
            {
                _logger.LogWarning("Your device is not connected to any network");
            }
            NLog.LogManager.Shutdown();
        }
    }
}