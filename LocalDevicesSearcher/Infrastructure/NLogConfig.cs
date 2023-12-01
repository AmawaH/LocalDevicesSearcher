using Microsoft.Extensions.Logging;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using System;

namespace LocalDevicesSearcher.Infrastructure
{
    public class NLogConfig
    {
        public NLogConfig() {
            var config = new LoggingConfiguration();
            //Targets:
            var fileTarget = new FileTarget("fileTarget")
            {
                FileName = DateTime.Now.ToString("yyyyMMdd_HHmmss"),
                Layout = "${longdate}|${level:uppercase=true}|${message} ${exception:format=ToString}"
            };
            var consoleTarget = new ConsoleTarget("consoleTarget")
            {
                Layout = "${longdate}|${level:uppercase=true}|${message} ${exception:format=ToString}"
            };
            config.AddTarget(fileTarget);
            config.AddTarget(consoleTarget);
            //Rules:
            config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, consoleTarget);
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, fileTarget);
            NLog.LogManager.Configuration = config;
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
