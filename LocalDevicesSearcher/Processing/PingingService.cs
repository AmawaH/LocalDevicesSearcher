﻿using LocalDevicesSearcher.Infrastructure.Logger;
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
        private ILogger _logger;
        private const int pingTimeout = 100;
        public PingingService(ILogger logger)
        {
            _logger = logger;
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
                    _logger.Log(msg);

                    return replyAddress;
                }
                else
                {
                    string msg = $"Pinged address: {replyAddress} Nothing here...";
                    _logger.Log(msg);

                    return null;
                }
            }
            catch (Exception ex)
            {
                string exMsg = ex.Message;

                string msg = $"Error {exMsg} while pinging {pingedAddress}";
                _logger.Log(msg);

                return null;
            }
        }
    }
}
