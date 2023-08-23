using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LocalDevicesSearcher
{
    interface IResultWriterService
    {
        void WriteToResultFile(string resultFileName, Device device);
    }

    public class ResultWriterService : IResultWriterService
    {
        public void WriteToResultFile(string resultFileName, Device device)
        {
            using (StreamWriter writer = new StreamWriter(resultFileName, true))
            {
                var content = JsonSerializer.Serialize<Device>(device);
                writer.WriteLine(content);
            }

        }

    }

    class ResultWriter
    {
        private IResultWriterService resultWriterService;
        private string resultFileName;
        public ResultWriter(IResultWriterService resultWriterService)
        {
            this.resultWriterService = resultWriterService;
        }

        public void WriteResult(Device device)
        {
            resultWriterService.WriteToResultFile(resultFileName, device);
        }

        //private string CreateDirectoryIfNotExists()
        //{
        //    string path = Environment.CurrentDirectory;
        //    string subpath = @"\Result";
        //    string newpath = path + subpath;
        //    DirectoryInfo dirInfo = new DirectoryInfo(newpath);
        //    if (!dirInfo.Exists)
        //    {
        //        dirInfo.Create();
        //    }
        //    return newpath;
        //}

        public void CreateResultFile(string fileName)
        {
            //string path = CreateDirectoryIfNotExists() + @"\";
            resultFileName = $"{fileName}.json";
            using (StreamWriter writer = new StreamWriter(resultFileName, false)) { };

        }
    }
}
