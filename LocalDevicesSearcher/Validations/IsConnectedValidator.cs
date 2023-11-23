namespace LocalDevicesSearcher.Validations
{
    public interface IIpValidator
    {
        bool IsConnectedValidation(string ip);
    }
    public class IsConnectedValidator : IIpValidator
    {
        public bool IsConnectedValidation(string ip)
        {
            bool result = (ip != "127.0.0.1");
            return result;
        }
    }

}
