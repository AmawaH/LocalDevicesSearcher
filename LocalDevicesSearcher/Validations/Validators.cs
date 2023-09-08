namespace LocalDevicesSearcher.Validations
{
    public class Validators
    {
        private IIpValidator ipValidator;
        private ICreateFileValidator CreateFileValidator;
        public Validators()
        {
            ipValidator = new IpValidator();
            CreateFileValidator = new CreateFileValidator();
        }
        public bool IpValidation(string ip)
        {
            return ipValidator.IpValidation(ip);
        }
        public bool TryCreateFile(string filename)
        {
            return CreateFileValidator.TryCreateFile(filename);
        }
    }
}
