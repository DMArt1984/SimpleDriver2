using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;
using System.IO;
using System.Windows.Forms;
using DML.Log;
using WinSimpleIDriver.Connector;


namespace DML
{
    class JsonControl
    {
        const int version = 1000; // Версия

        public static readonly ILogger logger;

        static JsonControl()
        {
            // Получаем логгер на основе нужного лог-таргета
            logger = BaseLogger.GetLogger(LogTarget.FileConsoleForm);
        }

        #region Convert From/To Json
        // Строка в Json данные
        static public dynamic Deserialize_Json_Data(string input = "")
        {
            dynamic output = null;
            if (String.IsNullOrWhiteSpace(input))
                return output;
            try
            {
                var expConverter = new ExpandoObjectConverter();
                output = JsonConvert.DeserializeObject<ExpandoObject>(input, expConverter);
            }
            catch (Exception ex)
            {
                logger.Error(ex.HResult, $"Ошибка распознования json строки: {input}: {ex.Message}", eMessageCategory.Json);
                MessageBox.Show($"Ошибка распознования json строки: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return output;
        }

        // Json данные в строку
        static public string Serialize_Json_Data(dynamic my_params)
        {
            // Serialize
            string json = "";
            if (my_params == null)
                return json;
            try
            {
                json = JsonConvert.SerializeObject(my_params, Formatting.Indented);
            }
            catch (Exception ex)
            {
                logger.Error(ex.HResult, $"Ошибка получения json строки: {my_params}: {ex.Message}", eMessageCategory.Json);
                MessageBox.Show($"Ошибка получения json строки: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return json;
        }
        #endregion

        // Проверить наличие свойства
        static public bool IsProp(dynamic obj, string prop)
        {
            return (obj != null && !String.IsNullOrEmpty(prop)) ? ((IDictionary<String, object>)obj).ContainsKey(prop) : false;
        }

        // Получить массив строковых значений
        static public List<string> GetTags(dynamic obj)
        {
            List<string> myArray = new List<string> { };
            if (IsProp(obj, "Tags"))
            {
                foreach (dynamic item in obj.Tags)
                {
                    myArray.Add(Convert.ToString(item));
                }
            }
            return myArray;
        }

        // Получить объект со свойством Json
        static public ExpandoObject GetJson(dynamic obj, ExpandoObject defaultValue = null)
        {
            if (JsonControl.IsProp(obj, "Json"))
            {
                return obj.Json;
            }
            else
            {
                return null;
            }
        }

        // Получить значение типа источника
        static public eDriverType GetTypeDriver(dynamic obj, string prop, eDriverType defaultValue)
        {
            if (IsProp(obj, prop))
            {
                try
                {
                    string stringValue = Convert.ToString(((IDictionary<string, object>)obj)[prop]);
                    return (eDriverType)System.Enum.Parse(typeof(eDriverType), stringValue, true);
                }
                catch
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }

        // Получить значение типа перечисления
        static public T GetTypeEnum<T>(dynamic obj, string prop, T defaultValue)
        {
            if (IsProp(obj, prop))
            {
                try
                {
                    string stringValue = Convert.ToString(((IDictionary<string, object>)obj)[prop]);
                    return (T)System.Enum.Parse(typeof(T), stringValue, true);
                }
                catch
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }

        // Получить список параметров ограниченных символами
        public static List<string> GetBetweenString(string text, string left, string right)
        {
            List<string> result = new List<string>();

            if (String.IsNullOrWhiteSpace(text))
                return result;

            if (String.IsNullOrWhiteSpace(left))
                return result;

            if (String.IsNullOrWhiteSpace(right))
                right = left;

            bool next;

            do
            {
                next = false;
                if (text.Length == 0)
                {

                }
                else
                {
                    var indexStart = text.IndexOf(left, 0); // [
                    if (indexStart > 0) // abc[de
                    {
                        result.Add(text.Substring(0, indexStart)); // abc
                        text = text.Substring(indexStart - 1 + left.Length); // [de
                        next = true;
                    }
                    else if (indexStart == 0) // [de
                    {
                        var indexStop = text.IndexOf(right, 1); // ]
                        if (indexStop == text.Length - 1) // [abcde]
                        {
                            result.Add(text.Substring(1, text.Length - 2)); // 
                            text = "";
                        }
                        else if (indexStop == -1) // [
                        {
                            result.Add(text.Substring(1));
                            text = "";
                        }
                        else if (indexStop == 1) // []
                        {
                            text = "";
                        }
                        else // [abcd]ef
                        {
                            result.Add(text.Substring(1, indexStop - 1));
                            text = text.Substring(indexStop + right.Length);
                            next = true;
                        }
                    }
                    else // abc
                    {
                        result.Add(text);
                        text = "";
                    }
                }

            } while (next == true);

            result.RemoveAll(x => String.IsNullOrWhiteSpace(x));
            return result;
        }

        // Получить список строк
        static public string[] GetArrayString(dynamic obj, string prop)
        {
            if (IsProp(obj, prop))
            {
                try
                {
                    var data = (List<object>)((IDictionary<string, object>)obj)[prop];
                    return data.Select(x => x.ToString().Trim()).Where(y => String.IsNullOrWhiteSpace(y) == false).ToArray();
                } catch
                {
                    return new string[] { };
                }
            }
            return new string[] { };
        }

        #region Get Bool

        // Получить логическое значение
        static public bool GetBool(dynamic obj, string prop, bool defaultValue = false)
        {
            if (IsProp(obj, prop))
            {
                string stringValue = Convert.ToString(((IDictionary<string, object>)obj)[prop]).ToLower();
                bool result = bool.TryParse(stringValue, out bool value);
                if (result)
                    return value;

                return !(String.IsNullOrWhiteSpace(stringValue) || stringValue.ToLower() == "false" || stringValue == "0");
            }
            return defaultValue;
        }
        static public bool GetBool(dynamic obj)
        {
            if (obj == null)
                return false;
            string stringValue = Convert.ToString(obj).ToLower();
            bool result = bool.TryParse(stringValue, out bool value);
            if (result)
                return value;

            return !(String.IsNullOrWhiteSpace(stringValue) || stringValue.ToLower() == "false" || stringValue == "0");
        }

        #endregion

        #region Get Byte

        // Получить числовое значение, ограниченное 0 - 100
        static public byte GetByte100(dynamic obj, string prop, byte defaultValue = 0)
        {
            byte value = GetByte(obj, prop, defaultValue);
            if (value < 0)
                value = 0;
            if (value > 100)
                value = 100;
            return value;
        }

        // Получить числовое значение (byte)
        static public byte GetByte(dynamic obj, string prop, byte defaultValue = 0)
        {
            if (IsProp(obj, prop))
            {
                string stringValue = Convert.ToString(((IDictionary<string, object>)obj)[prop]);
                bool result = byte.TryParse(stringValue, out byte value);
                if (result)
                    return value;
            }
            return defaultValue;
        }

        #endregion

        #region Get Number

        // Получить числовое значение (short)
        static public short GetShort(dynamic obj, string prop, short defaultValue = 0)
        {
            if (IsProp(obj, prop))
            {
                string stringValue = Convert.ToString(((IDictionary<string, object>)obj)[prop]);
                bool result = short.TryParse(stringValue, out short value);
                if (result)
                    return value;
            }
            return defaultValue;
        }

        static public short GetShort(dynamic obj)
        {
            string stringValue = Convert.ToString(obj);
            bool result = short.TryParse(stringValue, out short value);
            if (result)
                return value;

            return 0;
        }

        static public ushort GetUShort(dynamic obj)
        {
            string stringValue = Convert.ToString(obj);
            bool result = ushort.TryParse(stringValue, out ushort value);
            if (result)
                return value;

            return 0;
        }

        // Получить числовое значение (int)
        static public int GetInt(dynamic obj, string prop, int defaultValue = 0)
        {
            if (IsProp(obj, prop))
            {
                string stringValue = Convert.ToString(((IDictionary<string, object>)obj)[prop]);
                bool result = int.TryParse(stringValue, out int value);
                if (result)
                    return value;
            }
            return defaultValue;
        }

        static public int GetInt(dynamic obj)
        {
            string stringValue = Convert.ToString(obj);
            bool result = int.TryParse(stringValue, out int value);
            if (result)
                return value;

            return 0;
        }

        // Получить числовое значение (uint)
        static public uint GetUint(dynamic obj, string prop, uint defaultValue = 0)
        {
            if (IsProp(obj, prop))
            {
                string stringValue = Convert.ToString(((IDictionary<string, object>)obj)[prop]);
                bool result = uint.TryParse(stringValue, out uint value);
                if (result)
                    return value;
            }
            return defaultValue;
        }

        static public uint GetUint(dynamic obj)
        {
            string stringValue = Convert.ToString(obj);
            bool result = uint.TryParse(stringValue, out uint value);
            if (result)
                return value;

            return 0;
        }

        // Получить числовое значение (long)
        static public long GetLong(dynamic obj, string prop, long defaultValue = 0)
        {
            if (IsProp(obj, prop))
            {
                string stringValue = Convert.ToString(((IDictionary<string, object>)obj)[prop]);
                bool result = long.TryParse(stringValue, out long value);
                if (result)
                    return value;
            }
            return defaultValue;
        }
        static public long GetLong(dynamic obj)
        {
            string stringValue = Convert.ToString(obj);
            bool result = long.TryParse(stringValue, out long value);
            if (result)
                return value;

            return 0;
        }

        #endregion

        #region Get String

        // Получить строковое значение
        static public string GetString(dynamic obj, string prop, string defaultValue = "")
        {
            if (IsProp(obj, prop))
            {
                string value = Convert.ToString(((IDictionary<string, object>)obj)[prop]);
                if (!String.IsNullOrWhiteSpace(value))
                    return value;
            }
            return defaultValue;
        }

        static public string GetString(dynamic obj)
        {
            string value = Convert.ToString(obj);
            if (!String.IsNullOrWhiteSpace(value))
                return value;

            return "";
        }

        #endregion



    }
}
