using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
using System.Collections;

namespace WindowsFormsIDevice.Connector.Driver.Component
{
    class ModbusRTUmaster
    {
        private byte[] tcpSynClBuffer = new byte[2048];

        public delegate void MyLog(string message);
        public MyLog log;

        public bool connected => _connected;

        public byte statusLastAnswer = 0; // статус ответа на последний запрос

        // Private declarations
        private static bool _connected = false;

        // порт
        private SerialPort sport;   // порт подключения
        private string portName = "COM1";      // имя порта для подключения
        private int baudrate = 9600;    // скорость передачи
        private Parity parity = Parity.None;   // контроль четности
        private int dataBits = 8;   //  кол-во бит данных
        private StopBits stopBits = StopBits.One;  //кол-во стоп битов
        private byte UnitIdentifier = 1; // Slave ID
        private int _timeout = 500;

        //
        private List<byte> recievedData = new List<byte>();   // буфер с полученными данными

        #region CONST

        // Constants for access
        private const byte fctReadCoil = 1;
        private const byte fctReadDiscreteInputs = 2;
        private const byte fctReadHoldingRegister = 3;
        private const byte fctReadInputRegister = 4;
        private const byte fctWriteSingleCoil = 5;
        private const byte fctWriteSingleRegister = 6;
        private const byte fctWriteMultipleCoils = 15;
        private const byte fctWriteMultipleRegister = 16;
        private const byte fctReadWriteMultipleRegister = 23;

        /// <summary>Constant for exception illegal function.</summary>
        public const byte excIllegalFunction = 1;
        /// <summary>Constant for exception illegal data address.</summary>
        public const byte excIllegalDataAdr = 2;
        /// <summary>Constant for exception illegal data value.</summary>
        public const byte excIllegalDataVal = 3;
        /// <summary>Constant for exception slave device failure.</summary>
        public const byte excSlaveDeviceFailure = 4;
        /// <summary>Constant for exception acknowledge. This is triggered if a write request is executed while the watchdog has expired.</summary>
        public const byte excAck = 5;
        /// <summary>Constant for exception slave is busy/booting up.</summary>
        public const byte excSlaveIsBusy = 6;
        /// <summary>Constant for exception gate path unavailable.</summary>
        public const byte excGatePathUnavailable = 10;
        /// <summary>Constant for exception not connected.</summary>
        public const byte excExceptionNotConnected = 253;
        /// <summary>Constant for exception connection lost.</summary>
        public const byte excExceptionConnectionLost = 254;
        /// <summary>Constant for exception response timeout.</summary>
        public const byte excExceptionTimeout = 255;
        /// <summary>Constant for exception wrong offset.</summary>
        private const byte excExceptionOffset = 128;
        /// <summary>Constant for exception send failt.</summary>
        private const byte excSendFailt = 100;

        public const byte excExceptionEmpty = 252;
        public const byte excExceptionLittleData = 251;
        public const byte excExceptionOtherTelegram = 250;
        public const byte excExceptionCRC = 248;
        public const byte excExceptionZero = 249;
        public const byte excExceptionPort = 247;

        private const int minBytesAnswer = 5;

        #endregion

        #region INFO
        static public Dictionary<byte, string> exc = new Dictionary<byte, string>()
            {
                { 1, "illegal function" },
                { 2, "illegal data address" },
                { 3, "illegal data value" },
                { 4, "slave device failure" },
                { 5, "this is triggered if a write request is executed while the watchdog has expired" },
                { 6, "slave is busy/booting up" },
                { 10, "gate path unavailable" },
                { 253, "not connected" },
                { 254, "connection lost" },
                { 255, "response timeout" },
                { 128, "wrong offset" },
                { 100, "send failt" },
                { 252, "Empty data" },
                { 251, "Little Data" },
                { 250, "Other telegram" },
                { 249, "Zero" },
                { 248, "CRC fault" },
                { 247, "COM-port fault" },
            };
        #endregion

        public ModbusRTUmaster() {


        }

        // ==========================================================================================================

        #region ConnectDisconnect

