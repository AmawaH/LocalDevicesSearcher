using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace LocalDevicesSearcher
{
    interface ILogService
    { 
        void WriteToConsole(string content);
        void WriteToLogFile(string logFileName, string content);
    }

    public class SimpleLogService : ILogService
    {
        public void WriteToConsole(string content)
        {

            Console.WriteLine(content);

        }
        public void WriteToLogFile(string logFileName, string content)
        {
            using (StreamWriter writer = new StreamWriter(logFileName, true))
            {
                writer.WriteLine(content);
            }

        }

    }

    class Logger
    {
        private ILogService logService;
        private string logFileName;
        public Logger(ILogService logService)
        {
            this.logService = logService;
        }
        public string GetFileName()
        {
            return logFileName;
        }
        
        public void Log(string message)
        {
            string time = DateTime.Now.ToString();
            string content = $"{time}  {message}";
            logService.WriteToConsole(content);
            logService.WriteToLogFile(logFileName,content);
            
        }

        //private string CreateDirectoryIfNotExists()
        //{
        //    string path = Environment.CurrentDirectory;
        //    string subpath = @"\Logs";
        //    string newpath = path + subpath;
        //    DirectoryInfo dirInfo = new DirectoryInfo(newpath);
        //    if (!dirInfo.Exists )
        //    {
        //        dirInfo.Create();
        //    }
        //    return newpath;
        //}

        public void CreateLogFile(string filename)
        {
            //string path = CreateDirectoryIfNotExists()+@"\";
            logFileName =$"{filename}.log";
            using (StreamWriter writer = new StreamWriter(logFileName, false)) {};
               
        }
    }

}
