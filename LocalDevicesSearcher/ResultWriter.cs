using System.IO;
using System.Text.Json;

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

        public void CreateResultFile(string fileName)
        {
            resultFileName = $"{fileName}.json";
            using (StreamWriter writer = new StreamWriter(resultFileName, false)) { };

        }
    }
}
