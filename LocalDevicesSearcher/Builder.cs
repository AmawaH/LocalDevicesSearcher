using LocalDevicesSearcher.Infrastructure.ResultWriter;
using LocalDevicesSearcher.Infrastructure.Logger;
using LocalDevicesSearcher.Infrastructure;
using LocalDevicesSearcher.Validations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LocalDevicesSearcher
{
    public class Builder
    {
        private readonly ILogger _logger;
        private readonly IIsConnectedValidator _isConnectedValidator;
        private readonly ISelfIpAddressGetter _selfIpAddressGetter;
        private readonly IConfiguration _configuration;
        public Builder()
        {
            _logger = new NLogConfig().GetNLogLogger();
            _isConnectedValidator = new IsConnectedValidator();
            _selfIpAddressGetter = new SelfIpAddressGetter();
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .Build();
        }
        public Runner Build()
        {
            return new Runner(_logger, _isConnectedValidator, _selfIpAddressGetter, _configuration);
        }
    }
}