using System;

namespace LocalDevicesSearcher.infrastructure.Logger
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
}