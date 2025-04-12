using Connector.Driver.Component;

using LogCodeMessage;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connector.Driver
{
    class ModbusRTUAdapter : DeviceReal
    {
        public const string driverName = "Modbus RTU Client";

        // Справка
        // Описание адреса устройства
        public static Dictionary<string, string> HelpSource
            => new Dictionary<string, string> {
                       { "Пример", "port=COM1;baudrate=9600;parity=None;dataBits=8;stopBits=2;timeout=500;unit=1;fails=10" },
                        { "port", "номер порта (COM1)" },
                        { "baudrate", "Скорость (300, 9600, 19200, 115200 и др.)" },
                        { "parity", "Контроль четности (None, Odd, Even, Mark, Space)" },
                        { "dataBits", "Биты данных (7, 8)" },
                        { "stopBits", "Стоповые биты (1, 2)" },
                        { "unit", "адрес/ID устройства (1...255)" },
                        { "timeout", "время (мсек) ожидания ответа (500)" },
                        { "fails", "количество ошибочных запросов перед отключением (переподключением) драйвера (=10)" }
                    };
        // Описание адреса тега для данного устройства
        public static Dictionary<string, string> HelpTag
            => new Dictionary<string, string> {
                        { "Адрес Holding Register", "HR-3-1 = 40003 Order HighLow (read-write)" },
                        { "Адрес Input Register", "IR-1-0 = 30001 Order LowHigh (read)" },
                        { "Адрес Coil Status", "CO-10 = 00010 (read-write)" },
                        { "Адрес Input Status", "IN-7 = 10007 (read)" },
                        { "ID устройства в адресе", "10:IN-7 = адрес 10007 и ID устр. 10" },
                        { "Пример №1 четыре регистра начиная с первого", "HR-1>4" },
                        { "Пример №2", "HR-8" },
                        { "Пример №3", "40003" },
                        { "Пример №4", "1:40003" }
                    };
        
        // Клиент
        public ModbusRTUmaster client;
        private string portName = "COM1";      // имя порта для подключения
        private int baudrate = 9600;    // скорость передачи
        private Parity parity = Parity.None;   // контроль четности
        private int dataBits = 8;   //  кол-во бит данных
        private StopBits stopBits = StopBits.One;  //кол-во стоп битов
        private byte unitCOMidentifier = 1; // Not necessary since default slaveID = 1;
        private static int _timeout = 500;

        // Настройки клиента
        public int baseAddress = 0; // 0 или 1
        ushort IdTrans = 1; // идентификатор сообщения

        public override bool SupportTLog { get; } = true;
        
        public ModbusRTUAdapter()
        {
            //...
        }

        ~ModbusRTUAdapter()
        {
            try
            {
                client?.Disconnect();
                client = null;
            } catch
            {

            }
        }

        // -----------------------------------------------------------------------------

        // Создание клиента
        public override CodeMessage CreateClient(string parameters = "")
        {
            try
            {
                if (String.IsNullOrWhiteSpace(parameters) == false)
                    UseParameters(ParamsToDic(parameters));

                client = new ModbusRTUmaster();
                client.log = InnerTrafficLog;
                return new CodeMessage();
            }
            catch (Exception ex)
            {
                return CodeMessageFactory.FromException(ex);
            }
        }

        // ---------------------------------------------------------------------------------------------

        public override CodeMessage Connect(string parameters)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(parameters) == false)
                    UseParameters(ParamsToDic(parameters));

                if (client == null)
                    return CodeMessageFactory.FromEnumX(eSQLStatus.noClient);
                client.Connect(portName, baudrate, parity, dataBits, stopBits, unitCOMidentifier, _timeout);
                return client.connected ? new CodeMessage(0,"") : CodeMessageFactory.FromEnumX(eSQLStatus.errOpen);
            }
            catch (Exception ex)
            {
                return CodeMessageFactory.FromException(ex);
            }
        }

        // Разбор строки подключения
        public bool UseParameters(Dictionary<string, string> dic)
        {
            try
            {
                unitCOMidentifier = (dic.ContainsKey("unit")) ? byte.Parse(dic["unit"]) : (dic.ContainsKey("id")) ? byte.Parse(dic["id"]) : (byte)1;
                portName = (dic.ContainsKey("port")) ? dic["port"] : (dic.ContainsKey("com")) ? "COM"+dic["com"] : "COM1";
                baudrate = (dic.ContainsKey("baudrate")) ? int.Parse(dic["baudrate"]) : 9600;
                parity = (dic.ContainsKey("parity")) ? (System.IO.Ports.Parity)Enum.Parse(typeof(System.IO.Ports.Parity), dic["parity"]) : System.IO.Ports.Parity.None;
                dataBits = (dic.ContainsKey("dataBits")) ? int.Parse(dic["dataBits"]) : 8;
                stopBits = (dic.ContainsKey("stopBits")) ? (System.IO.Ports.StopBits)int.Parse(dic["stopBits"]) : System.IO.Ports.StopBits.Two;
                _timeout = (dic.ContainsKey("timeout")) ? int.Parse(dic["timeout"]) : 500;

                return true;
            }
            catch
            {
                return false;
            }

        }

        // -----------------------------------------------------------------------------------------------

        public override CodeMessage Disconnect()
        {
            try
            {
                if (client == null)
                    return CodeMessageFactory.FromEnumX(eSQLStatus.noClient);
                client.Disconnect();
                return client.connected == false ? new CodeMessage() : CodeMessageFactory.FromEnumX(eSQLStatus.errClose);
            }
            catch (Exception ex)
            {
                return CodeMessageFactory.FromException(ex);
            }
        }

        // ---------------------------------------------------------------------------------------------

        // Разбор строки адреса
        private void ParsingAddress(string address, out string addrArea, out int addrStart, out int addrOrder, out int countValues, out byte unit)
        {
            // HR-3-1 = Holding Register 40003 Order HighLow (read-write),
            // IR-1-0 = Input Register 30001 Order LowHigh,
            // CO-10 = Coil Status 00010 (read-write),
            // IN-7 = Input Status 10007

            // HR-3-1>2 = Two values
            // 40003>5 = Five values

            unit = 0;

            string[] unitAddr = address.Split(':');
            if (unitAddr.Length == 2)
            {
                unit = byte.Parse(unitAddr[0]);
                address = unitAddr[1];
            }

            string[] addr = address.Split('>');
            countValues = (addr.Length >= 2) ? int.Parse(addr[1]) : 1; // количество значений
            if (countValues < 1)
                countValues = 1;

            address = addr[0];
            if (address.Contains("-"))
            {
                string[] CN = address.Split('-');
                addrArea = CN.Length >= 1 ? CN[0].ToUpper() : ""; // тип области данных
                addrStart = CN.Length >= 2 ? int.Parse(CN[1]) : 0; // начальный адрес
                addrOrder = (CN.Length >= 3) ? int.Parse(CN[2]) : 0; // порядок
            }
            else
            {
                long Register = long.Parse(address);
                if (Register > 40000)
                {
                    addrArea = "HR";
                    addrStart = (int)(Register - 1 - 40000);
                }
                else if (Register > 30000)
                {
                    addrArea = "IR";
                    addrStart = (int)(Register - 1 - 30000);
                }
                else if (Register > 10000)
                {
                    addrArea = "IN";
                    addrStart = (int)(Register - 1 - 10000);
                }
                else if (Register > 0)
                {
                    addrArea = "CO";
                    addrStart = (int)(Register - 1);
                }
                else
                {
                    addrArea = "X";
                    addrStart = 0;
                }
                addrOrder = 0;
            }

            addrStart += baseAddress; // plc or protocol base
        }

        // ---------------------------------------------------------------------------------------------

        // Получить значение тега
        public override TagResult GetValue(string address, eDataType DataType)
        {
            // Открыто ли подключение
            //if (IsConnected() == false)
            //    return new TagResult(0, (int)eTagCode.sourceDisconnect, const_SourceDisconnect);

            dynamic Value = null; // итоговое значение

            try
            {
                ParsingAddress(address, out string addrArea, out int addrStart, out int addrOrder, out int count, out byte unit);

                if (unit == 0)
                    unit = unitCOMidentifier;

                //Console.WriteLine($"GetValue(address={address}) -> count={count}");

                    // количество регистров в значении
                ushort RegsInValue = 1;
                switch (DataType)
                {
                    case eDataType.Short:
                    case eDataType.UShort:
                        RegsInValue = 1;
                        break;
                    case eDataType.Int:
                    case eDataType.Float:
                        RegsInValue = 2;
                        break;
                    case eDataType.Double:
                        RegsInValue = 4;
                        break;
                }

                // ответ
                byte[] bytes = new byte[] { };

                // ID сообщения
                IdTrans++;
                if (IdTrans >= ushort.MaxValue)
                    IdTrans = 1;

                int MBit = 1;
                int MX = 2;

                // Получение регистров (в байтах)
                switch (addrArea)
                {
                    case "CO":
                        client.ReadCoils(IdTrans, unit, (ushort)addrStart, (ushort)count, ref bytes);
                        MBit = 8;
                        MX = 1;
                        break;
                    case "IN":
                        client.ReadDiscreteInputs(IdTrans, unit, (ushort)addrStart, (ushort)count, ref bytes);
                        MBit = 8;
                        MX = 1;
                        break;
                    case "HR":
                        client.ReadHoldingRegister(IdTrans, unit, (ushort)addrStart, (ushort)(count * RegsInValue), ref bytes);
                        break;
                    case "IR":
                        client.ReadInputRegister(IdTrans, unit, (ushort)addrStart, (ushort)(count * RegsInValue), ref bytes);
                        break;
                }

                // Есть ли ошибки в ответе?
                if (client.statusLastAnswer != 0)
                {
                    if (client.statusLastAnswer == ModbusRTUmaster.excExceptionConnectionLost) // если ошибка связи (порта)
                    {
                        log?.Invoke(new CodeMessage(-1, $"ModbusRTU.excExceptionConnectionLost for {address}"));
                        return new TagResult(0, TAG.CM.BreakError.code, $"{client.statusLastAnswer}");
                    }
                    return new TagResult(0, -client.statusLastAnswer, ModbusRTUmaster.exc[client.statusLastAnswer]);
                }

                if (bytes == null || bytes.Any() == false)
                {
                    return new TagResult(0, TAG.CM.NoData);
                }

                //Console.WriteLine("next...");

                // Проверка на полноту данных
                switch (DataType)
                {
                    case eDataType.Short:
                    case eDataType.UShort:
                    case eDataType.Float:
                    case eDataType.Int:
                    case eDataType.UInt:
                    case eDataType.Double:
                        if (bytes.Length*MBit != count * RegsInValue * MX) // <
                        {
                            log?.Invoke(new CodeMessage(1, $"Проверка на тип данных: (bytes.Length = {bytes.Length}) != (count * RegsInValue * 2 = {count * RegsInValue * 2})"));
                            return new TagResult(0, TAG.CM.Inconsistency);
                        }
                        break;
                }

                // Получение значений из регистров
                // addrOrder = Desired Word Order (Low Register first or High Register first)
                switch (DataType)
                {
                    case eDataType.Bool:
                        Value = ModbusRTUmaster.GetBoolFromBytes(bytes, (ushort)count);
                        break;

                    case eDataType.Binary:
                        Value = ModbusRTUmaster.GetBinaryFromBytes(bytes);
                        break;

                    case eDataType.Byte:
                        Value = bytes;
                        break;

                    case eDataType.Short:
                        Value = ModbusRTUmaster.GetInt16FromBytes(bytes);
                        break;

                    case eDataType.UShort:
                        Value = ModbusRTUmaster.GetUInt16FromBytes(bytes);
                        break;

                    case eDataType.Float:
                        if (addrOrder == 0)
                        {
                            Value = ModbusRTUmaster.GetFloatFromBytes(bytes);
                        }
                        else
                        {
                            Value = ModbusRTUmaster.GetFloatInverseFromBytes(bytes);
                        }
                        break;

                    case eDataType.Int:
                    case eDataType.UInt:
                        if (addrOrder == 0)
                        {
                            Value = ModbusRTUmaster.GetInt32FromBytes(bytes);
                        }
                        else
                        {
                            Value = ModbusRTUmaster.GetInt32InverseFromBytes(bytes);
                        }
                        break;

                    case eDataType.Double:
                        if (addrOrder == 0)
                        {
                            Value = ModbusRTUmaster.GetDoubleFromBytes(bytes);
                        }
                        else
                        {
                            Value = ModbusRTUmaster.GetDoubleInverseFromBytes(bytes);
                        }
                        break;

                    default:
                        return new TagResult(0, TAG.CM.NotSupport);
                }

                // массив и одиночное значение
                OneArrayToValue(ref Value);

                // Вернуть тег
                return new TagResult(Value);
            }
            catch (Exception ex)
            {
                return new TagResult(Value, ex);
            }

        }

        // Записать значение тега
        public override TagResult SetValue(string address, eDataType DataType, dynamic newValue = null)
        {
            // Открыто ли подключение
            //if (IsConnected() == false)
            //    return new TagResult(newValue, (int)eTagCode.sourceDisconnect, const_SourceDisconnect);

            if (newValue == null)
                return new TagResult(newValue, TAG.CM.NewValueIsNull);

            string[] newValues = DynamicToStringArray(newValue);

            try
            {
                ParsingAddress(address, out string addrArea, out int addrStart, out int addrOrder, out int count, out byte unit);

                if (unit == 0)
                    unit = unitCOMidentifier;

                for (int i = 0; i < newValues.Length; i++)
                    newValues[i] = (newValues[i].ToUpper() == "TRUE") ? "1" : ((newValues[i].ToUpper() == "FALSE") ? "0" : newValues[i]);

                // ID сообщения
                IdTrans++;
                if (IdTrans >= ushort.MaxValue)
                    IdTrans = 1;

                byte[] result = new byte[] { };

                switch (addrArea)
                {
                    case "CO":
                        switch (DataType)
                        {
                            case eDataType.Bool:
                                {
                                    bool[] bools = newValues.Select(x => StringToBool(x)).ToArray();
                                    if (count > bools.Length)
                                        count = bools.Length;

                                    if (bools.Length == 1 || count == 1)
                                    {
                                        client.WriteSingleCoils(IdTrans, unit, (ushort)addrStart, bools[0], ref result);
                                    }
                                    else
                                    {
                                        byte[] bytes = ModbusRTUmaster.ValToBytes(bools);
                                        client.WriteMultipleCoils(IdTrans, unit, (ushort)addrStart, (ushort)count, bytes, ref result);
                                    }
                                }
                                break;

                            case eDataType.Binary:
                                {
                                    List<bool> bl = new List<bool>();
                                    foreach (var item in newValues)
                                        bl.AddRange(item.Select(x => (x == '1') ? true : false));

                                    bool[] bools = bl.ToArray();
                                    if (count > bools.Length)
                                        count = bools.Length;

                                    if (bools.Length == 1 || count == 1)
                                    {
                                        client.WriteSingleCoils(IdTrans, unit, (ushort)addrStart, bools[0], ref result);
                                    }
                                    else
                                    {
                                        byte[] bytes = ModbusRTUmaster.ValToBytes(bools);
                                        client.WriteMultipleCoils(IdTrans, unit, (ushort)addrStart, (ushort)count, bytes, ref result);
                                    }
                                }
                                break;

                            case eDataType.Byte:
                                {
                                    byte[] bytes = newValues.Select(x => byte.Parse(x)).ToArray();
                                    if (count > bytes.Length * 8)
                                        count = bytes.Length * 8;

                                    client.WriteMultipleCoils(IdTrans, unit, (ushort)addrStart, (ushort)count, bytes, ref result);
                                }
                                break;

                            case eDataType.Short:
                                {
                                    short[] shorts = newValues.Select(x => short.Parse(x)).ToArray();
                                    List<byte> bytes = new List<byte>();
                                    foreach (var item in shorts)
                                        bytes.AddRange(ModbusRTUmaster.ValToBytes(item));

                                    if (count > bytes.Count * 8)
                                        count = bytes.Count * 8;

                                    client.WriteMultipleCoils(IdTrans, unit, (ushort)addrStart, (ushort)count, bytes.ToArray(), ref result);
                                }
                                break;

                            default:
                                // Этот тип данных не поддерживается
                                return new TagResult(newValue, TAG.CM.NotSupport);
                        }
                        break;

                    case "HR":
                        switch (DataType)
                        {
                            case eDataType.Bool:
                            case eDataType.Byte:
                            case eDataType.Short:
                                if (count == 1)
                                {
                                    byte[] bytes = ModbusRTUmaster.ValToBytes(short.Parse(newValues[0]));
                                    client.WriteSingleRegister(IdTrans, unit, (ushort)addrStart, bytes, ref result);
                                }
                                else
                                {
                                    short[] shorts = newValues.Select(x => short.Parse(x)).ToArray();
                                    List<byte> bytes = new List<byte>();
                                    foreach (var item in shorts)
                                        bytes.AddRange(ModbusRTUmaster.ValToBytes(item));

                                    client.WriteMultipleRegister(IdTrans, unit, (ushort)addrStart, bytes.ToArray(), ref result);
                                }
                                break;

                            case eDataType.UShort:
                                if (count == 1)
                                {
                                    byte[] bytes = ModbusRTUmaster.ValToBytes(ushort.Parse(newValues[0]));
                                    client.WriteSingleRegister(IdTrans, unit, (ushort)addrStart, bytes, ref result);
                                }
                                else
                                {
                                    ushort[] shorts = newValues.Select(x => ushort.Parse(x)).ToArray();
                                    List<byte> bytes = new List<byte>();
                                    foreach (var item in shorts)
                                        bytes.AddRange(ModbusRTUmaster.ValToBytes(item));

                                    client.WriteMultipleRegister(IdTrans, unit, (ushort)addrStart, bytes.ToArray(), ref result);
                                }
                                break;

                            case eDataType.Int:
                                {
                                    int[] ints = newValues.Select(x => int.Parse(x)).ToArray();
                                    List<byte> bytes = new List<byte>();
                                    foreach (var item in ints)
                                        bytes.AddRange(ModbusRTUmaster.ValToBytes(item, addrOrder != 0));

                                    client.WriteMultipleRegister(IdTrans, unit, (ushort)addrStart, bytes.ToArray(), ref result);
                                }
                                break;

                            case eDataType.Float:
                                {
                                    float[] floats = newValues.Select(x => ModbusRTUmaster.FloatFromString(x)).ToArray();
                                    List<byte> bytes = new List<byte>();
                                    foreach (var item in floats)
                                        bytes.AddRange(ModbusRTUmaster.ValToBytes(item, addrOrder != 0));

                                    client.WriteMultipleRegister(IdTrans, unit, (ushort)addrStart, bytes.ToArray(), ref result);
                                }
                                break;

                            case eDataType.Double:
                                {
                                    double[] doubles = newValues.Select(x => ModbusRTUmaster.DoubleFromString(x)).ToArray();
                                    List<byte> bytes = new List<byte>();
                                    foreach (var item in doubles)
                                        bytes.AddRange(ModbusRTUmaster.ValToBytes(item, addrOrder != 0));

                                    client.WriteMultipleRegister(IdTrans, unit, (ushort)addrStart, bytes.ToArray(), ref result);
                                }
                                break;

                            default:
                                // Этот тип данных не поддерживается
                                return new TagResult(newValue, TAG.CM.NotSupport);
                        }
                        break;
                }


                // Есть ли ошибки в ответе?
                if (client.statusLastAnswer != 0)
                {
                    if (client.statusLastAnswer == ModbusRTUmaster.excExceptionConnectionLost)
                    {
                        log?.Invoke(new CodeMessage(-1, $"Есть ошибки в ответе: ModbusRTUmaster.excExceptionConnectionLost for {address}"));
                        return new TagResult(0, TAG.CM.NotSupport);
                    }
                    return new TagResult(0, -client.statusLastAnswer, ModbusRTUmaster.exc[client.statusLastAnswer]);
                }

                return new TagResult(newValue);
            }
            catch (Exception ex)
            {
                return new TagResult(newValue, ex);
            }
        }


    }
}