        // Открытие COM-порта
        public void Connect(string portName, int baudrate, Parity parity, int dataBits, StopBits stopBits, byte UnitIdentifier, int timeout)
        {
            recievedData.Clear(); // очистка буфера

            if (_connected == false)
            {
                if (sport == null || sport.IsOpen == false)
                {
                    log("connect...");
                    try
                    {
                        _timeout = timeout;
                        //-----------------------------------------------------------------
                        this.portName = portName;
                        this.baudrate = baudrate;
                        this.parity = parity;
                        this.dataBits = dataBits;
                        this.stopBits = stopBits;
                        this.UnitIdentifier = UnitIdentifier;

                        // ----------------------------------------------------------------
                        sport = new SerialPort(portName, baudrate, parity, dataBits, stopBits);
                        sport.ReadTimeout = _timeout;
                        sport.WriteTimeout = _timeout;
                        sport.Open();
                        sport.DataReceived += new SerialDataReceivedEventHandler(SerialPortDataReceived);
                        _connected = true;
                        log("OK");
                    }
                    catch (System.IO.IOException error)
                    {
                        _connected = false;
                        log(error.HResult.ToString("X") + " - " + error.Message);
                        throw (error);
                    }
                    catch (Exception ex)
                    {
                        _connected = false;
                        log(ex.HResult.ToString("X") + " - " + ex.Message);
                    }
                }
            }
        }

        // Закрытие COM-порта
        public void Disconnect()
        {
            if (_connected == true)
            {
                if (sport != null && sport.IsOpen)
                {
                    log("disconnect...");
                    sport.DataReceived -= new SerialDataReceivedEventHandler(SerialPortDataReceived);
                    sport.Close();
                }
            }
            _connected = false;
            log("OK");
        }

        #endregion

        // ==========================================================================================================

        // Событие: получение данных
        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            log($"Получение данных {portName}...");

            try
            {
                var data = new byte[sport.BytesToRead];
                sport.Read(data, 0, data.Length);

                log($"   Принято {data.Length} байт");

                DataReceived(data); // работа с данными от COM-порта

            }
            catch (System.IO.IOException ex)
            {
                statusLastAnswer = excExceptionConnectionLost;
                log($"   Ошибка порта при получении данных: {ex.HResult.ToString("X")} {ex.Message}");
            }
            catch (Exception ex)
            {
                statusLastAnswer = excExceptionPort;
                log($"   Ошибка получения данных: {ex.HResult.ToString("X")} {ex.Message}");
            }
            finally
            {
                //log("Данные COM-порта получены");
            }
        }

        // работа с данными от COM-порта
        private void DataReceived(byte[] data)
        {
            recievedData.AddRange(data); // добавляем полученные данные в список
            log($"   Буфер {recievedData.Count} байт");
            //...
        }

        // ===================================================================================================================

        internal void CallException(ushort id, byte unit, byte function, byte exception)
        {
            if ((sport == null)) return;

            statusLastAnswer = exception;

            log($"id={id} unit={unit} function={function} exception = {exception}");
            if (exc.ContainsKey(exception))
                log($"номер ошибки {exception}: {exc[exception]}");
        }

        #region Functions

