using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Connector.SGT;
using Connector.Driver.Component;

namespace Connector.Driver
{
    class FormulaAdapter : Device
    {
        public const string driverName = "Formula";

        // Справка
        // Описание адреса устройства
        public static Dictionary<string, string> HelpSource
            => new Dictionary<string, string> {
                        { "Адрес", "Адресом должна быть пустая строка" }
                    };
        // Описание адреса тега для данного устройства
        public static Dictionary<string, string> HelpTag
            => new Dictionary<string, string> {
                        { "Операторы", " ( ) ^ * / % + - > < = OR AND NOT" },
                        { "Логика", "true = 1, false = 0" },
                        { "Дата и время", "NOW, NOW_MSECOND, NOW_SECOND, NOW_MINUTE, NOW_HOUR, NOW_DAY, NOW_DAYWEEK, NOW_DAYYEAR, NOW_MONTH, NOW_YEAR" },
                        { "Конвертация", "HDEX, HDEC, SBYTE, UBYTE, SINT, UINT, SDINT, UDINT, \r\nBYTES_TO_W, BYTES_TO_DW, BYTES_TO_FLOAT, BYTES_TO_LONG, BYTES_TO_DOUBLE, \r\nBYTES_FROM_W, BYTES_FROM_DW, BYTES_FROM_FLOAT, BYTES_FROM_LONG, BYTES_FROM_DOUBLE, DW_TO_FLOAT, DW_FROM_FLOAT"},
                        { "Работа с данными", "DICT, LIST, SEL, RANGE, INSIDE, CHANGE, LEN" },
                        { "Разное", "RND = случайное число 1 ... 100;\nRNDDOUBLE = случайное число 0 ... <1;\nPLUS = +1;\nBYTEVAL = 0 ... 255;\nBOOL = 0 ... 1;\nSIN = -1.00 ... 1.00;\nCOS = -1.00 ... 1.00;" },
                        { "Пример №1 Условие", "{Value1} AND ({Value2} OR {Value3})" },
                        { "Пример №2 Сравнение", "{Value1} > ( {Value3} + 10 )" },
                        { "Пример №3 Формула = 12", "(100 + 200)/25" },
                        { "Пример №4 Формула", "1,75 + NOW_DAY*1000 + {Value8}" },
                        { "Пример №5 Словарь = 9", "DICT(a;b;c|8;9;10|b)" },
                        { "Пример №6 Значение из списка = Д", "LIST(А;Б;В;Г;Д|4)" },
                        { "Пример №7 Двоичный выбор = Start", "SEL(true;Start;End)" },
                        { "Пример №8 Значение внутри = 501", "INSIDE(value[501];[;])" },
                        { "Пример №9 Ограничение = 100", "LIM(108;15;100)" },
                        { "Пример №10 Сдвиг числа = 97", "ROLL(-3;0;100)" },
                        { "Важно", "Если в функции используется сивол ; или |, то вложенность таких функций запрещена!" }
                    };
        
        // Клиент
        public StringFormula2 client;

        public double value_PLUS = 0; // + 1
        public byte value_BYTE = 0; // 0 - 255
        public int value_Angle = 0; // 0 - 359
        public bool value_Bool = false; // true, false

        public FormulaAdapter()
        {
            client = new StringFormula2();
        }

        ~FormulaAdapter()
        {
            client = null;
        }

        // ---------------------------------------------------------------------------------------------

        #region GET/SET

