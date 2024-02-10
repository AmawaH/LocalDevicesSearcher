using LocalDevicesSearcher.Models;
using LocalDevicesSearcher.Validations;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace LocalDevicesSearcher.Infrastructure.ResultWriter
{
    public interface IResultWriter
    {
        void WriteResult(List<Device> devices);
        void WriteResult(Device device);
        void CreateResultFile(string path);
    }
    public class ResultWriter : IResultWriter
    {
        private bool _canWriteResultInFile;
        private string _resultFileName;
        private IResultWriterToFileService _resultWriterToFileService;
        private IDeviceRepository _repository;
        public ResultWriter(IDeviceRepository repository)
        {
            _resultWriterToFileService = new ResultWriterToFileService();
            _repository = repository;
        }
        public ResultWriter(IDeviceRepository repository, IResultWriterToFileService resultWriterService)
        {
            _repository = repository;
            _resultWriterToFileService = resultWriterService;
        }
        public ResultWriter(IDeviceRepository repository, bool canWriteResultInFile, string resultFileName, IResultWriterToFileService resultWriterToFileService)
        {
            _repository = repository;
            _canWriteResultInFile = canWriteResultInFile;
            _resultFileName = resultFileName;
            _resultWriterToFileService = resultWriterToFileService;
        }
        public void WriteResult(List<Device> devices)
        {
            if (devices != null)
            {
                foreach (Device device in devices)
                {
                    if (_canWriteResultInFile)
                    {
                        _resultWriterToFileService.ResultWriteToFile(_resultFileName, device);
                    }
                    _repository.AddDevice(device);
                }
            }
        }
        public void WriteResult(Device device)
        {
            if (device != null)
            {
                if (_canWriteResultInFile)
                {
                    _resultWriterToFileService.ResultWriteToFile(_resultFileName, device);
                }
                _repository.AddDevice(device);
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