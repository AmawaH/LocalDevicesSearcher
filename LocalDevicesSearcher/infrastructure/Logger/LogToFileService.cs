using System.IO;

namespace LocalDevicesSearcher.infrastructure.Logger
{
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
}