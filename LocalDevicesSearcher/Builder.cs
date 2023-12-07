using LocalDevicesSearcher.Infrastructure.ResultWriter;
using LocalDevicesSearcher.Infrastructure.Logger;
using LocalDevicesSearcher.Infrastructure;
using LocalDevicesSearcher.Validations;
using Microsoft.Extensions.Logging;

namespace LocalDevicesSearcher
{
    public class Builder
    {
        private readonly ILogger _logger;
        private readonly IResultWriter _resultWriter;
        private readonly IIsConnectedValidator _isConnectedValidator;
        private readonly ISelfIpAddressGetter _selfIpAddressGetter;
        public Builder()
        {
            _logger = new NLogConfig().GetNLogLogger();
            _resultWriter = new ResultWriter();
            _isConnectedValidator = new IsConnectedValidator();
            _selfIpAddressGetter = new SelfIpAddressGetter();
        }
        public Runner Build()
        {
            return new Runner(_logger, _resultWriter, _isConnectedValidator, _selfIpAddressGetter);
        }
    }
}