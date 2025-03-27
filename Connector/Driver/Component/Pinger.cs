using System;
using System.Net.NetworkInformation;

namespace WinSimpleIDriver.Connector.Driver.Component
{
    class Pinger
    {
        public static bool PingHost(string nameOrAddress, int timeout = 50)
        {
            try
            {
                using (var pinger = new Ping())
                {
                    var reply = pinger.Send(nameOrAddress, timeout);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch (PingException)
            {
                // Пинг не удался — просто возвращаем false
                return false;
            }
            catch (Exception)
            {
                // Для безопасности — ловим другие исключения, если вдруг возникнут
                return false;
            }
        }
    }
}
