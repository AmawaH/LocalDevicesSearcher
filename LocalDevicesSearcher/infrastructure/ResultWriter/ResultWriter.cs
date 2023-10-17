using LocalDevicesSearcher.Validations;


namespace LocalDevicesSearcher.infrastructure.ResultWriter
{

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