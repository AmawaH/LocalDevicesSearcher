using LocalDevicesSearcher.Models;
using LocalDevicesSearcher.Validations;
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
        private IResultWriterToFileService _resultWriterService;
        public ResultWriter()
        {
            _resultWriterService = new ResultWriterToFileService();
        }
        public ResultWriter(IResultWriterToFileService resultWriterService)
        {
            _resultWriterService = resultWriterService;
        }
        public void SetResultFileName(string fileName)
        {
            _resultFileName = fileName;
        }
        public void WriteResult(List<Device> devices)
        {
            if ((_canWriteResultInFile) && (devices != null))
            {
                foreach (Device device in devices)
                {
                    _resultWriterService.ResultWriteToFile(_resultFileName, device);
                }
            }
        }
        public void WriteResult(Device device)
        {
            if ((_canWriteResultInFile) && (device != null))
            {
                _resultWriterService.ResultWriteToFile(_resultFileName, device);
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