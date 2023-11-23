namespace LocalDevicesSearcher.Validations
{
    public interface IValidators
    {
        bool IsConnectedValidation(string ip);
        bool TryCreateFile(string filename);
    }
    public class Validators : IValidators
    {
        private IIpValidator isConnectedValidator;
        private ICanCreateFileValidator canCreateFileValidator;
        public Validators()
        {
            isConnectedValidator = new IsConnectedValidator();
            canCreateFileValidator = new CanCreateFileValidator();
        }
        public bool IsConnectedValidation(string ip)
        {
            return isConnectedValidator.IsConnectedValidation(ip);
        }
        public bool TryCreateFile(string filename)
        {
            return canCreateFileValidator.TryCreateFile(filename);
        }
    }
}
