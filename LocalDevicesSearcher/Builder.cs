using LocalDevicesSearcher.Infrastructure.Logger;
using LocalDevicesSearcher.Infrastructure;
using LocalDevicesSearcher.Validations;
using Microsoft.Extensions.Logging;

namespace LocalDevicesSearcher
{
    public class Builder
    {
        private readonly ILogger _logger;
        private readonly IIsConnectedValidator _isConnectedValidator;
        private readonly ISelfIpAddressGetter _selfIpAddressGetter;
        private readonly IConfig _config;
        public Builder()
        {
            _logger = new NLogConfig().GetNLogLogger();
            _isConnectedValidator = new IsConnectedValidator();
            _selfIpAddressGetter = new SelfIpAddressGetter();
            _config = new Config();
        }
        public Runner Build()
        {
            return new Runner(_logger, _isConnectedValidator, _selfIpAddressGetter, _config);
        }
    }
}