using LocalDevicesSearcher.Validations;
using LocalDevicesSearcher.Processing;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using LocalDevicesSearcher.Models;
using System.Collections.Generic;
using LocalDevicesSearcher.infrastructure.Logger;
using LocalDevicesSearcher.infrastructure.ResultWriter;

namespace LocalDevicesSearcher
{
    public class Program
    {
        private readonly Logger logger;
        private readonly ResultWriter resultWriter;
        private readonly Validators validators;
        const int minSubnetRange = 0;
        const int maxSubnetRange = 256; 
        static readonly string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss"); // "TestDir/testfilename";
        private Program(Logger _logger, ResultWriter _resultWriter, Validators _validators)
        {
            logger = _logger;
            resultWriter = _resultWriter;
            validators = _validators;
        }
        public class Builder
        {
            private readonly Logger logger;
            private readonly ResultWriter resultWriter;
            private readonly Validators validators;
            public Builder()
            {
                logger = new Logger();
                resultWriter = new ResultWriter();
                validators = new Validators();
            }
            public Program Build()
            {
                return new Program(logger, resultWriter, validators);
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
            IPAddress selfLocalIp4 = GetSelfLocalIpAddress();
            string selfLocalIp4String = selfLocalIp4.ToString();

            string msg = $"Local device Ip detected: {selfLocalIp4String}";
            logger.Log(msg);

            bool isConnectedToNetwork = validators.IpValidation(selfLocalIp4String);
            if (isConnectedToNetwork)
            {
                string subnet = selfLocalIp4String.Substring(0, selfLocalIp4String.LastIndexOf('.') + 1);

                msg = $"Processing subnet {subnet}{minSubnetRange} - {subnet}{maxSubnetRange} :\n";
                logger.Log(msg);

                Parallel.ForEach(
                    Enumerable.Range(minSubnetRange, maxSubnetRange - minSubnetRange),
                        new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, // Кількість паралельних потоків
                        i =>
                        {
                            string addressInProcess = subnet + i;
                            var processor = new Processor(logger, resultWriter);
                            IPAddress detectedIp = processor.Pinging(addressInProcess);
                            if (detectedIp is not null)
                            { 
                                List<int> openedPorts = processor.PortsDetect(detectedIp);
                                IPAddress address = detectedIp.MapToIPv4();
                                var deviceCalculator = new DeviceDataCalculator(address, openedPorts);
                                Device device = deviceCalculator.GetDevice();
                                resultWriter.WriteResult(device);
                            }

                        });
            }
            else
            {
                logger.Log("Your device is not connected to any network");
            }
        }
        private static IPAddress GetSelfLocalIpAddress()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry ipHostEntry = Dns.GetHostEntry(hostName);
            IPAddress ip4 = ipHostEntry.AddressList
                .FirstOrDefault(ip => ip.AddressFamily.ToString() == ProtocolFamily.InterNetwork.ToString());
            return ip4;
        }
    }
}

