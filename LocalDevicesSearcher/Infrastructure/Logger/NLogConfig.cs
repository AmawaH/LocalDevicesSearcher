using Microsoft.Extensions.Logging;
using NLog.Common;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using System;

namespace LocalDevicesSearcher.Infrastructure.Logger
{
    public class NLogConfig
    {
        public NLogConfig()
        {
            InternalLogger.LogToConsole = true;
            InternalLogger.LogToConsoleError = true;
            var config = new LoggingConfiguration();
            string datetime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"{datetime}.log";
            //Targets:
            var fileTarget = new FileTarget("fileTarget")
            {
                FileName = fileName,
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
        public static ILogger GetNLogLogger()
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
