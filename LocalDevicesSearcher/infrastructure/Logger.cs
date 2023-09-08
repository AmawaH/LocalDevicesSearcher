using System;
using System.IO;
using LocalDevicesSearcher.Validations;

namespace LocalDevicesSearcher.infrastructure
{
    public interface ILogToConsoleService
    {
        void WriteToConsole(string content);
    }

    public class LogToConsoleService : ILogToConsoleService
    {
        public void WriteToConsole(string content)
        {
            Console.WriteLine(content);
        }
    }
    public interface ILogToFileService
    { 
        void WriteToLogFile(string logFileName, string content);
    }

    public class LogToFileService : ILogToFileService
    { 
        public void WriteToLogFile(string logFileName, string content)
        {
            lock (this)
            {
                using (StreamWriter writer = new StreamWriter(logFileName, true))
                {
                    writer.WriteLine(content);
                }
            }
        }
    }

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
