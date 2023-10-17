using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace LocalDevicesSearcher.infrastructure.ResultWriter
{
    public interface IResultWriterService
    {
        void WriteToResultFile(string resultFileName, Device device);
    }
    public class ResultWriterService : IResultWriterService
    {
        public void WriteToResultFile(string resultFileName, Device device)
        {
            lock (this)
            {
                using (StreamWriter writer = new StreamWriter(resultFileName, true))
                {
                    var content = JsonConvert.SerializeObject(device);
                    writer.WriteLine(content);
                }
            }
        }
    }
}
