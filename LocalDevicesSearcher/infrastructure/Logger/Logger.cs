using System;
using LocalDevicesSearcher.Validations;

namespace LocalDevicesSearcher.infrastructure.Logger
{
    public class Logger
    {
        private string logFileName;
        private bool canLogInFile = true;
        private ILogToConsoleService logToConsoleService;
        private ILogToFileService logToFileService;
        public Logger()
        {
            logToConsoleService = new LogToConsoleService();
            logToFileService = new LogToFileService();
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
    }

}