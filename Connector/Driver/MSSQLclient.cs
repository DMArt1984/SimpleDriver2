using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsIDevice.Connector.SGT;

namespace WindowsFormsIDevice.Connector.Driver
{
    class MSSQLclient : DeviceNet
    {
        public const string driverName = "Microsoft SQL Client";

        // Справка
        public static Dictionary<string, string> GetHelpSource()
        {
            return new Dictionary<string, string> {
                        { "Пример 1",  "user id=DM;" +
                                       "password=65536;server=WIN-6D9BE2IKJQB\\SQLEXPRESS;" +
                                       "Trusted_Connection=yes;" +
                                       "database=MSDataBase1; " +
                                       "connection timeout=30" },
                        { "Пример 2", "Data Source=localhost\\SQLEXPRESS01;Initial Catalog=dbLkzPremix_ARH;User Id = Tech; Password = 123456"}
                    };
        } // Описание адреса устройства

        public static Dictionary<string, string> GetHelpTag()
        {
            return new Dictionary<string, string> {
                        { "Чтение данных", "SELECT COUNT(*) FROM DRIVE" },
                        { "Обновление данных", "UPDATE DRIVE SET Text = 'Changed' WHERE ID=1" }
                    };
        } // Описание адреса тега для данного устройства

        // SQL подключение
        public SqlConnection client;

        private Dictionary<string, bool> fronts = new Dictionary<string, bool>(); // фронт для записи тегов один раз

        // Настройки клиента
        // ...

        public MSSQLclient()
        {
            disableHostForOpen = true;
            //...
        }

        ~MSSQLclient()
        {
            try
            {
                fronts = new Dictionary<string, bool>();
                client?.Close();
                client = null;
            } catch
            {

            }
        }

        // -----------------------------------------------------------------------------

        // Создание подключения
        public override CodeMessage CreateClient(string connectionString)
        {
            try
            {
                client = new SqlConnection(connectionString);
                IP = FindIP(connectionString, IP);
                timeout = FindTimeout(connectionString, timeout);
                return new CodeMessage(0,"");
            }
            catch (Exception ex)
            {
                return new CodeMessage(ex.HResult, ex.Message);
            }
        }

        // ---------------------------------------------------------------------------------------------

        public override CodeMessage Connect(string connectionString = "")
        {
            try
            {
                if (String.IsNullOrWhiteSpace(connectionString) == false)
                    client = new SqlConnection(connectionString);

                IP = FindIP(connectionString, IP);
                timeout = FindTimeout(connectionString, timeout);
                client.Open();

                return client.State == System.Data.ConnectionState.Open ? new CodeMessage(0,"") : new CodeMessage(-56, "Ошибка открытия");
            }
            catch (Exception ex)
            {
                return new CodeMessage(ex.HResult, ex.Message);
            }
        }

        //-------------------------------------------------------

        private string FindIP (string connectionString, string defaultIP = "")
        {
            string[] parts1 = connectionString.Split( new string[] { "server=" }, StringSplitOptions.None);
            if (parts1.Length < 2)
                parts1 = connectionString.Split(new string[] { "Data Source=" }, StringSplitOptions.None);
            if (parts1.Length < 2)
                return defaultIP;

            string[] parts2 = parts1[1].Split('\\');
            if (parts2.Length < 2)
                return defaultIP;

            return parts2[0];
        }

        private int FindTimeout(string connectionString, int defaultTimeout = 100)
        {
            string[] parts1 = connectionString.Split(new string[] { " timeout=" }, StringSplitOptions.None);
            if (parts1.Length < 2)
                parts1 = connectionString.Split(new string[] { ";timeout=" }, StringSplitOptions.None);
            if (parts1.Length < 2)
                return defaultTimeout;

            string[] parts2a = parts1[1].Split(';');
            string[] parts2b = parts1[1].Split(' ');

            if (parts2a.Length == 2)
                return Convert.ToInt32(parts2a[0]);

            if (parts2b.Length == 2)
                return Convert.ToInt32(parts2b[0]);

            if (parts2a.Length == 1)
                return Convert.ToInt32(parts2a[0]);

            return Convert.ToInt32(parts2b[0]);
        }

