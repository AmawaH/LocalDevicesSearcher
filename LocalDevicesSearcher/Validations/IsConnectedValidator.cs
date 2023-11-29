namespace LocalDevicesSearcher.Validations
{
    public interface IIsConnectedValidator
    {
        bool IsConnectedValidation(string ip);
    }
    public class IsConnectedValidator : IIsConnectedValidator
    {
        public bool IsConnectedValidation(string ip)
        {
            bool result = (ip != "127.0.0.1");
            return result;
        }
    }

}
