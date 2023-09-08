using LocalDevicesSearcher.Validations;
using Newtonsoft.Json;
using System;
using System.IO;


namespace LocalDevicesSearcher.infrastructure
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

    public class ResultWriter
    {
        private bool canWriteResultInFile;
        private string resultFileName;
        private IResultWriterService resultWriterService;
        public ResultWriter()
        {
            resultWriterService = new ResultWriterService();
        }

        public void WriteResult(Device device)
        {
            if (canWriteResultInFile)
            {
                resultWriterService.WriteToResultFile(resultFileName, device);
            }
        }

        public void CreateResultFile(string path)
        {
            resultFileName = $"{path}.json";
            var validators = new Validators();
            canWriteResultInFile = validators.TryCreateFile(resultFileName);
        }
    }
}
