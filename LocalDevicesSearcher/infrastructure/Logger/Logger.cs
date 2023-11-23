using System;
using LocalDevicesSearcher.Validations;

namespace LocalDevicesSearcher.Infrastructure.Logger
{
    public interface ILogger
    {
        void SetLogFileName(string fileName);
        void Log(string message);
        void CreateLogFile(string path);
        void DisableLogToFileLogging();
    }
    public class Logger : ILogger
    {
        private string logFileName;
        private bool canLogInFile;
        private ILogToConsoleService logToConsoleService;
        private ILogToFileService logToFileService;
        public Logger()
        {
            canLogInFile = true;
            logToConsoleService = new LogToConsoleService();
            logToFileService = new LogToFileService();
        }
        public Logger(ILogToConsoleService _logToConsoleService)
        {
            logToConsoleService = _logToConsoleService;
        }

        public Logger(ILogToConsoleService _logToConsoleService, ILogToFileService _logToFileService) : this(_logToConsoleService)
        {
            logToFileService = _logToFileService;
        }
        public void SetLogFileName(string fileName)
        {
            logFileName = fileName;
        }
        public void Log(string message)
        {
            string time = DateTime.Now.ToString("G");
            string content = $"{time}  {message}";
            logToConsoleService.WriteToConsole(content);
            if (canLogInFile)
            {
                logToFileService.WriteToLogFile(logFileName, content);
            }
        }
        public void CreateLogFile(string path)
        {
            logFileName = $"{path}.log";
            var validators = new Validators();
            canLogInFile = validators.TryCreateFile(logFileName);
        }
        public void DisableLogToFileLogging()
        {
            canLogInFile = false;
        }
    }

}