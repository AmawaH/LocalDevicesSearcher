using Microsoft.Extensions.Logging;
using NLog.Common;
using NLog.Extensions.Logging;

namespace LocalDevicesSearcher.Infrastructure.Logger
{
    public class NLogConfig
    {
        public NLogConfig()
        {
            InternalLogger.LogToConsole = true;
            InternalLogger.LogToConsoleError = true;
        }
        public ILogger GetNLogLogger()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddNLog();
            });
            ILogger logger = loggerFactory.CreateLogger<Program>();
            return logger;
        }
    }
}