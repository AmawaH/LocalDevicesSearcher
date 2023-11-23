using LocalDevicesSearcher.Validations;
using LocalDevicesSearcher.Processing;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using LocalDevicesSearcher.Models;
using System.Collections.Generic;
using LocalDevicesSearcher.Infrastructure.Logger;
using LocalDevicesSearcher.Infrastructure.ResultWriter;
using LocalDevicesSearcher.Infrastructure;

namespace LocalDevicesSearcher
{
    public class Program
    {
        private readonly ILogger logger;
        private readonly IResultWriter resultWriter;
        private readonly IValidators validators;
        private readonly ISelfIpAddressGetter selfLocalIpAddressGetter;
        private readonly IDeviceSearcher deviceSearcher;

        const int minSubnetRange = 1;
        const int maxSubnetRange = 256; 
        static readonly string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss"); // "TestDir/testfilename";
        private Program(ILogger _logger,
            IResultWriter _resultWriter,
            IValidators _validators,
            ISelfIpAddressGetter _selfIpAddressGetter,
            IDeviceSearcher _deviceSearcher)
        {
            logger = _logger;
            resultWriter = _resultWriter;
            validators = _validators;
            selfLocalIpAddressGetter = _selfIpAddressGetter;
            deviceSearcher = _deviceSearcher;
        }
        public class Builder
        {
            private readonly ILogger logger;
            private readonly IResultWriter resultWriter;
            private readonly IValidators validators;
            private readonly ISelfIpAddressGetter selfIpAddressGetter;
            private readonly IDeviceSearcher deviceSearcher;

            public Builder()
            {
                logger = new Logger();
                resultWriter = new ResultWriter();
                validators = new Validators();
                selfIpAddressGetter = new SelfIpAddressGetter();
                deviceSearcher = new DeviceSearcher(logger);
            }
            public Program Build()
            {
                return new Program(logger, resultWriter, validators, selfIpAddressGetter, deviceSearcher);
            }
        }
        public static void Main()
        {
            var program = new Builder()
                .Build();
            program.Run();
        }


        public void Run()
        {
            logger.CreateLogFile(fileName);
            resultWriter.CreateResultFile(fileName);
            IPAddress selfLocalIp4 = selfLocalIpAddressGetter.GetSelfIp4Address();
            string selfLocalIp4String = selfLocalIp4.ToString();

            string msg = $"Local device Ip detected: {selfLocalIp4String}";
            logger.Log(msg);

            bool isConnectedToNetwork = validators.IsConnectedValidation(selfLocalIp4String);
            if (isConnectedToNetwork)
            {
                string subnet = selfLocalIp4String.Substring(0, selfLocalIp4String.LastIndexOf('.') + 1);

                msg = $"Processing subnet {subnet}{minSubnetRange} - {subnet}{maxSubnetRange} :\n";
                logger.Log(msg);
                List<Device> devices = deviceSearcher.DevicesSearch(minSubnetRange, maxSubnetRange, subnet);
                resultWriter.WriteResult(devices);
            }
            else
            {
                logger.Log("Your device is not connected to any network");
            }
        }
    }
}

