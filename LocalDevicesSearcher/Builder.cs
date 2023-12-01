using LocalDevicesSearcher.Infrastructure.ResultWriter;
using LocalDevicesSearcher.Infrastructure;
using LocalDevicesSearcher.Processing;
using LocalDevicesSearcher.Validations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using NLog.Extensions.Logging;
using NLog.Config;
using NLog.Targets;

namespace LocalDevicesSearcher
{
    public class Builder
    {
        private readonly ILogger _logger;
        private readonly IResultWriter _resultWriter;
        private readonly IIsConnectedValidator _isConnectedValidator;
        private readonly ISelfIpAddressGetter _selfIpAddressGetter;
        private readonly IDeviceSearcher _deviceSearcher;
        public Builder()
        {
            _logger = new NLogConfig().GetNLogLogger();
            _resultWriter = new ResultWriter();
            _isConnectedValidator = new IsConnectedValidator();
            _selfIpAddressGetter = new SelfIpAddressGetter();
            _deviceSearcher = new DeviceSearcher(_logger);
        }
        public Runner Build()
        {
            return new Runner(_logger, _resultWriter, _isConnectedValidator, _selfIpAddressGetter, _deviceSearcher);
        }
    }
}