        // Получить значение тега (с типом)
        public override TagResult GetValue(string address, eDataType DataType)
        {
            dynamic Value = 0; // итоговое значение

            try
            {
                // Вычисления...
                Value = Calculation(address, DataType);

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
            return new TagResult(Tag.ConvertValueWithArray(newValue, DataType));
        }
        #endregion

        // ---------------------------------------------------------------------------------------------

        #region LIB

        // Расчет значения...
        public dynamic Calculation(string address, eDataType DataType)
        {
            // >>> Расчет...
            if (!String.IsNullOrWhiteSpace(address))
            {
                #region Mask DateTime
                // DateTime
                address = address.Replace("NOW_MSECOND", DateTime.Now.Millisecond.ToString());
                address = address.Replace("NOW_SECOND", DateTime.Now.Second.ToString());
                address = address.Replace("NOW_MINUTE", DateTime.Now.Minute.ToString());
                address = address.Replace("NOW_HOUR", DateTime.Now.Hour.ToString());
                address = address.Replace("NOW_DAYWEEK", DateTime.Now.DayOfWeek.ToString());
                address = address.Replace("NOW_DAYYEAR", DateTime.Now.DayOfYear.ToString());
                address = address.Replace("NOW_DAY", DateTime.Now.Day.ToString());
                address = address.Replace("NOW_MONTH", DateTime.Now.Month.ToString());
                address = address.Replace("NOW_YEAR", DateTime.Now.Year.ToString());
                address = address.Replace("NOW", DateTime.Now.ToString());
                #endregion

                #region Mask 2 (RND, PLUS, SIN, BOOL...)
                if (address.Contains("RNDDOUBLE"))
                {
                    Random _random = new Random();
                    address = address.Replace("RNDDOUBLE", Convert.ToString(_random.NextDouble()));
                }
                else if (address.Contains("RND"))
                {
                    Random _random = new Random();
                    address = address.Replace("RND", Convert.ToString(_random.Next(1, 100)));
                }
                if (address.Contains("PLUS"))
                {
                    value_PLUS++;
                    address = address.Replace("PLUS", Convert.ToString(value_PLUS));
                }
                if (address.Contains("BYTEVAL"))
                {
                    value_BYTE++;
                    if (value_BYTE > 255)
                        value_BYTE = 0;
                    address = address.Replace("BYTEVAL", Convert.ToString(value_BYTE));
                }
                if (address.Contains("SIN"))
                {
                    value_Angle++;
                    if (value_Angle > 359)
                        value_Angle = 0;
                    address = address.Replace("SIN", Convert.ToString(Math.Round(Math.Sin(Math.PI * value_Angle / 180), 4)));
                }
                if (address.Contains("COS"))
                {
                    value_Angle++;
                    if (value_Angle > 359)
                        value_Angle = 0;
                    address = address.Replace("COS", Convert.ToString(Math.Round(Math.Cos(Math.PI * value_Angle / 180), 4)));
                }
                if (address.Contains("BOOL"))
                {
                    value_Bool = !value_Bool;
                    address = address.Replace("BOOL", Convert.ToString(value_Bool));
                }
                #endregion

                string prevAddress;
                do
                {
                    prevAddress = address; // тот адрес, который был в начале
                    // Функции
                    #region Функция Y = Fn(X1;X2;X3)

                    // Словарь
                    while (true) // DICT
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "DICT", '|');
                        if (result.Length != 4) break;
                        // 0 = DICT(a;b;c|8;9;10|b)
                        // 1 = a;b;c
                        // 2 = 8;9;10
                        // 3 = Key = b
                        // Result = 9

                        dynamic CValue;
                        try
                        {
                            string[] myKeys = result[1].Split(';');
                            string[] SRC = result[2].Split(';');

                            int index = Array.IndexOf(myKeys, result[3]);
                            if (index >= 0)
                            {
                                CValue = SRC[index];
                            }
                            else
                            {
                                CValue = 0;
                            }
                        }
                        catch
                        {
                            CValue = 0;
                        }
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }
                    // Список
                    while (true) // LIST
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "LIST", '|');
                        if (result.Length != 3) break;
                        // 0 = LIST(А;Б;В;Г;Д|4)
                        // 1 = 10;20;30;40;50
                        // 2 = Index = 4
                        // Result = Д

                        dynamic CValue;
                        try
                        {
                            string[] SRC = result[1].Split(';');
                            int index = Convert.ToInt32(Calculation(result[2], eDataType.Int));
                            CValue = SRC[index];
                        }
                        catch (Exception ex)
                        {
                            CValue = 0;
                        }
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }
                    while (true) // LISTB
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "LISTB", '|');
                        if (result.Length != 3) break;
                        // 0 = LISTB(А Б В Г Д|4)
                        // 1 = А Б В Г Д
                        // 2 = Index = 4
                        // Result = Д

