using LocalDevicesSearcher.Models;
using Newtonsoft.Json;
using System.IO;

namespace LocalDevicesSearcher.Infrastructure.ResultWriter
{
    public interface IResultWriterToFileService
    {
        void ResultWriteToFile(string resultFileName, Device device);
    }
    public class ResultWriterToFileService : IResultWriterToFileService
    {
        public void ResultWriteToFile(string resultFileName, Device device)
        {
            lock (this)
            {
                using (StreamWriter writer = new StreamWriter(resultFileName, true))
                {
                    var content = JsonConvert.SerializeObject(device);
                    writer.WriteLine(content);
                    writer.Flush();
                    writer.Close();
                }
            }
        }
    }
}