        public void ReadCoils(ushort id, byte unit, ushort startAddress, ushort numInputs, ref byte[] values)
        {
            log(">");
            log($"Read Coils [{id}]");
            statusLastAnswer = 0;
            if (numInputs > 2000)
            {
                CallException(id, unit, fctReadCoil, excIllegalDataVal);
                return;
            }
            var waitBytes = 5 + (int)Math.Ceiling((decimal)numInputs/8);
            values = WriteSyncData(CreateReadHeader(id, unit, startAddress, numInputs, fctReadCoil), id, minBytesAnswer, waitBytes);
        }
        public void ReadDiscreteInputs(ushort id, byte unit, ushort startAddress, ushort numInputs, ref byte[] values)
        {
            log(">");
            log($"Read Discrete Inputs [{id}]");
            statusLastAnswer = 0;
            if (numInputs > 2000)
            {
                CallException(id, unit, fctReadDiscreteInputs, excIllegalDataVal);
                return;
            }
            var waitBytes = 5 + (int)Math.Ceiling((decimal)numInputs / 8);
            values = WriteSyncData(CreateReadHeader(id, unit, startAddress, numInputs, fctReadDiscreteInputs), id, minBytesAnswer, waitBytes);
        }
        public void ReadHoldingRegister(ushort id, byte unit, ushort startAddress, ushort numInputs, ref byte[] values)
        {
            log(">");
            log($"Read HR [{id}]");
            statusLastAnswer = 0;
            if (numInputs > 125)
            {
                CallException(id, unit, fctReadHoldingRegister, excIllegalDataVal);
                return;
            }
            var waitBytes = 5 + numInputs * 2;
            values = WriteSyncData(CreateReadHeader(id, unit, startAddress, numInputs, fctReadHoldingRegister), id, minBytesAnswer, waitBytes);
        }
        public void ReadInputRegister(ushort id, byte unit, ushort startAddress, ushort numInputs, ref byte[] values)
        {
            log(">");
            log($"Read IR [{id}]");
            statusLastAnswer = 0;
            if (numInputs > 125)
            {
                CallException(id, unit, fctReadInputRegister, excIllegalDataVal);
                return;
            }
            var waitBytes = 5 + numInputs * 2;
            values = WriteSyncData(CreateReadHeader(id, unit, startAddress, numInputs, fctReadInputRegister), id, minBytesAnswer, waitBytes);
        }
        public void WriteSingleCoils(ushort id, byte unit, ushort startAddress, bool OnOff, ref byte[] result)
        {
            log(">");
            log($"Write Coil [{id}]");
            statusLastAnswer = 0;
            byte[] data;
            data = CreateWriteSingleCoilHeader(id, unit, startAddress, OnOff);
            result = WriteSyncData(data, id, minBytesAnswer, 8);
        }
        public void WriteMultipleCoils(ushort id, byte unit, ushort startAddress, ushort numBits, byte[] values, ref byte[] result)
        {
            log(">");
            log($"Write Coils [{id}]");
            statusLastAnswer = 0;
            ushort numBytes = Convert.ToUInt16(values.Length);
            if (numBytes > 250 || numBits > 2000)
            {
                CallException(id, unit, fctWriteMultipleCoils, excIllegalDataVal);
                return;
            }

            byte[] data;
            data = CreateWriteMultiCoilsHeader(id, unit, startAddress, numBits, values);
            result = WriteSyncData(data, id, minBytesAnswer, 8);
        }
        public void WriteSingleRegister(ushort id, byte unit, ushort startAddress, byte[] value, ref byte[] result)
        {
            log(">");
            log($"Write Register [{id}]");
            statusLastAnswer = 0;
            if (value.GetUpperBound(0) != 1)
            {
                CallException(id, unit, fctReadCoil, excIllegalDataVal);
                return;
            }
            byte[] data;
            data = CreateWriteSingleRegisterHeader(id, unit, startAddress, value); //, fctWriteSingleRegister);
            result = WriteSyncData(data, id, minBytesAnswer, 8);
        }
        public void WriteMultipleRegister(ushort id, byte unit, ushort startAddress, byte[] values, ref byte[] result)
        {
            log(">");
            log($"Write Registers [{id}]");
            statusLastAnswer = 0;
            ushort numBytes = Convert.ToUInt16(values.Length);
            if (numBytes > 250)
            {
                CallException(id, unit, fctWriteMultipleRegister, excIllegalDataVal);
                return;
            }

            if (numBytes % 2 > 0) numBytes++;
            byte[] data;

            data = CreateWriteMultiRegistersHeader(id, unit, startAddress, values);
            result = WriteSyncData(data, id, minBytesAnswer, 8);
        }
        public void ReadWriteMultipleRegister(ushort id, byte unit, ushort startReadAddress, ushort numInputs, ushort startWriteAddress, byte[] values, ref byte[] result)
        {
            log(">");
            log($"Read Write Registers [{id}]");
            statusLastAnswer = 0;
            ushort numBytes = Convert.ToUInt16(values.Length);
            if (numBytes > 250)
            {
                CallException(id, unit, fctReadWriteMultipleRegister, excIllegalDataVal);
                return;
            }

            if (numBytes % 2 > 0) numBytes++;
            byte[] data;

            data = CreateReadWriteHeader(id, unit, startReadAddress, numInputs, startWriteAddress, Convert.ToUInt16(numBytes / 2));
            Array.Copy(values, 0, data, 17, values.Length);
            result = WriteSyncData(data, id);
        }

        #endregion

        #region Headers

