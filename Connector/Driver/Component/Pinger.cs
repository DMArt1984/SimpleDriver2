using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace WinSimpleIDriver.Connector.Driver.Component
{
    class Pinger
    {
        public static bool PingHost(string nameOrAddress, int timeout = 50)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress, timeout);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Ничего не делаем
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }
            return pingable;
        }
    }
}
