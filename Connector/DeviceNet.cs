using DML.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinSimpleIDriver.Connector.Driver.Component;
using WinSimpleIDriver.Connector.SGT;

namespace WinSimpleIDriver.Connector
{
    public interface INetDevice
    {
        bool IsHostReachable(string hostUri, int timeout);
        CodeMessage TryTcpConnect(string hostUri, int portNumber, int timeoutMs);
    }

    class DeviceNet : Device, INetDevice
    {
        const string DEFAULTHOST = "localhost";

        protected int timeout = 100; // время ожидания ответа

        protected string host = DEFAULTHOST; // host/ip

        protected int port = 502;

        protected byte unitIdentifier = 1;

        public bool disableHostForOpen = false;

        // Проверка общей доступности хоста
        public virtual bool IsHostReachable(string ip = "", int timeout = 0)
        {
            if (string.IsNullOrWhiteSpace(ip))
                ip = this.host;

            if (timeout <= 0)
                timeout = this.timeout;

            var result = Pinger.IsHostReachable(ip, timeout);
            return result;
        }

        // Проверка нужного сервиса
        //
        // Проверяет, открыт ли конкретный порт(например, 502 для Modbus TCP).
        // Использует TCP-соединение.
        // Выявляет не только доступность хоста, но и то, что служба(сервер) работает.
        public CodeMessage TryTcpConnect(string host = "", int port = 0, int timeout = 0)
        {
            if (string.IsNullOrWhiteSpace(host))
                host = this.host;

            if (timeout <= 0)
                timeout = this.timeout;

            if (port <= 0)
                port = this.port;

            var result = Pinger.TryTcpConnect(host, port, timeout);
            return result;
        }

        // Нормализация IP
        protected string NormalizeIP(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
                return DEFAULTHOST;

            // Простая проверка: IPv4 состоит из 4 чисел от 0 до 255
            var parts = ip.Split('.');
            if (parts.Length == 4 && parts.All(p => byte.TryParse(p, out _)))
                return ip;

            return DEFAULTHOST;
        }

        // Получение произвольных параметров
        protected virtual bool UseParameters(Dictionary<string, string> dic)
        {
            if (dic == null)
                return false;

            try
            {
                if (dic.TryGetValue("ip", out string ip) || dic.TryGetValue("host", out ip))
                    host = ip;

                if (dic.TryGetValue("port", out string portStr) && int.TryParse(portStr, out int parsedPort))
                    port = parsedPort;
                else
                    port = 502;

                if (dic.TryGetValue("id", out string idStr) && byte.TryParse(idStr, out byte parsedId))
                    unitIdentifier = parsedId;
                else
                    unitIdentifier = 1;

                if (dic.TryGetValue("timeout", out string timeoutStr) && int.TryParse(timeoutStr, out int parsedTimeout))
                    timeout = parsedTimeout;
                else
                    timeout = 100;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
