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
        private string _logFileName;
        private bool _canLogInFile;
        private ILogToConsoleService _logToConsoleService;
        private ILogToFileService _logToFileService;
        public Logger()
        {
            _canLogInFile = true;
            _logToConsoleService = new LogToConsoleService();
            _logToFileService = new LogToFileService();
        }
        public Logger(ILogToConsoleService logToConsoleService)
        {
            _logToConsoleService = logToConsoleService;
        }

        public Logger(ILogToConsoleService logToConsoleService, ILogToFileService logToFileService) : this(logToConsoleService)
        {
            _logToFileService = logToFileService;
        }
        public void SetLogFileName(string fileName)
        {
            _logFileName = fileName;
        }
        public void Log(string message)
        {
            string time = DateTime.Now.ToString("G");
            string content = $"{time}  {message}";
            _logToConsoleService.WriteToConsole(content);
            if (_canLogInFile)
            {
                _logToFileService.WriteToLogFile(_logFileName, content);
            }
        }
        public void CreateLogFile(string path)
        {
            _logFileName = $"{path}.log";
            ICanCreateFileValidator canCreateFileValidator = new CanCreateFileValidator();
            _canLogInFile = canCreateFileValidator.TryCreateFile(_logFileName);
        }
        public void DisableLogToFileLogging()
        {
            _canLogInFile = false;
        }
    }

}