        // --- CRC ---
        public static ushort CRC(byte[] buf)
        {
            ushort crc = 0xFFFF;
            int len = buf.Length;

            for (int pos = 0; pos < len; pos++)
            {
                crc ^= buf[pos];

                for (int i = 8; i != 0; i--)
                {
                    if ((crc & 0x0001) != 0)
                    {
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    else
                        crc >>= 1;
                }
            }

            // lo-hi
            //return crc;

            // ..or
            // hi-lo reordered
            return (ushort)((crc >> 8) | (crc << 8));
        }

        // ------------------------------------------------------------------------
        // Create modbus header for read action
        private byte[] CreateReadHeader(ushort id, byte unit, ushort startAddress, ushort length, byte function)
        {
            byte[] data = new byte[8];

            data[0] = unit;					// Slave address
            data[1] = function;				// Function code
            byte[] _adr = BitConverter.GetBytes((short)startAddress);
            data[2] = _adr[1];				// Start address
            data[3] = _adr[0];				// Start address
            byte[] _length = BitConverter.GetBytes((short)length);
            data[4] = _length[1];			// Number of data to read
            data[5] = _length[0];			// Number of data to read

            byte[] _CRC = BitConverter.GetBytes((ushort)CRC(data.Take(6).ToArray()));
            data[6] = _CRC[1];			// CRC
            data[7] = _CRC[0];			// CRC

            return data;
        }
        // ------------------------------------------------------------------------
        // Create modbus header for write action
        private byte[] CreateWriteSingleRegisterHeader(ushort id, byte unit, ushort startAddress, byte[] value)
        {
            byte[] data = new byte[8];

            data[0] = unit;					    // Slave address
            data[1] = fctWriteSingleRegister;   // Function code
            byte[] _adr = BitConverter.GetBytes((short)startAddress);
            data[2] = _adr[1];				    // Start address
            data[3] = _adr[0];				    // Start address
            data[4] = value[0];			    // Value
            data[5] = value[1];			    // Value

            byte[] _CRC = BitConverter.GetBytes((ushort)CRC(data.Take(6).ToArray()));
            data[6] = _CRC[1];			// CRC
            data[7] = _CRC[0];			// CRC

            return data;
        }

        // ------------------------------------------------------------------------
        // Create modbus header for write action
        private byte[] CreateWriteMultiRegistersHeader(ushort id, byte unit, ushort startAddress, byte[] values)
        {
            var bytes = values.Length; // количество байт значений
            var count = bytes / 2; // количество регистров

            byte[] data = new byte[9 + bytes];

            data[0] = unit;					    // Slave address
            data[1] = fctWriteMultipleRegister;   // Function code
            byte[] _adr = BitConverter.GetBytes((short)startAddress);
            data[2] = _adr[1];				    // Start address
            data[3] = _adr[0];                  // Start address
            byte[] _count = BitConverter.GetBytes((short)count);
            data[4] = _count[1];			    // Count
            data[5] = _count[0];			    // Count
            data[6] = (byte)bytes; // byte of data
            for (var i=1; i<=bytes;i++)
            {
                data[6+i] = values[i-1];  // Data
            }

            byte[] _CRC = BitConverter.GetBytes((ushort)CRC(data.Take(7+bytes).ToArray()));
            data[7 + bytes] = _CRC[1];			// CRC
            data[8 + bytes] = _CRC[0];			// CRC

            return data;
        }

        // ------------------------------------------------------------------------
        // Create modbus header for write action
        private byte[] CreateWriteSingleCoilHeader(ushort id, byte unit, ushort startAddress, bool value)
        {
            byte[] data = new byte[8];

            data[0] = unit;					// Slave address
            data[1] = fctWriteSingleCoil;				// Function code
            byte[] _adr = BitConverter.GetBytes((short)startAddress);
            data[2] = _adr[1];				// Start address
            data[3] = _adr[0];				// Start address

            data[4] = (byte)((value) ? 255 : 0);			// Number of data to read
            data[5] = 0;			// Number of data to read

            byte[] _CRC = BitConverter.GetBytes((ushort)CRC(data.Take(6).ToArray()));
            data[6] = _CRC[1];			// CRC
            data[7] = _CRC[0];			// CRC

            return data;
        }

        // Create modbus header for write action
        private byte[] CreateWriteMultiCoilsHeader(ushort id, byte unit, ushort startAddress, ushort numBits, byte[] values)
        {
            var bytes = values.Length; // количество байт значений

            byte[] data = new byte[9 + bytes];

            data[0] = unit;					    // Slave address
            data[1] = fctWriteMultipleCoils;   // Function code
            byte[] _adr = BitConverter.GetBytes((short)startAddress);
            data[2] = _adr[1];				    // Start address
            data[3] = _adr[0];                  // Start address
            byte[] _count = BitConverter.GetBytes((short)numBits);
            data[4] = _count[1];			    // Count
            data[5] = _count[0];			    // Count
            data[6] = (byte)bytes; // byte of data
            for (var i = 1; i <= bytes; i++)
            {
                data[6 + i] = values[i - 1];  // Data
            }

            byte[] _CRC = BitConverter.GetBytes((ushort)CRC(data.Take(7 + bytes).ToArray()));
            data[7 + bytes] = _CRC[1];			// CRC
            data[8 + bytes] = _CRC[0];			// CRC

            return data;
        }

        // ------------------------------------------------------------------------
        // Create modbus header for write action
        private byte[] CreateWriteHeader(ushort id, byte unit, ushort startAddress, ushort numData, ushort numBytes, byte function)
        {
            byte[] data = new byte[numBytes + 11];
            // ???
            return data;
        }

        // ------------------------------------------------------------------------
        // Create modbus header for read/write action
        private byte[] CreateReadWriteHeader(ushort id, byte unit, ushort startReadAddress, ushort numRead, ushort startWriteAddress, ushort numWrite)
        {
            byte[] data = new byte[numWrite * 2 + 17];
            // ???
            return data;
        }

        #endregion

        // ==========================================================================================================

        // Write data and and wait for response
        private byte[] WriteSyncData(byte[] write_data, ushort id, ushort minCharRetval = 5, int waitBytes = 0)
        {
            if (sport != null && sport.IsOpen)
            {
                try
                {
                    log("TX: " + String.Join(" ", write_data.Select(x => x.ToString("X")).Select(y => y.Length < 2 ? "0" + y : y)));

                    // очистка буфера
                    recievedData.Clear();

                    // запрос
                    sport.Write(write_data, 0, write_data.Length);

                    // 1. Асинхронный вариант
                    // ждем ответ
                    DateTime dt = DateTime.Now;
                    do
                    {
                        if (recievedData.Count >= 5 && recievedData[1] > excExceptionOffset) // ответ с ошибкой
                            break;

                        if (waitBytes > 0 && recievedData.Count == waitBytes) // ожидаемое количество данных получено
                            break;

                        TimeSpan ts = DateTime.Now.Subtract(dt);
                        if (ts.TotalMilliseconds > _timeout)
                            break;

                    } while (true);

                    // ответ
                    tcpSynClBuffer = recievedData.ToArray();
                    
                    // результат
                    if (recievedData.Any() == false) // ничего нет!
                    {
                        log("RX: none");
                        CallException(id, write_data[0], write_data[1], excExceptionEmpty);
                        return new byte[] { };
                    }

                    // очистка буфера
                    //recievedData.Clear();

                    // Количество байт в ответе
                    int len = tcpSynClBuffer.Length; // tcpSynClBuffer[2] + 2;

                    string rvDEC = "Retval (DEC)";
                    //string rvHEX = "Retval (HEX)";
                    for (var chri = 0; chri < len; chri++)
                    {
                        rvDEC += $"-{tcpSynClBuffer[chri]}";
                        //rvHEX += $"-{tcpSynClBuffer[chri].ToString("X")}";
                    }
                    
                    log(rvDEC);
                    //log(rvHEX);

                    if (len < minCharRetval)
                    {
                        log($"RX: {len} < {minCharRetval}");

                        CallException(id, write_data[0], write_data[1], excExceptionLittleData);
                        return new byte[] { };
                    }

                    log("RX: " + String.Join(" ", tcpSynClBuffer.Select(x => x.ToString("X")).Take(len).Select(y => y.Length < 2 ? "0" + y : y)));

                    // Проверка CRC 
                    byte[] _CRC = BitConverter.GetBytes((ushort)CRC(tcpSynClBuffer.Take(len-2).ToArray()));
                    log($"RX CRC: {tcpSynClBuffer[len - 2]} {tcpSynClBuffer[len - 1]} = ? {_CRC[1]} {_CRC[0]}");
                    if (tcpSynClBuffer[len - 2] != _CRC[1] || tcpSynClBuffer[len - 1] != _CRC[0])
                    {
                        log($"Error CRC");

                        CallException(id, write_data[0], write_data[1], excExceptionCRC);
                        return new byte[] { };
                    }

                    // ---
                    byte unit = tcpSynClBuffer[0];
                    byte function = tcpSynClBuffer[1];
                    byte[] data;

                    // ------------------------------------------------------------
                    // Response data is slave exception
                    if (function > excExceptionOffset)
                    {
                        function -= excExceptionOffset;
                        CallException(id, unit, function, tcpSynClBuffer[2]);
                        return null;
                    }
                    // ------------------------------------------------------------
                    // Write response data
                    else if ((function >= fctWriteSingleCoil) && (function != fctReadWriteMultipleRegister))
                    {
                        data = new byte[2];
                        Array.Copy(tcpSynClBuffer, 4, data, 0, 2);
                    }
                    // ------------------------------------------------------------
                    // Read response data
                    else
                    {
                        data = new byte[tcpSynClBuffer[2]];
                        Array.Copy(tcpSynClBuffer, 3, data, 0, tcpSynClBuffer[2]);
                    }
                    return data;
                }
                catch (System.IO.IOException ex)
                {
                    log($"   Ошибка порта: {ex.HResult.ToString("X")} {ex.Message}");
                    CallException(id, write_data[0], write_data[1], excExceptionConnectionLost);
                }
                catch (Exception ex) //(SystemException)
                {
                    log($"   Ошибка работы с портом: {ex.HResult.ToString("X")} {ex.Message}");
                    CallException(id, write_data[0], write_data[1], excExceptionPort);
                }
            }
            else CallException(id, write_data[0], write_data[1], excExceptionConnectionLost);
            return null;
        }

        // ===================================================================================================

        #region float(double) from string
        // Получить float из string
        static public float FloatFromString(string text, float faultValue = 0)
        {
            if (float.TryParse(text, out float result1))
                return result1;

            if (text.Contains(".")) // .
            {
                if (float.TryParse(text.Replace('.', ','), out float result2))
                    return result2;
            }
            else // ,
            {
                if (float.TryParse(text.Replace(',', '.'), out float result2))
                    return result2;
            }
            return faultValue;
        }

        // Получить double из string
        static public double DoubleFromString(string text, double faultValue = 0)
        {
            if (double.TryParse(text, out double result1))
                return result1;

            if (text.Contains(".")) // .
            {
                if (double.TryParse(text.Replace('.', ','), out double result2))
                    return result2;
            }
            else // ,
            {
                if (double.TryParse(text.Replace(',', '.'), out double result2))
                    return result2;
            }
            return faultValue;
        }
        #endregion

        #region Value To Bytes
        // Bool
        static public byte[] ValToBytes(bool[] values)
        {
            BitArray bits = new BitArray(values);
            byte[] bytes = new byte[(int)Math.Ceiling((double)values.Length / 8)];
            bits.CopyTo(bytes, 0);
            return bytes;
        }

        // Register
        static public byte[] ValToBytes(short value)
        {
            return BitConverter.GetBytes(value).Reverse().ToArray();
        }

        // Unsigned Register
        static public byte[] ValToBytes(ushort value)
        {
            return BitConverter.GetBytes(value).Reverse().ToArray();
        }

        // Long
        static public byte[] ValToBytes(int value, bool inverse = false)
        {
            byte[] b = BitConverter.GetBytes(value);
            if (inverse == false)
                return new byte[] { b[1], b[0], b[3], b[2] };
            return new byte[] { b[3], b[2], b[1], b[0] };
        }

        // Float
        static public byte[] ValToBytes(float value, bool inverse = false)
        {
            byte[] b = BitConverter.GetBytes(value);
            if (inverse == false)
                return new byte[] { b[1], b[0], b[3], b[2] };
            return new byte[] { b[3], b[2], b[1], b[0] };
        }

        // Double
        static public byte[] ValToBytes(double value, bool inverse = false)
        {
            byte[] b = BitConverter.GetBytes(value);
            if (inverse == false)
                return new byte[] { b[1], b[0], b[3], b[2], b[5], b[4], b[7], b[6] };
            return new byte[] { b[7], b[6], b[5], b[4], b[3], b[2], b[1], b[0] };
        }

        #endregion

        static public byte[] StringToBytes(MBDisplayFormat displayFormat, string text, ushort len = 16)
        {
            if (String.IsNullOrWhiteSpace(text))
                return new byte[] { };

            switch (displayFormat)
            {
                case MBDisplayFormat.Binary: // Binary
                    {
                        ushort write = Convert.ToUInt16(text, 2);
                        return (len > 8) ? BitConverter.GetBytes(write) : new byte[] { (byte)write };
                    }
                case MBDisplayFormat.Register: // Register
                    {
                        short write = short.Parse(text);
                        return ValToBytes(write);
                    }
                case MBDisplayFormat.UnsignedReguster: // Unsigned Register
                    {
                        ushort write = ushort.Parse(text);
                        return ValToBytes(write);
                    }
                case MBDisplayFormat.Long: // Long
                    {
                        int write = int.Parse(text);
                        return ValToBytes(write);
                    }
                case MBDisplayFormat.LongInverse: // Long Inverse
                    {
                        int write = int.Parse(text);
                        return ValToBytes(write, true);
                    }
                case MBDisplayFormat.Float: // Float
                    {
                        float write = SocetModbusTCPmaster.FloatFromString(text);
                        return ValToBytes(write);
                    }
                case MBDisplayFormat.FloatInverse: // Float Inverse
                    {
                        float write = SocetModbusTCPmaster.FloatFromString(text);
                        return ValToBytes(write, true);
                    }
                case MBDisplayFormat.Double: // Double
                    {
                        double write = SocetModbusTCPmaster.DoubleFromString(text);
                        return ValToBytes(write);
                    }
                case MBDisplayFormat.DoubleInverse: // Double Inverse
                    {
                        double write = SocetModbusTCPmaster.DoubleFromString(text);
                        return ValToBytes(write, true);
                    }
                default: // Byte
                    {
                        return (byte.TryParse(text, out byte write)) ? new byte[] { 0, write } : new byte[] { 0, 255 };
                    }
            }
        }

        #region Values

        static public bool[] GetBoolFromBytes(byte[] bytes, ushort count)
        {
            if (count == 0)
                return new bool[] { };

            System.Collections.BitArray b = new System.Collections.BitArray(bytes);
            bool[] bitValues = new bool[b.Count];
            b.CopyTo(bitValues, 0);
            if (count % 8 == 0)
                return bitValues;

            return bitValues.Take(count).ToArray();
        }
        static public string[] GetBinaryFromBytes(byte[] bytes)
        {
            return bytes.Select(x => Convert.ToString(x, 2)).ToArray();
        }
        static public short[] GetInt16FromBytes(byte[] bytes)
        {
            //if (bytes.Length < 2)
            //    return new short[] { Convert.ToInt16(bytes[0]) };

            List<short> result = new List<short>();
            for (var i = 0; i < bytes.Length; i += 2)
                result.Add(BitConverter.ToInt16(new byte[] { bytes[i + 1], bytes[i] }, 0));
            return result.ToArray();
        }
        static public ushort[] GetUInt16FromBytes(byte[] bytes)
        {
            //if (bytes.Length < 2)
            //    return new ushort[] { Convert.ToUInt16(bytes[0]) };

            List<ushort> result = new List<ushort>();
            for (var i = 0; i < bytes.Length; i += 2)
                result.Add(BitConverter.ToUInt16(new byte[] { bytes[i + 1], bytes[i] }, 0));
            return result.ToArray();
        }
        static public int[] GetInt32FromBytes(byte[] bytes)
        {
            List<int> result = new List<int>();
            for (var i = 0; i < bytes.Length; i += 4)
                result.Add(BitConverter.ToInt32(new byte[] { bytes[i + 1], bytes[i], bytes[i + 3], bytes[i + 2] }, 0));
            return result.ToArray();
        }
        static public int[] GetInt32InverseFromBytes(byte[] bytes)
        {
            List<int> result = new List<int>();
            for (var i = 0; i < bytes.Length; i += 4)
                result.Add(BitConverter.ToInt32(new byte[] { bytes[i + 3], bytes[i + 2], bytes[i + 1], bytes[i] }, 0));
            return result.ToArray();
        }
        static public float[] GetFloatFromBytes(byte[] bytes)
        {
            List<float> result = new List<float>();
            for (var i = 0; i < bytes.Length; i += 4)
                result.Add(BitConverter.ToSingle(new byte[] { bytes[i + 1], bytes[i], bytes[i + 3], bytes[i + 2] }, 0));
            return result.ToArray();
        }
        static public float[] GetFloatInverseFromBytes(byte[] bytes)
        {
            List<float> result = new List<float>();
            for (var i = 0; i < bytes.Length; i += 4)
                result.Add(BitConverter.ToSingle(new byte[] { bytes[i + 3], bytes[i + 2], bytes[i + 1], bytes[i] }, 0));
            return result.ToArray();
        }
        static public double[] GetDoubleFromBytes(byte[] bytes)
        {
            List<double> result = new List<double>();
            for (var i = 0; i < bytes.Length; i += 8)
                result.Add(BitConverter.ToDouble(new byte[] { bytes[i + 1], bytes[i], bytes[i + 3], bytes[i + 2], bytes[i + 5], bytes[i + 4], bytes[i + 7], bytes[i + 6] }, 0));
            return result.ToArray();
        }
        static public double[] GetDoubleInverseFromBytes(byte[] bytes)
        {
            List<double> result = new List<double>();
            for (var i = 0; i < bytes.Length; i += 8)
                result.Add(BitConverter.ToDouble(new byte[] { bytes[i + 7], bytes[i + 6], bytes[i + 5], bytes[i + 4], bytes[i + 3], bytes[i + 2], bytes[i + 1], bytes[i] }, 0));
            return result.ToArray();
        }
        #endregion

        #region Bytes To Value
        static public void GetBinaryFromBytes(byte[] values, ref List<dynamic> result)
        {
            foreach (var item in values)
                result.Add(Convert.ToString(item, 2));
        }
        static public void GetInt16FromBytes(byte[] values, ref List<dynamic> result)
        {
            for (var i = 0; i < values.Length; i += 2)
                result.Add(BitConverter.ToInt16(new byte[] { values[i + 1], values[i] }, 0));
        }
        static public void GetUInt16FromBytes(byte[] values, ref List<dynamic> result)
        {
            for (var i = 0; i < values.Length; i += 2)
                result.Add(BitConverter.ToUInt16(new byte[] { values[i + 1], values[i] }, 0));
        }
        static public void GetInt32FromBytes(byte[] values, ref List<dynamic> result)
        {
            for (var i = 0; i < values.Length; i += 4)
                result.Add(BitConverter.ToInt32(new byte[] { values[i + 1], values[i], values[i + 3], values[i + 2] }, 0));
        }
        static public void GetInt32InverseFromBytes(byte[] values, ref List<dynamic> result)
        {
            for (var i = 0; i < values.Length; i += 4)
                result.Add(BitConverter.ToInt32(new byte[] { values[i + 3], values[i + 2], values[i + 1], values[i] }, 0));
        }
        static public void GetFloatFromBytes(byte[] values, ref List<dynamic> result)
        {
            for (var i = 0; i < values.Length; i += 4)
                result.Add(BitConverter.ToSingle(new byte[] { values[i + 1], values[i], values[i + 3], values[i + 2] }, 0));
        }
        static public void GetFloatInverseFromBytes(byte[] values, ref List<dynamic> result)
        {
            for (var i = 0; i < values.Length; i += 4)
                result.Add(BitConverter.ToSingle(new byte[] { values[i + 3], values[i + 2], values[i + 1], values[i] }, 0));
        }

        static public void GetDoubleFromBytes(byte[] values, ref List<dynamic> result)
        {
            for (var i = 0; i < values.Length; i += 8)
                result.Add(BitConverter.ToDouble(new byte[] { values[i + 1], values[i], values[i + 3], values[i + 2], values[i + 5], values[i + 4], values[i + 7], values[i + 6] }, 0));
        }
        static public void GetDoubleInverseFromBytes(byte[] values, ref List<dynamic> result)
        {
            for (var i = 0; i < values.Length; i += 8)
                result.Add(BitConverter.ToDouble(new byte[] { values[i + 7], values[i + 6], values[i + 5], values[i + 4], values[i + 3], values[i + 2], values[i + 1], values[i] }, 0));
        }
        #endregion

        static public IEnumerable<dynamic> BytesToValues(MBDisplayFormat displayFormat, byte[] values)
        {
            var result = new List<dynamic>();

            if (values == null || values.Any() == false)
                return result;

            switch (displayFormat)
            {
                case MBDisplayFormat.Binary: // Binary
                    GetBinaryFromBytes(values, ref result);
                    break;

                case MBDisplayFormat.Register: // Register
                    GetInt16FromBytes(values, ref result);
                    break;

                case MBDisplayFormat.UnsignedReguster: // Unsigned Register
                    GetUInt16FromBytes(values, ref result);
                    break;

                case MBDisplayFormat.Long: // Long
                    GetInt32FromBytes(values, ref result);
                    break;

                case MBDisplayFormat.LongInverse: // Long Inverse
                    GetInt32InverseFromBytes(values, ref result);
                    break;

                case MBDisplayFormat.Float: // Float
                    GetFloatFromBytes(values, ref result);
                    break;

                case MBDisplayFormat.FloatInverse: // Float Inverse
                    GetFloatInverseFromBytes(values, ref result);
                    break;

                case MBDisplayFormat.Double: // Double
                    GetDoubleFromBytes(values, ref result);
                    break;

                case MBDisplayFormat.DoubleInverse: // Double Inverse
                    GetDoubleInverseFromBytes(values, ref result);
                    break;

                default: // Byte
                    for (var i = 0; i < values.Length; i += 1)
                        result.Add(values[i]);
                    break;
            }

            return result;
        }

        static public string BytesToString(MBDisplayFormat displayFormat, byte[] values, string join = " ")
        {
            if (values == null || values.Any() == false)
                return "";

            return String.Join(join, BytesToValues(displayFormat, values));
        }
    }
}
