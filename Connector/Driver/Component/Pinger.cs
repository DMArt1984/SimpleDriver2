using DML.Log;
using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace WinSimpleIDriver.Connector.Driver.Component
{
    class Pinger
    {
        // Проверка общей доступности хоста
        //
        // Проверяет доступность хоста на уровне ICMP(сетевого уровня).
        // Быстрая проверка: работает даже без открытого порта.
        // Не требует "слушающего" сервера на удалённой стороне.
        // Полезно для:
        //  - базовой диагностики сети;
        //  - определения "жив ли хост вообще";
        //  - выявления проблем маршрутизации.
        public static bool IsHostReachable(string nameOrAddress, int timeout = 50)
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

        // Проверка нужного сервиса
        //
        // Проверяет, открыт ли конкретный порт(например, 502 для Modbus TCP).
        // Использует TCP-соединение.
        // Выявляет не только доступность хоста, но и то, что служба(сервер) работает.
        public static CodeMessage TryTcpConnect(string hostUri, int portNumber, int timeout = 50)
        {
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
                return new CodeMessage(ex.HResult, $"Unexpected error: {ex.Message}");
            }
            finally
            {
                if (client != null)
                {
                    client.Close(); // Закрываем соединение, даже если подключение не удалось
                }
            }
        }

    }
}
