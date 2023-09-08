using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using LocalDevicesSearcher.infrastructure;
using LocalDevicesSearcher.Models;

namespace LocalDevicesSearcher.Processing
{
    public class Processor
    {
        private Logger logger;
        private ResultWriter resultWriter;
        private const int pingTimeout = 100;

        public Processor(Logger _logger, ResultWriter _resultWriter)
        {
            logger = _logger;
            resultWriter = _resultWriter;
        }
        public void Pinging(string pingedAddress)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("PingingMessage");
            Ping pinger = new Ping();
            PingReply reply = pinger.Send(pingedAddress, pingTimeout, buffer);
            IPAddress replyAddress = reply.Address;
            if (reply.Status == IPStatus.Success)
            {
                string msg = $"Pinged address: {replyAddress} Found something!";
                logger.Log(msg);

                IPAddress address = replyAddress.MapToIPv4();
                var deviceCalculator = new DeviceDataCalculator(address);
                Device device = deviceCalculator.GetDevice();
                resultWriter.WriteResult(device);
            }
            else
            {
                string msg = $"Pinged address: {replyAddress} Nothing here...";
                logger.Log(msg);
            }
        }



    }
}