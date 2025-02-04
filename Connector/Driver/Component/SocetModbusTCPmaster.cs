using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WinSimpleIDriver.Connector.Driver.Component
{
    enum MBDisplayFormat
    {
        Binary = 0,
        Byte = 1,
        Register = 2,
        UnsignedReguster = 3,
        Long = 4,
        LongInverse = 5,
        Float = 6,
        FloatInverse = 7,
        Double = 8,
        DoubleInverse = 9
    }

    class SocetModbusTCPmaster
    {
        private Socket socet;
        private byte[] tcpSynClBuffer = new byte[2048];

        public delegate void MyLog(string message);
        public MyLog log;

        public bool connected => _connected;

        public byte statusLastAnswer = 0; // статус ответа на последний запрос

        // Private declarations
        private static ushort _timeout = 500;
        private static ushort _refresh = 10;
        private static bool _connected = false;

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
        public const byte excExceptionCRC = 248; // не используется
        public const byte excExceptionZero = 249;

        private const int minBytesAnswer = 9;

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
                { 251, "Little Data (<11)" },
                { 250, "Other telegram" },
                { 249, "Zero" },
                { 248, "CRC fault (not used)" },
            };
        #endregion

        public SocetModbusTCPmaster()
        {

        }

        // ==========================================================================================================

        #region ConnectDisconnect

        public void Connect(string ip, int port, ushort timeout)
        {
            log("connect...");
            try
            {
                _timeout = timeout;
                //-----------------------------------------------------------------
                IPAddress _ip;
                if (IPAddress.TryParse(ip, out _ip) == false)
                {
                    IPHostEntry hst = Dns.GetHostEntry(ip);
                    ip = hst.AddressList[0].ToString();
                }
                // ----------------------------------------------------------------
                // Connect synchronous client

                socet = new Socket(IPAddress.Parse(ip).AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socet.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                socet.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, _timeout);
                socet.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, _timeout);
                socet.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, 1);

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

        public void Disconnect()
        {
            log("disconnect...");
            if (socet != null)
            {
                if (socet.Connected)
                {
                    try { socet.Shutdown(SocketShutdown.Both); }
                    catch { }
                    socet.Close();
                }
                socet = null;
            }
            _connected = false;
            log("OK");
        }

        #endregion

        // ==========================================================================================================

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
            var waitBytes = 9 + (int)Math.Ceiling((decimal)numInputs / 8);
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
            var waitBytes = 9 + (int)Math.Ceiling((decimal)numInputs / 8);
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
            var waitBytes = 9 + numInputs * 2;
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
            var waitBytes = 9 + numInputs * 2;
            values = WriteSyncData(CreateReadHeader(id, unit, startAddress, numInputs, fctReadInputRegister), id, minBytesAnswer, waitBytes);
        }
        public void WriteSingleCoils(ushort id, byte unit, ushort startAddress, bool OnOff, ref byte[] result)
        {
            log(">");
            log($"Write Coil [{id}]");
            statusLastAnswer = 0;
            byte[] data;
            data = CreateWriteHeader(id, unit, startAddress, 1, 1, fctWriteSingleCoil);
            if (OnOff == true) data[10] = 255;
            else data[10] = 0;
            result = WriteSyncData(data, id, minBytesAnswer, 12);
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
            data = CreateWriteHeader(id, unit, startAddress, numBits, (byte)(numBytes + 2), fctWriteMultipleCoils);
            Array.Copy(values, 0, data, 13, numBytes);
            result = WriteSyncData(data, id, minBytesAnswer, 12);
        }
        public void WriteSingleRegister(ushort id, byte unit, ushort startAddress, byte[] values, ref byte[] result)
        {
            log(">");
            log($"Write Register [{id}]");
            statusLastAnswer = 0;
            if (values.GetUpperBound(0) != 1)
            {
                CallException(id, unit, fctReadCoil, excIllegalDataVal);
                return;
            }
            byte[] data;
            data = CreateWriteHeader(id, unit, startAddress, 1, 1, fctWriteSingleRegister);
            data[10] = values[0];
            data[11] = values[1];
            result = WriteSyncData(data, id, minBytesAnswer, 12);
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

            data = CreateWriteHeader(id, unit, startAddress, Convert.ToUInt16(numBytes / 2), Convert.ToUInt16(numBytes + 2), fctWriteMultipleRegister);
            Array.Copy(values, 0, data, 13, values.Length);
            result = WriteSyncData(data, id, minBytesAnswer, 12);
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

        // ==========================================================================================================

        // Write data and and wait for response
        private byte[] WriteSyncData(byte[] write_data, ushort id, ushort minCharRetval = 9, int waitBytes = 0)
        {
            if (socet != null && socet.Connected)
            {
                try
                {
                    log("TX: " + String.Join(" ", write_data.Select(x => x.ToString("X")).Select(y => y.Length < 2 ? "0" + y : y)));

                    // запрос
                    socet.Send(write_data, 0, write_data.Length, SocketFlags.None);

                    // ответ
                    tcpSynClBuffer = new byte[2048]; //

                    // результат (и попытки)
                    var maxAttempt = 5;
                    for (var attempt = 1; attempt <= maxAttempt; attempt++)
                    {
                        int result = socet.Receive(tcpSynClBuffer, 0, tcpSynClBuffer.Length, SocketFlags.None);

                        if (result == 0) // ничего нет!
                        {
                            log("RX: none");
                            if (attempt == maxAttempt)
                            {
                                CallException(id, 0, write_data[7], excExceptionEmpty);
                                return new byte[] { };
                            }
                            continue;
                        }

                        if (result < minCharRetval) // слишком мало байт
                        {
                            log($"RX: {result} < {minCharRetval}");
                            if (attempt == maxAttempt)
                            {
                                CallException(id, 0, write_data[7], excExceptionLittleData);
                                return new byte[] { };
                            }
                            continue;
                        }

                        // RX
                        string rvDEC = "Retval (DEC)";
                        string rvHEX = "Retval (HEX)";
                        for (var chri = 0; chri < result; chri++)
                        {
                            rvDEC += $"-{tcpSynClBuffer[chri]}";
                            rvHEX += $"-{tcpSynClBuffer[chri].ToString("X")}";
                        }

                        log("RX: " + String.Join(" ", tcpSynClBuffer.Select(x => x.ToString("X")).Take(result).Select(y => y.Length < 2 ? "0" + y : y)));

                        // 
                        if (tcpSynClBuffer[0] == 0 && tcpSynClBuffer[1] == 0) // пустой ответ
                        {
                            log("RX: 0 0 ...");
                            if (attempt == maxAttempt)
                            {
                                CallException(id, 0, write_data[7], excExceptionZero);
                                return new byte[] { };
                            }
                            continue;
                        }
                            
                        if (write_data[0] != tcpSynClBuffer[0] || write_data[1] != tcpSynClBuffer[1]) // другой ответ
                        {
                            log($"RX: other telegram {write_data[0].ToString("X")} {write_data[1].ToString("X")} <> {tcpSynClBuffer[0].ToString("X")} {tcpSynClBuffer[1].ToString("X")}...");
                            if (attempt == maxAttempt)
                            {
                                CallException(id, 0, write_data[7], excExceptionOtherTelegram);
                                return new byte[] { };
                            }
                            continue;
                        } else
                        {
                            break;
                        }
                    }

                    // ---
                    byte unit = tcpSynClBuffer[6];
                    byte function = tcpSynClBuffer[7];
                    byte[] data;

                    // ------------------------------------------------------------
                    // Response data is slave exception
                    if (function > excExceptionOffset)
                    {
                        function -= excExceptionOffset;
                        CallException(id, unit, function, tcpSynClBuffer[8]);
                        return null;
                    }
                    // ------------------------------------------------------------
                    // Write response data
                    else if ((function >= fctWriteSingleCoil) && (function != fctReadWriteMultipleRegister))
                    {
                        data = new byte[2];
                        Array.Copy(tcpSynClBuffer, 10, data, 0, 2);
                    }
                    // ------------------------------------------------------------
                    // Read response data
                    else
                    {
                        data = new byte[tcpSynClBuffer[8]];
                        Array.Copy(tcpSynClBuffer, 9, data, 0, tcpSynClBuffer[8]);
                    }
                    return data;
                }
                catch (Exception ex) //(SystemException)
                {
                    //Console.WriteLine($"{DateTime.Now} excExceptionConnectionLost ERR={ex.HResult.ToString("X")}: {ex.Message}, len={tcpSynClBuffer.Length}, step={step}");
                    CallException(id, write_data[6], write_data[7], excExceptionConnectionLost);
                }
            }
            else CallException(id, write_data[6], write_data[7], excExceptionConnectionLost);
            return null;
        }

        #region Headers

        // ------------------------------------------------------------------------
        // Create modbus header for read action
        private byte[] CreateReadHeader(ushort id, byte unit, ushort startAddress, ushort length, byte function)
        {
            byte[] data = new byte[12];

            byte[] _id = BitConverter.GetBytes((short)id);
            data[0] = _id[1];			    // Slave id high byte
            data[1] = _id[0];				// Slave id low byte
            data[5] = 6;					// Message size
            data[6] = unit;					// Slave address
            data[7] = function;				// Function code
            byte[] _adr = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)startAddress));
            data[8] = _adr[0];				// Start address
            data[9] = _adr[1];				// Start address
            byte[] _length = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)length));
            data[10] = _length[0];			// Number of data to read
            data[11] = _length[1];			// Number of data to read
            return data;
        }

        // ------------------------------------------------------------------------
        // Create modbus header for write action
        private byte[] CreateWriteHeader(ushort id, byte unit, ushort startAddress, ushort numData, ushort numBytes, byte function)
        {
            byte[] data = new byte[numBytes + 11];

            byte[] _id = BitConverter.GetBytes((short)id);
            data[0] = _id[1];				// Slave id high byte
            data[1] = _id[0];				// Slave id low byte
            byte[] _size = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)(5 + numBytes)));
            data[4] = _size[0];				// Complete message size in bytes
            data[5] = _size[1];				// Complete message size in bytes
            data[6] = unit;					// Slave address
            data[7] = function;				// Function code
            byte[] _adr = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)startAddress));
            data[8] = _adr[0];				// Start address
            data[9] = _adr[1];				// Start address
            if (function >= fctWriteMultipleCoils)
            {
                byte[] _cnt = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)numData));
                data[10] = _cnt[0];			// Number of bytes
                data[11] = _cnt[1];			// Number of bytes
                data[12] = (byte)(numBytes - 2);
            }
            return data;
        }

        // ------------------------------------------------------------------------
        // Create modbus header for read/write action
        private byte[] CreateReadWriteHeader(ushort id, byte unit, ushort startReadAddress, ushort numRead, ushort startWriteAddress, ushort numWrite)
        {
            byte[] data = new byte[numWrite * 2 + 17];

            byte[] _id = BitConverter.GetBytes((short)id);
            data[0] = _id[1];						// Slave id high byte
            data[1] = _id[0];						// Slave id low byte
            byte[] _size = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)(11 + numWrite * 2)));
            data[4] = _size[0];						// Complete message size in bytes
            data[5] = _size[1];						// Complete message size in bytes
            data[6] = unit;							// Slave address
            data[7] = fctReadWriteMultipleRegister;	// Function code
            byte[] _adr_read = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)startReadAddress));
            data[8] = _adr_read[0];					// Start read address
            data[9] = _adr_read[1];					// Start read address
            byte[] _cnt_read = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)numRead));
            data[10] = _cnt_read[0];				// Number of bytes to read
            data[11] = _cnt_read[1];				// Number of bytes to read
            byte[] _adr_write = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)startWriteAddress));
            data[12] = _adr_write[0];				// Start write address
            data[13] = _adr_write[1];				// Start write address
            byte[] _cnt_write = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)numWrite));
            data[14] = _cnt_write[0];				// Number of bytes to write
            data[15] = _cnt_write[1];				// Number of bytes to write
            data[16] = (byte)(numWrite * 2);

            return data;
        }

        #endregion

        internal void CallException(ushort id, byte unit, byte function, byte exception)
        {
            if ((socet == null)) return;
            //if (exception == excExceptionConnectionLost)
            //{
            //    socet = null;
            //}
            //if (OnException != null) OnException(id, unit, function, exception);

            statusLastAnswer = exception;

            log($"id={id} unit={unit} function={function} exception = {exception}");
            if (exc.ContainsKey(exception))
                log($"номер ошибки {exception}: {exc[exception]}");
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