                        dynamic CValue;
                        try
                        {
                            string[] SRC = result[1].Split(' ');
                            int index = Convert.ToInt32(Calculation(result[2], eDataType.Int));
                            CValue = SRC[index];
                        }
                        catch (Exception ex)
                        {
                            CValue = 0;
                        }
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }
                    while (true) // LISTC
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "LISTC", '|');
                        if (result.Length != 3) break;
                        // 0 = LISTC(А~Б~В~Г~Д|4)
                        // 1 = А~Б~В~Г~Д
                        // 2 = Index = 4
                        // Result = Д

                        dynamic CValue;
                        try
                        {
                            string[] SRC = result[1].Split('~');
                            int index = Convert.ToInt32(Calculation(result[2], eDataType.Int));
                            CValue = SRC[index];
                        }
                        catch (Exception ex)
                        {
                            CValue = 0;
                        }
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }
                    // Двоичный селектор
                    while (true) // SEL
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "SEL", ';');
                        if (result.Length != 4) break;
                        // 0 = SEL(true;Start;End)
                        // 1 = bool value = true
                        // 2 = TRUE value = Start
                        // 3 = FALSE value = End
                        // Result = Start

                        dynamic CValue;
                        try
                        {
                            result[1] = Convert.ToString(Calculation(result[1], eDataType.Bool));
                            CValue = (bool.Parse(result[1])) ? result[2] : result[3];
                            CValue = Calculation(CValue, DataType); //...
                        }
                        catch (Exception ex)
                        {
                            CValue = 0;
                        }
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }
                    // Диапазон
                    while (true) // RANGE
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "RANGE", ';');
                        if (result.Length != 6) break;
                        // 0 = RANGE(50;0;100;200;400)
                        // 1 = value = 50
                        // 2 = min = 0
                        // 3 = max = 100
                        // 4 = NEW min = 200
                        // 5 = NEW max = 400
                        // Result = 300
                        dynamic CValue;
                        try
                        {
                            float inValue = Convert.ToDouble(Calculation(result[1], eDataType.Float));
                            float min = Convert.ToDouble(Calculation(result[2], eDataType.Float));
                            float max = Convert.ToDouble(Calculation(result[3], eDataType.Float));
                            float newMin = Convert.ToDouble(Calculation(result[4], eDataType.Float));
                            float newMax = Convert.ToDouble(Calculation(result[5], eDataType.Float));
                            CValue = (inValue - min) * (newMax - newMin) / (max - min) + newMin;
                        }
                        catch
                        {
                            CValue = 0;
                        }
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }

                    // Получить значение между символами
                    while (true) // INSIDE
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "INSIDE", ';');
                        if (result.Length != 4) break;
                        // 0 = INSIDE(value[501];[;])
                        // 1 = value[]
                        // 2 = [
                        // 3 = ]
                        // Result = 501
                        string str = result[1]; // строка где ищем значение между A и B
                        string a = result[2]; // строка A
                        string b = result[3]; // строка B
                        if (str == "" || (a == "" && b == "")) break;
                        if (a == "")
                        {
                            int index = str.IndexOf(b);
                            if (index > 0)
                                address = address.Replace(result[0], str.Substring(0, index));
                        }
                        else if (b == "")
                        {
                            int index = str.IndexOf(a);
                            if (index + a.Length < str.Length)
                                address = address.Replace(result[0], str.Substring(index + a.Length));
                        }
                        else
                        {
                            int indexA = str.IndexOf(a);
                            int indexB = str.IndexOf(b);
                            if (indexB > indexA && indexA + a.Length < str.Length && indexB > 0)
                            {
                                address = address.Replace(result[0], str.Substring(indexA + a.Length, indexB - (indexA + a.Length)));
                            }

                        }

                        address = Calculation(address, DataType); //...

                    }

                    // Сдвиг
                    while (true) // ROLL
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "ROLL", ';');
                        if (result.Length != 4) break;
                        // 0 = ROLL(1200;0;1000)
                        // 1 = 1200
                        // 2 = 0
                        // 3 = 1000
                        // Result = 200

                        dynamic CValue;
                        try
                        {
                            CValue = Convert.ToDouble(Calculation(result[1], eDataType.Float));
                            double min = Convert.ToDouble(Calculation(result[2], eDataType.Float));
                            double max = Convert.ToDouble(Calculation(result[3], eDataType.Float));

                            if (CValue < min)
                                CValue = max - (min - CValue);
                            if (CValue > max)
                                CValue = min + (CValue - max);
                        }
                        catch (Exception ex)
                        {
                            CValue = 0;
                        }
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }

                    // Ограничение
                    while (true) // LIM
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "LIM", ';');
                        if (result.Length != 4) break;
                        // 0 = LIM(1100;0;1000)
                        // 1 = 1100
                        // 2 = 0
                        // 3 = 1000
                        // Result = 1000

                        dynamic CValue;
                        try
                        {
                            CValue = Convert.ToDouble(Calculation(result[1], eDataType.Float));
                            double min = Convert.ToDouble(Calculation(result[2], eDataType.Float));
                            double max = Convert.ToDouble(Calculation(result[3], eDataType.Float));

                            if (CValue < min)
                                CValue = min;
                            if (CValue > max)
                                CValue = max;
                        }
                        catch
                        {
                            CValue = 0;
                        }
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }

                    // Замена символов
                    while (true) // CHANGE({%rec2_Volume}/,/.)
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "CHANGE", ';');
                        if (result.Length != 4) break;

                        dynamic CValue;
                        try
                        {
                            string Delim1 = result[2];
                            if (Delim1 == "") Delim1 = ",";
                            string Delim2 = result[3];
                            if (Delim2 == "") Delim2 = ".";
                            CValue = result[1].Replace(Delim1, Delim2);
                        }
                        catch
                        {
                            CValue = 0;
                        }
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }

                    #endregion

                    // Конвертация
                    while (true) // DEC в HEX
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "DHEX");
                        if (result.Length != 2) break;
                        long LValue = Convert.ToInt32(Calculation(result[1], eDataType.Long)); // ...
                        string SValue = LValue.ToString("X");
                        address = address.Replace(result[0], SValue);
                    }
                    while (true) // HEX в DEC
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "HDEC");
                        if (result.Length != 2) break;
                        string SValue = Convert.ToString(Calculation(result[1], eDataType.STRING)); // ...
                        long LValue = Convert.ToInt32(SValue, 16);
                        address = address.Replace(result[0], LValue.ToString());
                    }
                    while (true) // DEC в BIN
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "DBIN");
                        if (result.Length != 2) break;

                        string s = Convert.ToString(Calculation(result[1], eDataType.Long), 2); //Convert to binary in a string
                        address = address.Replace(result[0], s);
                    }
                    while (true) // BIN в DEC
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "BDEC");
                        if (result.Length != 2) break;

                        long LValue = Convert.ToInt64(Calculation(result[1], eDataType.STRING), 2);
                        address = address.Replace(result[0], LValue.ToString());
                    }
                    while (true) // SBYTE
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "SBYTE");
                        if (result.Length != 2) break;
                        result[1] = Calculation(result[1], eDataType.Int); // ...
                        sbyte CValue = Convert.ToSByte(Convert.ToByte(result[1]));
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }
                    while (true) // UBYTE
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "UBYTE");
                        if (result.Length != 2) break;
                        result[1] = Calculation(result[1], eDataType.Int); // ...
                        byte CValue = Convert.ToByte(Convert.ToSByte(result[1]));
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }
                    while (true) // SINT
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "SINT");
                        if (result.Length != 2) break;
                        result[1] = Calculation(result[1], eDataType.Int); // ...
                        Int16 CValue = Convert.ToInt16(Convert.ToUInt16(result[1]));
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }
                    while (true) // UINT
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "UINT");
                        if (result.Length != 2) break;
                        result[1] = Calculation(result[1], eDataType.Int); // ...
                        UInt16 CValue = Convert.ToUInt16(Convert.ToInt16(result[1]));
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }
                    while (true) // SDINT
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "SDINT");
                        if (result.Length != 2) break;
                        result[1] = Calculation(result[1], eDataType.Long); // ...
                        Int32 CValue = Convert.ToInt32(Convert.ToUInt32(result[1]));
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }
                    while (true) // UDINT
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "UDINT");
                        if (result.Length != 2) break;
                        result[1] = Calculation(result[1], eDataType.Long); // ...
                        UInt32 CValue = Convert.ToUInt32(Convert.ToInt32(result[1]));
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }

                    while (true) // BYTES_TO_W
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "BYTES_TO_W");
                        if (result.Length != 3) break;
                        byte[] bytes = StringUnpack.Parse_ParamsByte(new string[] { result[1], result[2] });
                        ushort CValue = BitConverter.ToUInt16(bytes, 0);
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }
                    while (true) // BYTES_FROM_W
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "BYTES_FROM_W");
                        if (result.Length != 2) break;
                        byte[] bytes = BitConverter.GetBytes(Convert.ToInt16(result[1]));
                        address = address.Replace(result[0], String.Join(";", bytes));
                    }
                    while (true) // BYTE_FROM_W
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "BYTE_FROM_W");
                        if (result.Length != 3) break;
                        byte CValue = BitConverter.GetBytes(Convert.ToInt16(result[1]))[Convert.ToInt16(result[2])];
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }

                    while (true) // BYTES_TO_DW
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "BYTES_TO_DW");
                        if (result.Length != 5) break;
                        int CValue = BitConverter.ToInt32(StringUnpack.Parse_ParamsByte(new string[] { result[1], result[2], result[3], result[4] }), 0);
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }
                    while (true) // BYTE_FROM_DW
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "BYTE_FROM_DW");
                        if (result.Length != 3) break;
                        byte CValue = BitConverter.GetBytes(Convert.ToInt32(result[1]))[Convert.ToInt16(result[2])];
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }
                    while (true) // BYTES_FROM_DW
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "BYTES_FROM_DW");
                        if (result.Length != 2) break;
                        byte[] bytes = BitConverter.GetBytes(Convert.ToInt32(result[1]));
                        address = address.Replace(result[0], String.Join(";", bytes));
                    }

                    while (true) // BYTES_TO_FLOAT
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "BYTES_TO_FLOAT");
                        if (result.Length != 5) break;
                        double CValue = BitConverter.ToSingle(StringUnpack.Parse_ParamsByte(new string[] { result[1], result[2], result[3], result[4] }), 0);
                        CValue = Math.Round(CValue, 4);
                        address = address.Replace(result[0], CValue.ToString("#.####"));
                    }
                    while (true) // BYTES_FROM_FLOAT
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "BYTES_FROM_FLOAT");
                        if (result.Length != 2) break;
                        byte[] bytes = BitConverter.GetBytes(Convert.ToSingle(result[1]));
                        address = address.Replace(result[0], String.Join(";", bytes));
                    }

                    while (true) // BYTES_TO_LONG
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "BYTES_TO_LONG");
                        if (result.Length != 9) break;
                        Int64 CValue = BitConverter.ToInt64(StringUnpack.Parse_ParamsByte(new string[] { result[1], result[2], result[3], result[4], result[5], result[6], result[7], result[8] }), 0);
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }
                    while (true) // BYTES_FROM_LONG
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "BYTES_FROM_LONG");
                        if (result.Length != 2) break;
                        byte[] bytes = BitConverter.GetBytes(Convert.ToInt64(result[1]));
                        address = address.Replace(result[0], String.Join(";", bytes));
                    }

                    while (true) // BYTES_TO_DOUBLE
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "BYTES_TO_DOUBLE");
                        if (result.Length != 9) break;
                        double CValue = BitConverter.ToDouble(StringUnpack.Parse_ParamsByte(new string[] { result[1], result[2], result[3], result[4], result[5], result[6], result[7], result[8] }), 0);
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }
                    while (true) // BYTES_FROM_DOUBLE
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "BYTES_FROM_DOUBLE");
                        if (result.Length != 2) break;
                        byte[] bytes = BitConverter.GetBytes(Convert.ToDouble(result[1]));
                        address = address.Replace(result[0], String.Join(";", bytes));
                    }
                    while (true) // DW_TO_FLOAT
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "DW_TO_FLOAT"); // байты без знака
                        if (result.Length != 2) break;
                        double CValue = 0;
                        try
                        {
                            if (Convert.ToUInt32(result[1]) != 0)
                            {
                                byte[] CBytes = BitConverter.GetBytes(Convert.ToUInt32(result[1]));
                                CValue = BitConverter.ToSingle(CBytes, 0);
                                CValue = Math.Round(CValue, 4);
                            }
                        }
                        catch
                        {

                        }
                        address = address.Replace(result[0], CValue.ToString("#.####"));
                    }
                    while (true) // DW_FROM_FLOAT
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "DW_FROM_FLOAT"); // байты без знака
                        if (result.Length != 2) break;
                        uint CValue = 0;
                        try
                        {
                            if (Convert.ToDouble(result[1]) != 0)
                            {
                                byte[] CBytes = BitConverter.GetBytes((float)Convert.ToDouble(result[1]));
                                CValue = BitConverter.ToUInt32(CBytes, 0);
                            }
                        }
                        catch
                        {

                        }
                        address = address.Replace(result[0], CValue.ToString("#.####"));
                    }

                    // Длина
                    while (true) // LEN
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "LEN");
                        if (result.Length != 2) break;
                        int CValue = result[1].Length;
                        address = address.Replace(result[0], Convert.ToString(CValue));
                    }

                    // Работа с аналоговым сигналом (массив)
                    while (true) // HL_AI (Value, Good, LoLo, Lo, Hi, HiHi)
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "HL_AI");
                        if (result.Length != 7) break;
                        float Value = Convert.ToSingle(Calculation(result[1], eDataType.Float)); // аналоговое значение
                        bool Good = Convert.ToBoolean(Calculation(result[2], eDataType.Bool)); // качество тега
                        float LoLo = Convert.ToSingle(Calculation(result[3], eDataType.Float)); // уставка LoLo
                        float Lo = Convert.ToSingle(Calculation(result[4], eDataType.Float)); // уставка Lo
                        float Hi = Convert.ToSingle(Calculation(result[5], eDataType.Float)); // уставка Hi
                        float HiHi = Convert.ToSingle(Calculation(result[6], eDataType.Float)); // уставка HiHi

                        bool[] status = new bool[7];
                        status[0] = Good;
                        status[1] = Good && Value <= LoLo;
                        status[4] = Good && Value >= HiHi;
                        status[2] = Good && status[1] == false && Value <= Lo;
                        status[3] = Good && status[4] == false && Value >= Hi;
                        status[5] = Good && (status[1] || status[4]); // LoLo, HiHi
                        status[6] = Good && (status[2] || status[3]); // Lo, Hi

                        address = address.Replace(result[0], String.Join(";", status));
                    }

                    // Работа с аналоговым сигналом (число)
                    while (true) // STATUS_AI (Value, Good, LoLo, Lo, Hi, HiHi)
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "STATUS_AI");
                        if (result.Length != 7) break;
                        float Value = Convert.ToSingle(Calculation(result[1], eDataType.Float)); // аналоговое значение
                        bool Good = Convert.ToBoolean(Calculation(result[2], eDataType.Bool)); // качество тега
                        float LoLo = Convert.ToSingle(Calculation(result[3], eDataType.Float)); // уставка LoLo
                        float Lo = Convert.ToSingle(Calculation(result[4], eDataType.Float)); // уставка Lo
                        float Hi = Convert.ToSingle(Calculation(result[5], eDataType.Float)); // уставка Hi
                        float HiHi = Convert.ToSingle(Calculation(result[6], eDataType.Float)); // уставка HiHi

                        byte status = 0;
                        if (Good == false)
                        {
                            status = 1; // good = false
                        }
                        else if (Value <= LoLo)
                        {
                            status = 2; // LoLo
                        }
                        else if (Value >= HiHi)
                        {
                            status = 3; // HiHi
                        }
                        else if (Value <= Lo)
                        {
                            status = 4; // Lo
                        }
                        else if (Value >= Hi)
                        {
                            status = 5; // Hi
                        }

                        address = address.Replace(result[0], status.ToString());
                    }

                    // Только статусы сигналом (число)
                    while (true) // STATUS_5 (Good, LoLo, Lo, Hi, HiHi)
                    {
                        string[] result = StringUnpack.Parse_ParamsString(address, "STATUS_5");
                        if (result.Length != 6) break;
                        bool Good = Convert.ToBoolean(Calculation(result[1], eDataType.Bool)); // качество тега
                        bool LoLo = Convert.ToBoolean(Calculation(result[2], eDataType.Bool)); // LoLo
                        bool Lo = Convert.ToBoolean(Calculation(result[3], eDataType.Bool)); // Lo
                        bool Hi = Convert.ToBoolean(Calculation(result[4], eDataType.Bool)); // Hi
                        bool HiHi = Convert.ToBoolean(Calculation(result[5], eDataType.Bool)); // HiHi

                        byte status = 0;
                        if (Good == false)
                        {
                            status = 1; // good = false
                        }
                        else if (LoLo)
                        {
                            status = 2; // LoLo
                        }
                        else if (HiHi)
                        {
                            status = 3; // HiHi
                        }
                        else if (Lo)
                        {
                            status = 4; // Lo
                        }
                        else if (Hi)
                        {
                            status = 5; // Hi
                        }

                        address = address.Replace(result[0], status.ToString());
                    }


                } while (prevAddress != address && String.IsNullOrWhiteSpace(address) == false); // если адрес изменился и существует, то начинаем сначала

            }

            // >>> Преобразование к типу данных
            if (DataType == eDataType.STRING)
            {
                return address;
            }
            else
            {
                dynamic Value = 0;

                // Заменяем операторы логики на символы
                address = " " + address + " ";
                address = Regex.Replace(address, " true ", " 1 ", RegexOptions.IgnoreCase);
                address = Regex.Replace(address, " false ", " 0 ", RegexOptions.IgnoreCase);
                address = Regex.Replace(address, " and ", " & ", RegexOptions.IgnoreCase);
                address = Regex.Replace(address, " or ", " | ", RegexOptions.IgnoreCase);
                address = Regex.Replace(address, " not ", " 0! ", RegexOptions.IgnoreCase);
                address = address.Trim();

                if (String.IsNullOrWhiteSpace(address)) //Если ничего не осталось
                {
                    Value = 0;
                }
                else // Разбор выражения...
                {
                    bool Number = double.TryParse(address, out double valueFromString);
                    if (Number) // если можно конвертировать число
                    {
                        Value = valueFromString; // число
                    }
                    else // если требуются дальнейшие вычисления
                    {
                        try
                        {
                            var mString = (address[0] == '-') ? address.Substring(1) : address;
                            var matchPos = mString.IndexOfAny(StringFormula2.operators.Select(x => Convert.ToChar(x)).ToArray());
                            if (matchPos >= 0)
                            { // если строка содержит операторы
                                Value = client.Calc(address);
                            }
                            else
                            {
                                Value = address;
                            }
                        }
                        catch
                        {
                            Value = 0;
                        }
                    }

                }
                return Tag.ConvertValue(Value, DataType);
            }

        }

        #endregion

    }
}
