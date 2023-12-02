using LocalDevicesSearcher.Models;
using LocalDevicesSearcher.Validations;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace LocalDevicesSearcher.Infrastructure.ResultWriter
{
    public interface IResultWriter
    {
        void SetResultFileName(string fileName);
        void WriteResult(List<Device> devices);
        void WriteResult(Device device);
        void CreateResultFile(string path);
    }
    public class ResultWriter : IResultWriter
    {
        private bool _canWriteResultInFile;
        private string _resultFileName;
        private IResultWriterService _resultWriterService;
        public ResultWriter()
        {
            _resultWriterService = new ResultWriterService();
        }
        public ResultWriter(IResultWriterService resultWriterService) 
        {
            _resultWriterService = resultWriterService;
        }
        public void SetResultFileName(string fileName)
        {
            _resultFileName = fileName;
        }
        public void WriteResult(List<Device> devices)
        {
            if ((_canWriteResultInFile)&&(devices != null))
            {
                foreach (Device device in devices)
                {
                    _resultWriterService.WriteToResultFile(_resultFileName, device);
                }
            }
        }
        public void WriteResult(Device device)
        {
            if ((_canWriteResultInFile)&&(device != null))
            { 
                _resultWriterService.WriteToResultFile(_resultFileName, device);
            }
        }
        public void CreateResultFile(string path)
        {
            _resultFileName = $"{path}.json";
            ICanCreateFileValidator canCreateFileValidator = new CanCreateFileValidator();
            _canWriteResultInFile = canCreateFileValidator.TryCreateFile(_resultFileName);
        }
    }
}