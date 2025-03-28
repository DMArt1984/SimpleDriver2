using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinSimpleIDriver.Connector.SGT;
using DML.Log;

namespace WinSimpleIDriver.Connector.Driver
{
    class AppUDP: DeviceNet
    {
        public const string driverName = "App UDP";

        // Справка
        public static Dictionary<string, string> GetHelpSource()
        {
            return new Dictionary<string, string> {
                        { "Пример", "ip=127.0.0.1;remotePort=8002;localPort=8001;timeout=500;wait=500" },
                    };
        } // Описание адреса устройства

        public static Dictionary<string, string> GetHelpTag()
        {
            return new Dictionary<string, string> {
                        { "Адрес", "Так же как и для AppDevice" },
                    };
        } // Описание адреса тега для данного устройства

        public enum UDPcommand
        {
            None,
            Run,
            Stop,
            Restart
        }

        // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        // Клиент
        private int remotePort = 8002; // порт для отправки данных
        private int localPort = 8001; // локальный порт для прослушивания входящих подключений

        private UDPcommand cmd = UDPcommand.None;
        private Thread receiveThread;
        private bool threadExit = false;

        //
        private int wait = 1000;

        // События
        private delegate void AccountHandler(string message);
        private event AccountHandler Notify_Receive;

        // Полученные данные и время их получения
        private Dictionary<string, dynamic> recData = new Dictionary<string, dynamic>();
        private Dictionary<string, DateTime> recTime = new Dictionary<string, DateTime>();

        // Синхронизация потоков
        private object sync = new object();

        // Лог
        public override bool supportTLog { get; } = true;

        // ===================================================================================================

        //public AppUDP(string Host, int remotePort = 8002, int localPort = 8001, int timeout = 100, int wait = 1000)
        //{
        //    this.IP = Host;
        //    this.remotePort = remotePort;
        //    this.localPort = localPort;
        //    this.timeout = timeout;
        //    this.wait = wait;
        //    CreateClient();
        //}

        public AppUDP(string parameters)
        {
            receiveThread = new Thread(new ThreadStart(ReceiveMessage));
            receiveThread.IsBackground = true;
            receiveThread.Start();
            //...
            //CreateClient(parameters);
        }

        ~AppUDP()
        {
            try
            {
                threadExit = true;
                receiveThread?.Abort();
            }
            catch
            {

            }
        }

        // -------------------------------------------------------------------------------------------

        // Создание клиента
        public override CodeMessage CreateClient(string parameters = "")
        {
            disableHostForOpen = true;

            try
            {
                if (String.IsNullOrWhiteSpace(parameters) == false)
                    UseParameters(ParamsToDic(parameters));

                //receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                //receiveThread.IsBackground = true;
                //receiveThread.Start();

                //cmd = UDPcommand.Restart;

                return new CodeMessage();
            }
            catch (Exception ex)
            {
                return CodeMessageFactory.FromException(ex);
            }
        }

        protected override bool UseParameters(Dictionary<string, string> dic)
        {
            try
            {
                host = (dic.ContainsKey("ip")) ? NormalizeIP(dic["ip"]) : host; // если IP еще не был получен
                remotePort = (dic.ContainsKey("remotePort")) ? int.Parse(dic["remotePort"]) : remotePort;
                localPort = (dic.ContainsKey("localPort")) ? int.Parse(dic["localPort"]) : localPort;
                port = remotePort; // для проверки соединения (IsPing, IsHost)
                timeout = (dic.ContainsKey("timeout")) ? int.Parse(dic["timeout"]) : (int)100;
                wait = (dic.ContainsKey("wait")) ? int.Parse(dic["wait"]) : (int)1000;
                return true;
            }
            catch
            {
                return false;
            }
        }



        // ---------------------------------------------------------------------------------------------

        public override CodeMessage Connect(string parameters)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(parameters) == false)
                    UseParameters(ParamsToDic(parameters));

