namespace LocalDevicesSearcher.Validations
{
    public interface IIpValidator
    {
        bool IpValidation(string ip);
    }
    public class IpValidator : IIpValidator
    {
        public bool IpValidation(string ip)
        {
            return (ip != "127.0.0.1");
        }
    }

}
