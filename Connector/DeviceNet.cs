using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WinSimpleIDriver.Connector.Driver.Component;
using WinSimpleIDriver.Connector.SGT;

namespace WinSimpleIDriver.Connector
{
    public interface INetDevice
    {
        bool IsPing(string hostUri, int timeout);
        CodeMessage IsHost(string hostUri, int portNumber);
    }

    class DeviceNet : Device, INetDevice
    {
        protected int timeout = 100; // время ожидания ответа

        protected string IP = "localhost"; // host

        protected int port = 502;

        protected byte unitIdentifier = 1;

        public bool disableHostForOpen = false;

        // Есть ли связь по Ethernet
        public virtual bool IsPing(string IP = "", int timeout = 0)
        {
            bool ping = (String.IsNullOrWhiteSpace(IP)) ? Pinger.PingHost(this.IP, (timeout > 0) ? timeout : this.timeout) : Pinger.PingHost(IP, (timeout > 0) ? timeout : this.timeout);
            return ping;
        }

        public CodeMessage IsHost(string hostUri = "", int portNumber = 0)
        {
            if (String.IsNullOrWhiteSpace(hostUri))
                hostUri = this.IP;

            if (portNumber <= 0)
                portNumber = this.port;

            try
            {
                var client = new TcpClient();
                var result = client.BeginConnect(hostUri, this.port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(3));
                if (!success)
                {
                    return new CodeMessage(-404, "Connection timeout (3sec)");
                }
                // we have connected
                client.EndConnect(result);
                return new CodeMessage(0, "");

            }
            catch (SocketException ex)
            {
                return new CodeMessage(ex.HResult, ex.Message);
            }
        }

        public CodeMessage IsHost2(string hostUri = "", int portNumber = 0)
        {
            if (String.IsNullOrWhiteSpace(hostUri))
                hostUri = this.IP;

            if (portNumber <= 0)
                portNumber = this.port;

            try
            {
                using (var client = new TcpClient(hostUri, portNumber))
                    return new CodeMessage(0, "");
            }
            catch (SocketException ex)
            {
                return new CodeMessage(ex.HResult, ex.Message);
            }
        }

        protected string NormalIP(string IP)
        {
            return (!String.IsNullOrWhiteSpace(IP) && IP.Split('.').Length == 4) ? IP : "localhost";
        }

        protected virtual bool UseParameters(Dictionary<string, string> dic)
        {
            try
            {
                IP = (dic.ContainsKey("ip")) ? dic["ip"] : (dic.ContainsKey("host")) ? dic["host"] : IP;
                port = (dic.ContainsKey("port")) ? int.Parse(dic["port"]) : 502;
                unitIdentifier = (dic.ContainsKey("id")) ? byte.Parse(dic["id"]) : (byte)1;
                timeout = (dic.ContainsKey("timeout")) ? int.Parse(dic["timeout"]) : (int)100;
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
