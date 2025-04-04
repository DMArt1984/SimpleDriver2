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
using LogCodeMessage;

namespace Connector.Driver
{
    class AppUDP: DeviceNet
    {
        public const string driverName = "App UDP";

        // Справка
        // Описание адреса устройства
        public static Dictionary<string, string> HelpSource
            => new Dictionary<string, string> {
                        { "Пример", "ip=127.0.0.1;remotePort=8002;localPort=8001;timeout=500;wait=500" },
                    };
        // Описание адреса тега для данного устройства
        public static Dictionary<string, string> HelpTag
            => new Dictionary<string, string> {
                        { "Пример", "ip=127.0.0.1;remotePort=8002;localPort=8001;timeout=500;wait=500" },
                    };

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

        //
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private Task _receiveTask;

        // Лог
        public override bool SupportTLog { get; } = true;

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
            // Запускаем асинхронный прием сообщений
            _receiveTask = ReceiveMessagesAsync(_cts.Token);
            // Инициализация клиента или вызов CreateClient(parameters) при необходимости...
        }

        public override void Dispose()
        {
            try
            {
                _cts.Cancel();
                _receiveTask?.Wait(); // можно дождаться завершения или использовать await в асинхронном Dispose
            }
            catch { }
            base.Dispose();
        }

        // -------------------------------------------------------------------------------------------

        // Создание клиента
        public override CodeMessage CreateClient(string parameters = "")
        {
            disableHostForOpen = true;

            try
            {
                if (String.IsNullOrWhiteSpace(parameters) == false)
                    UseNetParameters(ParamsToDic(parameters));

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

        protected override bool UseNetParameters(Dictionary<string, string> dic)
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
                    UseNetParameters(ParamsToDic(parameters));

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
                        return new TagResult(Value, Tag.CM.NoData);
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
                            return new TagResult(Value, Tag.CM.TagTimeout);
                        }
                    }

                } else
                {
                    InnerTrafficLog($" Exeption = {exeption.HResult} {exeption.Message}");
                    return new TagResult(Value, exeption);
                }

                // -------
                Value = TagLib.ConvertValueWithArray(Value, dataType);

                // -------
                return new TagResult(Value);
            }
            catch (Exception ex)
            {
                return new TagResult(Value, ex);
            }

        }


        // ---------------------------------------------------------------------------------------------

        // Прием сообщений
        private async Task ReceiveMessagesAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Открываем UdpClient для приема сообщений
                using (UdpClient receiver = new UdpClient(localPort))
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            UdpReceiveResult result = await receiver.ReceiveAsync().ConfigureAwait(false);
                            string message = Encoding.Unicode.GetString(result.Buffer);

                            // Обработка сообщения с синхронизацией
                            lock (sync)
                            {
                                // Разбиваем сообщение на части и обрабатываем его
                                string[] partsMessages = message.Split((char)13);
                                if (partsMessages.Length == 3)
                                {
                                    var key = partsMessages[0];
                                    var strDataType = partsMessages[1];
                                    var content = partsMessages[2];

                                    if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(strDataType))
                                    {
                                        bool cmdFlag = key[0] == 'C'; // команда
                                        bool answerFlag = key[0] == 'A'; // ответ
                                        key = key.Substring(1);

                                        eDataType dataType = (eDataType)Enum.Parse(typeof(eDataType), strDataType, true);

                                        InnerTrafficLog($"cmd = {cmdFlag}; answer = {answerFlag}; key = {key}; dataType = {dataType}; content = {content}");

                                        if (cmdFlag)
                                        {
                                            TagResult tagResult = AppDevice.StaticGetValue(content, dataType);
                                            var strValue = tagResult.value;
                                            if (tagResult.value is IEnumerable && tagResult.value.GetType() != typeof(string))
                                            {
                                                strValue = String.Join("~", tagResult.value);
                                            }
                                            // Формируем ответ
                                            var hash = Convert.ToString(content.GetHashCode());
                                            content = "A" + hash + (char)13 + strDataType + (char)13 + strValue;

                                            InnerTrafficLog($"CMD: result = {tagResult.value}; code = {tagResult.codeMessage}; content = {content}");

                                            // Отправка ответа клиенту
                                            SendMessage(content);
                                        }
                                        else if (answerFlag)
                                        {
                                            dynamic value = content;
                                            string[] arr = content.Split((char)9);
                                            if (arr.Length >= 2)
                                                value = arr;

                                            // Обновляем словари ответов
                                            if (recData.ContainsKey(key))
                                            {
                                                recData[key] = value;
                                                recTime[key] = DateTime.Now;
                                                InnerTrafficLog("ANSWER: change");
                                            }
                                            else
                                            {
                                                recData.Add(key, value);
                                                recTime.Add(key, DateTime.Now);
                                                InnerTrafficLog("ANSWER: add");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (SocketException sex)
                        {
                            // Обработка ошибок сокета – если ошибка связана с отменой, то выходим из цикла
                            if (cancellationToken.IsCancellationRequested)
                                break;
                            InnerTrafficLog($"Socket error: {sex.Message}");
                        }
                        catch (Exception ex)
                        {
                            InnerTrafficLog($"Ошибка приема сообщения: {ex.Message}");
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Ожидаемое исключение при отмене
            }
            catch (Exception ex)
            {
                log?.Invoke(CodeMessageFactory.FromException(ex, $"ReceiveMessagesAsync error"));
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
