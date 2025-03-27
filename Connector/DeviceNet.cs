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
        const string DefaultHost = "localhost";

        protected int timeout = 100; // время ожидания ответа

        protected string IP = "localhost"; // host

        protected int port = 502;

        protected byte unitIdentifier = 1;

        public bool disableHostForOpen = false;

        // Проверка общей доступности хоста
        //
        // Проверяет доступность хоста на уровне ICMP(сетевого уровня).
        // Быстрая проверка: работает даже без открытого порта.
        // Не требует "слушающего" сервера на удалённой стороне.
        // Полезно для:
        //  - базовой диагностики сети;
        //  - определения "жив ли хост вообще";
        //  - выявления проблем маршрутизации.
        public virtual bool IsHostReachable(string IP = "", int timeout = 0)
        {
            bool ping = (String.IsNullOrWhiteSpace(IP)) ? Pinger.PingHost(this.IP, (timeout > 0) ? timeout : this.timeout) : Pinger.PingHost(IP, (timeout > 0) ? timeout : this.timeout);
            return ping;
        }

        // Проверка нужного сервиса
        //
        // Проверяет, открыт ли конкретный порт(например, 502 для Modbus TCP).
        // Использует TCP-соединение.
        // Выявляет не только доступность хоста, но и то, что служба(сервер) работает.
        public CodeMessage TryTcpConnect(string hostUri = "", int portNumber = 0, int timeout = 3000)
        {
            if (string.IsNullOrWhiteSpace(hostUri))
                hostUri = this.IP;

            if (portNumber <= 0)
                portNumber = this.port;

            TcpClient client = null;

            try
            {
                client = new TcpClient();

                var result = client.BeginConnect(hostUri, portNumber, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(timeout));

                if (!success)
                {
                    return new CodeMessage(-1, $"Connection timeout after {timeout} ms");
                }

                client.EndConnect(result);
                return new CodeMessage(0, "Connected successfully");
            }
            catch (SocketException ex)
            {
                return new CodeMessage(ex.HResult, $"Socket error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return new CodeMessage(-2, $"Unexpected error: {ex.Message}");
            }
            finally
            {
                if (client != null)
                {
                    client.Close(); // Закрываем соединение, даже если подключение не удалось
                }
            }
        }

        protected string NormalizeIP(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
                return DefaultHost;

            // Простая проверка: IPv4 состоит из 4 чисел от 0 до 255
            var parts = ip.Split('.');
            if (parts.Length == 4 && parts.All(p => byte.TryParse(p, out _)))
                return ip;

            return DefaultHost;
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