                if (cmd == UDPcommand.Run)
                {
                    cmd = UDPcommand.Restart;
                } else
                {
                    cmd = UDPcommand.Run;
                }
                return new CodeMessage();
            }
            catch (Exception ex)
            {
                return CodeMessageFactory.FromException(ex);
            }
        }

        public override CodeMessage Disconnect()
        {
            try
            {
                cmd = UDPcommand.Stop;
                return new CodeMessage();
            }
            catch (Exception ex)
            {
                return CodeMessageFactory.FromException(ex);
            }
        }

        // ---------------------------------------------------------------------------------------------

        // Получить значение тега (ждать ответ)
        public override TagResult GetValue(string command, eDataType dataType)
        {
            InnerTrafficLog($"GetValue = {command}");
            dynamic Value = null; // итоговое значение

            try
            {
                // добавление уникального кода сообщения
                var hash = Convert.ToString(command.GetHashCode());
                command = "C" + hash + (char)13 + dataType.ToString() + (char)13 + command;

                InnerTrafficLog($" ... {command}");

                // отправка запроса
                var exeption = SendMessage(command);
                if (exeption == null)
                {
                    // поиск ответа и проверка ожидания
                    if (recData.ContainsKey(hash))
                    {
                        Value = recData[hash]; // нашел!
                        InnerTrafficLog($" Result = {Value}");
 
                    } else
                    {
                        return new TagResult(Value, eTagCode.noData);
                        //if (recTime.ContainsKey(hash) == false)
                        //{
                        //    recTime.Add(hash, DateTime.Now);
                        //}

                    }

                    // Контроль времени
                    if (recTime.ContainsKey(hash))
                    {
                        TimeSpan ts = DateTime.Now.Subtract(recTime[hash]);
                        if (ts.TotalMilliseconds <= wait)
                        {
                            InnerTrafficLog($" {ts.TotalMilliseconds} мсек");
                        }
                        else
                        {
                            InnerTrafficLog($" Error: {ts.TotalMilliseconds} мсек > Limit; last result = {Value}");
                            InnerTrafficLog($"  now = {DateTime.Now}; last = {recTime[hash]}");
                            return new TagResult(Value, eTagCode.tagTimeout);
                        }
                    }

                } else
                {
                    InnerTrafficLog($" Exeption = {exeption.HResult} {exeption.Message}");
                    return new TagResult(Value, exeption.HResult, exeption.Message);
                }

                // -------
                Value = Tag.ConvertValueWithArray(Value, dataType);

                // -------
                return new TagResult(Value, eTagCode.good);
            }
            catch (Exception ex)
            {
                return new TagResult(Value, ex.HResult, ex.Message);
            }

        }


        // ---------------------------------------------------------------------------------------------

        // Прием сообщений
        private async void ReceiveMessage()
        {
            try
            {
                // Ожидание параметров клиента
                do
                {
                    await Task.Delay(10);
                } while (cmd == UDPcommand.None);
                
                // Установка клиента
                UdpClient receiver = new UdpClient(localPort); // UdpClient для получения данных
                IPEndPoint remoteIp = null; // адрес входящего подключения

                do
                {
                    if (cmd == UDPcommand.Run)
                    {

                        try
                        {
                            while (cmd == UDPcommand.Run)
                            {
                                lock (sync)
                                {
                                    byte[] data = receiver.Receive(ref remoteIp); // получаем данные
                                    string message = Encoding.Unicode.GetString(data);

                                    Notify_Receive?.Invoke(message); // событие...

                                    InnerTrafficLog($"receive: {message}");

                                    // Разбива сообщения на части
                                    string[] partsMessages = message.Split((char)13);

                                    if (partsMessages.Length == 3)
                                    {
                                        var key = partsMessages[0];
                                        var strDataType = partsMessages[1];
                                        var content = partsMessages[2];

                                        if (key.Length > 1 && strDataType.Length > 1)
                                        {
                                            bool cmd = (key[0] == 'C'); // команда
                                            bool answer = (key[0] == 'A'); // ответ
                                            key = key.Substring(1);

                                            eDataType datatType = (eDataType)System.Enum.Parse(typeof(eDataType), strDataType, true);

                                            InnerTrafficLog($" cmd = {cmd}; answer = {answer}; key = {key}; dataType = {datatType}; content = {content}");

                                            if (cmd)
                                            {
                                                // выполнение команды
                                                var tagResult = AppDevice.StaticGetValue(content, datatType);
                                                var strValue = tagResult.value;
                                                if (tagResult.value is IEnumerable && tagResult.value.GetType() != typeof(string))
                                                {
                                                    strValue = String.Join($"{(char)9}", tagResult.value);
                                                }

                                                // добавление уникального кода к ответу
                                                var hash = Convert.ToString(content.GetHashCode());
                                                content = "A" + hash + (char)13 + strDataType + (char)13 + strValue;

                                                InnerTrafficLog($" CMD: result = {tagResult.value}; code = {tagResult.codeMessage}; content = {content}");

                                                // отправка ответа клиенту
                                                var exeption = SendMessage(content); //SendMessage(content, remoteIp.Address.ToString());
                                            }
                                            else if (answer)
                                            {
                                                dynamic value = content;
                                                string[] arr = content.Split((char)9);
                                                if (arr.Length >= 2)
                                                    value = arr;

                                                // размещение данных в словаре
                                                if (recData.ContainsKey(key))
                                                {
                                                    recData[key] = value;
                                                    recTime[key] = DateTime.Now;
                                                    InnerTrafficLog($" ANSWER: change");
                                                }
                                                else
                                                {
                                                    recData.Add(key, value);
                                                    recTime.Add(key, DateTime.Now);
                                                    InnerTrafficLog($" ANSWER: add");
                                                }
                                            }
                                        }
                                    }


                                }

                            }

                        }
                        catch (Exception ex)
                        {
                            //situation = new CodeMessage(ex.HResult, ex.Message);
                        }

                    }

                    // RESTART...
                    if (cmd == UDPcommand.Restart)
                    {
                        receiver = new UdpClient(localPort); // UdpClient для получения данных
                        cmd = UDPcommand.Run;
                    }

                    await Task.Delay(10);

                } while (threadExit == false);

                receiver.Close();

            } catch (Exception ex)
            {
                log?.Invoke(CodeMessageFactory.FromException(ex, $"new UdpClient({localPort}): #"));
            }
        }

        // Отправка сообщения
        public Exception SendMessage(string message, string targetIP = "")
        {
            Exception myEx = null;
            if (cmd == UDPcommand.Run)
            {
                UdpClient sender = new UdpClient(); // создаем UdpClient для отправки сообщений
                try
                {
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    if (String.IsNullOrWhiteSpace(targetIP))
                    {
                        sender.Send(data, data.Length, host, remotePort); // отправка
                    } else
                    {
                        sender.Send(data, data.Length, targetIP, remotePort); // отправка
                    }
                }
                catch (Exception ex)
                {
                    myEx = ex;
                }
                finally
                {
                    sender.Close();
                }
                return myEx;
            }
            else
            {
                return new Exception("STOP");
            }
        }


    }
}
