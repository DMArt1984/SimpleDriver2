using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connector.SGT;
using DML.Log;
using Hylasoft.Opc.Common;
using Hylasoft.Opc.Da;
using Hylasoft.Opc.Ua;
using LogCodeMessage;

namespace Connector.Driver
{
    class HylasoftOPCUAAdapter : DeviceNet
    {
        public const string driverName = "Hylasoft OPC UA Client";

        // Справка
        public static Dictionary<string, string> GetHelpSource()
        {
            return new Dictionary<string, string> {
                        { "Пример 1", "opc.tcp://10.0.130.240:4840"},
                        { "Пример 2", "opc.tcp://127.0.0.1:62547/DataAccessServer"},
                        { "Пример 3", "opc.tcp://127.0.0.1:49320"}
                    };
        } // Описание адреса устройства

        public static Dictionary<string, string> GetHelpTag()
        {
            return new Dictionary<string, string> {
                        { "Чтение данных 1", "Simulation Examples.Functions.Ramp1" }
                    };
        } // Описание адреса тега для данного устройства

        // UA client подключение
        private UaClient client;

        // Настройки клиента
        // ...

        public HylasoftOPCUAAdapter()
        {
            //disableHostForOpen = true;
            //...
        }

        ~HylasoftOPCUAAdapter()
        {
            try
            {
                client?.Dispose();
                client = null;
            } catch
            {

            }
        }

        // -----------------------------------------------------------------------------

        // Создание подключения
        public override CodeMessage CreateClient(string SrvURL)
        {
            try
            {
                client = new UaClient(new Uri(SrvURL));
                host = FindIP(SrvURL, host);
                port = FindPort(SrvURL, port);
                return new CodeMessage();
            }
            catch (Exception ex)
            {
                return CodeMessageFactory.FromException(ex);
            }
        }

        // ---------------------------------------------------------------------------------------------

        public override CodeMessage Connect(string SrvURL = "")
        {
            try
            {
                if (String.IsNullOrWhiteSpace(SrvURL) == false)
                    client = new UaClient(new Uri(SrvURL));

                host = FindIP(SrvURL, host);
                port = FindPort(SrvURL, port);
                client.Connect();
                IEnumerable<UaNode> UaNodes = client.ExploreFolder("");

                return client.Status == OpcStatus.Connected ? new CodeMessage(0,"") : new CodeMessage(-56, "Ошибка открытия");
            }
            catch (Exception ex)
            {
                return CodeMessageFactory.FromException(ex);
            }
        }

        //-------------------------------------------------------

        private string FindIP(string connectionString, string defaultIP = "")
        {
            string[] parts1 = connectionString.Split(new string[] { "tcp://" }, StringSplitOptions.None);
            if (parts1.Length < 2)
                return defaultIP;

            string[] parts2 = parts1[1].Split(':');
            if (parts2.Length < 2)
                return defaultIP;

            return parts2[0];
        }

        private int FindPort(string connectionString, int defaultPort = 502)
        {
            string[] parts1 = connectionString.Split(new string[] { ":" }, StringSplitOptions.None);
            if (parts1.Length != 3)
                return defaultPort;

            string[] parts2 = parts1[2].Split('/');

            return Convert.ToInt32(parts2[0]);
        }

        public override CodeMessage Disconnect()
        {
            try
            {
                if (client == null)
                    return CodeMessageFactory.FromEnumX(eSourceStatus.noClient);

                client.Dispose();
                return client.Status == OpcStatus.NotConnected ? new CodeMessage(0, "") : CodeMessageFactory.FromEnumX(eSourceStatus.errClose);
            }
            catch (Exception ex)
            {
                return CodeMessageFactory.FromException(ex);
            }
        }

        // ---------------------------------------------------------------------------------------------

        // Выполнение запроса
        public override TagResult GetValue(string address, eDataType DataType)
        {
            dynamic Value = null; // итоговое значение

            try
            {
                // ...
                switch (DataType)
                {
                    case eDataType.Bool:
                        Value = client.Read<bool>(address).Value;
                        break;
                    case eDataType.Byte:
                        Value = client.Read<byte>(address).Value;
                        break;
                    case eDataType.Short:
                    case eDataType.UShort:
                        Value = client.Read<short>(address).Value;
                        break;
                    case eDataType.Int:
                    case eDataType.UInt:
                        Value = client.Read<Int32>(address).Value;
                        break;
                    case eDataType.Float:
                        Value = client.Read<float>(address).Value;
                        break;
                    case eDataType.Long:
                        Value = client.Read<Int64>(address).Value;
                        break;
                    case eDataType.Double:
                        Value = client.Read<double>(address).Value;
                        break;
                    case eDataType.STRING:
                        Value = client.Read<string>(address).Value;
                        break;
                }

                // Вернуть тег
                return new TagResult(Value, eTagCode.good);
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Tag exeption");
                if (ex.HResult.ToString("X") == "80131500") // Error establishing a connection OR BadConnectionClosed
                {
                    log?.Invoke(new CodeMessage(ex.HResult, $"Ошибка связи с сервером: {ex.Message}"));
                    return new TagResult(0, (int)eTagCode.breakError, $"{eTagCode.breakError.GetText()} ={ex.HResult} {ex.Message}");
                }

                log?.Invoke(CodeMessageFactory.FromException(ex));
                return new TagResult(Value, ex.HResult, ex.Message);
            }

        }

        // Выполнение запроса если новое значение TRUE
        public override TagResult SetValue(string address, eDataType DataType, dynamic newValue = null)
        {
            if (newValue == null)
                return new TagResult(newValue, eTagCode.newValueIsNull);

            try {
                // ...
                switch (DataType)
                {
                    case eDataType.Bool:
                        client.Write<bool>(address, Convert.ToBoolean(newValue));
                        break;
                    case eDataType.Byte:
                        client.Write<byte>(address, Convert.ToByte(newValue));
                        break;
                    case eDataType.Short:
                    case eDataType.UShort:
                        client.Write<short>(address, Convert.ToInt16(newValue));
                        break;
                    case eDataType.Int:
                    case eDataType.UInt:
                        client.Write<Int32>(address, Convert.ToInt32(newValue));
                        break;
                    case eDataType.Float:
                        client.Write<float>(address, Convert.ToSingle(newValue));
                        break;
                    case eDataType.Long:
                        client.Write<Int64>(address, Convert.ToInt64(newValue));
                        break;
                    case eDataType.Double:
                        client.Write<double>(address, Convert.ToDouble(newValue));
                        break;
                    case eDataType.STRING:
                        client.Write<string>(address, Convert.ToString(newValue));
                        break;
                }

                return new TagResult(newValue, eTagCode.good);
            }
            catch (Exception ex)
            {
                if (ex.HResult.ToString("X") == "80131500") // Error establishing a connection OR BadConnectionClosed
                {
                    log?.Invoke(new CodeMessage(ex.HResult, $"Ошибка связи с сервером: {ex.Message}"));
                    return new TagResult(0, (int)eTagCode.breakError, $"{eTagCode.breakError.GetText()} ={ex.HResult} {ex.Message}");
                }

                log?.Invoke(CodeMessageFactory.FromException(ex));
                return new TagResult(newValue, ex.HResult, ex.Message);
            }
        }


    }
}
