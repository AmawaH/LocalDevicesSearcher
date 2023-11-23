using LocalDevicesSearcher.Models;
using LocalDevicesSearcher.Validations;
using System.Collections.Generic;

namespace LocalDevicesSearcher.Infrastructure.ResultWriter
{
    public interface IResultWriter
    {
        void SetResultFileName(string fileName);
        void WriteResult(List<Device> devices);
        void CreateResultFile(string path);
    }
    public class ResultWriter : IResultWriter
    {
        private bool canWriteResultInFile;
        private string resultFileName;
        private IResultWriterService resultWriterService;
        public ResultWriter()
        {
            resultWriterService = new ResultWriterService();
        }
        public ResultWriter(IResultWriterService _resultWriterService)
        {
            resultWriterService = _resultWriterService;
        }
        public void SetResultFileName(string fileName)
        {
            resultFileName = fileName;
        }
        public void WriteResult(List<Device> devices)
        {
            if ((canWriteResultInFile)&&(devices != null))
            {
                foreach (Device device in devices)
                {
                    resultWriterService.WriteToResultFile(resultFileName, device);
                }
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