        // -------------------------------------------------------
        public override CodeMessage Disconnect()
        {
            try
            {
                if (client == null)
                    return new CodeMessage(eSourceStatus.noClient);

                fronts = new Dictionary<string, bool>();
                client.Close();
                return client.State == System.Data.ConnectionState.Closed ? new CodeMessage(0, "") : new CodeMessage(eSourceStatus.errClose);
            }
            catch (Exception ex)
            {
                return new CodeMessage(ex.HResult, ex.Message);
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
                string[] CN = address.Split(';');
                string SQL = (CN.Length >= 1) ? CN[0] : ""; // SQL-запрос
                int Column = (CN.Length >= 2) ? ((!String.IsNullOrWhiteSpace(CN[1])) ? int.Parse(CN[1]) : 0) : 0; // номер столбца из запроса
                int Row = (CN.Length >= 3) ? ((!String.IsNullOrWhiteSpace(CN[2])) ? int.Parse(CN[2]) : 0) : 0; // номер строки из запроса
                int Item = 0;
                SqlCommand cmd = new SqlCommand(SQL, client);

                bool arrayDelimer = false;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows) // есть данные в ответе
                    {
                        var arrValue = new List<dynamic>();

                        while (reader.Read())
                        {
                            Item++;
                            if (Row == 0 || Row == Item)
                            {
                                if (true || Column < reader.FieldCount)
                                {
                                    if (reader.FieldCount == 1) // один столбец
                                    {
                                        
                                        switch (DataType)
                                        {
                                            case eDataType.Bool:
                                                Value = reader.GetBoolean(Column);
                                                break;
                                            case eDataType.Byte:
                                                Value = reader.GetByte(Column);
                                                break;
                                            case eDataType.Char:
                                                Value = reader.GetChar(Column);
                                                break;
                                            case eDataType.Short:
                                            case eDataType.UShort:
                                                Value = reader.GetInt16(Column);
                                                break;
                                            case eDataType.Int:
                                            case eDataType.UInt:
                                                Value = reader.GetInt32(Column);
                                                break;
                                            case eDataType.Float:
                                                Value = reader.GetFloat(Column);
                                                break;
                                            case eDataType.Double:
                                                Value = reader.GetDouble(Column);
                                                break;
                                            case eDataType.STRING:
                                                Value = reader.GetString(Column);
                                                break;
                                        }
                                    }
                                    else
                                    { // все столбцы
                                        Value = "";
                                        for (int j = 0; j < reader.FieldCount; j++)
                                        {
                                            if (j > 0)
                                            {
                                                Value += ";";
                                                arrayDelimer = true;
                                            }
                                            Value += (reader.GetValue(j).ToString());
                                        }
                                    }

                                }
                                else
                                {
                                    Value = null;
                                }
                            }
                            arrValue.Add(Value);
                        }
                        
                        if (arrValue.Count() == 1 && arrayDelimer)
                        {
                            Value = Convert.ToString(arrValue[0]).Split(';');
                        } else
                        {
                            Value = arrValue;
                        }

                    } else // нет данных в ответе
                    {
                        Value = 1;
                    }
                }

                // массив и одиночное значение
                OneArrayToValue(ref Value);

                // Вернуть тег
                return new TagResult(Value, eTagCode.good);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 0)
                {
                    Log.LogHelper.LogError($"Ошибка связи с SQL сервером: {ex.HResult} [{ex.Number}] {ex.Message}");
                    return new TagResult(0, (int)eTagCode.breakError, $"{eTagCode.breakError.GetText()} ={ex.HResult} [{ex.Number}] {ex.Message}");
                }
                return new TagResult(Value, ex.HResult, $"[{ex.Number}] {ex.Message}");
            }
            catch (Exception ex)
            {
                if (ex.HResult.ToString("X") == "80131904" || ex.HResult.ToString("X") == "FFFFFDA8") // ошибка сервера?
                {
                    Log.LogHelper.LogError($"Ошибка SQL сервера: {ex.HResult} {ex.Message}");
                    return new TagResult(0, (int)eTagCode.breakError, $"{eTagCode.breakError.GetText()} ={ex.HResult} {ex.Message}");
                }
                return new TagResult(Value, ex.HResult, ex.Message);
            }

        }

        // Выполнение запроса если новое значение TRUE
        public override TagResult SetValue(string address, eDataType DataType, dynamic newValue = null)
        {
            if (newValue == null)
                return new TagResult(newValue, eTagCode.newValueIsNull);

            // добавляем адрес в список
            if (fronts.ContainsKey(address) == false)
                fronts.Add(address, false);

            // newValue
            if (Tag.ConvertValue(newValue, eDataType.Bool) == false)
            {
                fronts[address] = false;
                return new TagResult(newValue, eTagCode.good);
            }
            else
            {
                // записываем по положительному фронту
                if (fronts[address] == false)
                {
                    fronts[address] = true;
                    var result2 = GetValue(address, DataType);
                    return result2;
                } else
                {
                    return new TagResult(newValue, eTagCode.good);
                }
            }

            // записываем постоянно от newValue=true
            var result = GetValue(address, DataType);
            return result;
        }


    }
}
