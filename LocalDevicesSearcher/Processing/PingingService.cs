using LocalDevicesSearcher.Infrastructure.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace LocalDevicesSearcher.Processing
{
    public interface IPingingService
    {
        IPAddress Pinging(string pingedAddress);
    }
    public class PingingService : IPingingService
    {
        private ILogger logger;
        private const int pingTimeout = 100;
        public PingingService(ILogger _logger)
        {
            logger = _logger;
        } 
        public IPAddress Pinging(string pingedAddress)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("PingingMessage");
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(pingedAddress, pingTimeout, buffer);
                IPAddress replyAddress = reply.Address;
                if (reply.Status == IPStatus.Success)
                {
                    string msg = $"Pinged address: {replyAddress} Found something!";
                    logger.Log(msg);

                    return replyAddress;
                }
                else
                {
                    string msg = $"Pinged address: {replyAddress} Nothing here...";
                    logger.Log(msg);

                    return null;
                }
            }
            catch (Exception ex)
            {
                string exMsg = ex.Message;

                string msg = $"Error {exMsg} while pinging {pingedAddress}";
                logger.Log(msg);

                return null;
            }
        }
    }
}
