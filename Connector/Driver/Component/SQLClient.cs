using Connector.Driver.Component;
using LogCodeMessage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Connector.Driver
{
    public enum eSQLStatus
    {
        created = 100, // подключение создано
        connected = 101, // подключено
        open = 102, // открыто
        close = 103, // закрыто
        errOpen = -101, // ошибка открытия
        disconnected = 201, // отключено
        errClose = -201, // ошибка закрытия
        noClient = -1, // нет клиента
    }
    public static class eSQLStatusExtensions
    {
        public static string GetText(this eSQLStatus status)
        {
            switch (status)
            {
                case eSQLStatus.created:
                    return "Подключение создано";
                case eSQLStatus.connected:
                    return "Подключено";
                case eSQLStatus.open:
                    return "Открыто";
                case eSQLStatus.errOpen:
                    return "Ошибка открытия";
                case eSQLStatus.disconnected:
                    return "Отключено";
                case eSQLStatus.close:
                    return "Закрыто";
                case eSQLStatus.errClose:
                    return "Ошибка закрытия";
                default:
                    return status.ToString();
            }
        }
    }

    class MSSQLclient : IDisposable
    {
        public const int ver = 1057; // номер версии
        public const string defaultDataType = "[nchar](10) NULL";

        CodeMessage cmEmptyConnect = new CodeMessage(-998, "Пустая строка подключения в БД");
        CodeMessage cmErrorConnect = new CodeMessage(-999, "Ошибка подключения в БД (null)");

        public string formatDateTime = "yyyy-MM-dd HH:mm:ss.fff";
        public string floatPoint = "."; // Точка или запятая для отображения дробных чисел (500.123 или 500,123)

        // SQL подключение
        private SqlConnection client;

        private string connectionString;

        // Флаг для отслеживания освобождения ресурсов
        private bool disposed = false;

        public MSSQLclient(string connectionString = "")
        {
            this.connectionString = connectionString;
        }

        // Реализация IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Освобождаем управляемые ресурсы
                    if (client != null)
                    {
                        try
                        {
                            if (client.State != ConnectionState.Closed)
                                client.Close();
                        }
                        catch
                        {
                            // обработка исключения по необходимости
                        }
                        client.Dispose();
                        client = null;
                    }
                }
                // Освобождение неуправляемых ресурсов (если есть)
                disposed = true;
            }
        }

        ~MSSQLclient()
        {
            Dispose(false);
        }

        // ---------------------------------------------------------------------------------------------
        // Подключение
        public CodeMessage Connect(string newConnectionString = "")
        {
            try
            {
                if (String.IsNullOrWhiteSpace(newConnectionString) == false)
                    connectionString = newConnectionString;

                if (client != null)
                    client.Dispose();

                client = new SqlConnection(connectionString);

                if (client == null)
                    return cmErrorConnect;

                return CodeMessageFactory.FromEnumX(eSQLStatus.connected);
            }
            catch (Exception ex)
            {
                return CodeMessageFactory.FromException(ex);
            }
        }

        // ----------------------------------------------------------------------------------------------
        public CodeMessage Open()
        {
            try
            {
                if (client == null)
                    return CodeMessageFactory.FromEnumX(eSQLStatus.noClient);

                client.Open();
                return client.State == ConnectionState.Open ? CodeMessageFactory.FromEnumX(eSQLStatus.open) : CodeMessageFactory.FromEnumX(eSQLStatus.errOpen);
            }
            catch (Exception ex)
            {
                return CodeMessageFactory.FromException(ex);
            }
        }

        public CodeMessage Close()
        {
            try
            {
                if (client == null)
                    return CodeMessageFactory.FromEnumX(eSQLStatus.noClient);

                client.Close();
                return client.State == ConnectionState.Closed ? CodeMessageFactory.FromEnumX(eSQLStatus.close) : CodeMessageFactory.FromEnumX(eSQLStatus.errClose);
            }
            catch (Exception ex)
            {
                return CodeMessageFactory.FromException(ex);
            }
        }

        public void Exit()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
        }

        // ----------------------------------------------------------------------------------------------

        public SqlConnection GetConnection()
        {
            if (client == null)
                throw new InvalidOperationException("SQL Connection is not initialized.");
            return client;
        }

        // ----------------------------------------------------------------------------------

        #region ExistTable

        // Проверка наличия таблицы
        public bool ExistTable(string table, out CodeMessage message, string schema = "dbo")
        {
            string SQL = $"IF EXISTS ( SELECT 1 FROM information_schema.tables WHERE table_schema = '{schema}' AND table_name = '{table}') SELECT 'Y' ELSE SELECT 'N'; ";
            message = GetTable(out List<List<string>> rows, SQL);
            return CheckOneValue(rows, "Y");
        }

        public bool ExistTable(out CodeMessage message, string partRequest)
        {
            string SQL = $"IF EXISTS ( {partRequest} ) SELECT 'Y' ELSE SELECT 'N'; ";
            message = GetTable(out List<List<string>> rows, SQL);
            return CheckOneValue(rows, "Y");
        }

        // Сравнить первое значение
        static private bool CheckOneValue(List<List<string>> rows, string value)
        {
            return GetOneValue(rows) == value;
        }
        // Получить первое значение
        static private string GetOneValue(List<List<string>> rows, string def = "")
        {
            return (rows.Count >= 1 && rows[0].Count >= 1) ? rows[0][0] : def;
        }

        #endregion

        // ----------------------------------------------------------------------------------

        #region Request

        // Выполнение запроса для получения данных
        public CodeMessage GetTable(out List<List<string>> rows, string SQL)
        {
            rows = new List<List<string>>();
            if (String.IsNullOrWhiteSpace(SQL))
            {
                return Tag.CM.EmptyRequest;
            }

            CodeMessage cm;
            bool weOpenedConnection = false;

            try
            {
                if (client.State == ConnectionState.Closed)
                {
                    cm = Open();
                    if (cm.code != (int)eSQLStatus.open)
                        return cm;

                    weOpenedConnection = true; // помним, что именно мы открыли соединение
                }

                using (SqlCommand cmd = new SqlCommand(SQL, client))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var cols = new List<string>();
                                for (var i = 0; i < reader.FieldCount; i++)
                                {
                                    var value = reader.GetValue(i);
                                    string StringValue = "";
                                    if ((value is DateTime) && !String.IsNullOrWhiteSpace(formatDateTime))
                                    {
                                        StringValue = String.Format("{0:" + formatDateTime + "}", value).Trim();
                                    }
                                    else
                                    {
                                        StringValue = value.ToString();
                                    }
                                    cols.Add(StringValue);
                                }
                                rows.Add(cols);
                            }
                        }
                    }
                }

                if (weOpenedConnection)
                {
                    cm = Close();
                    if (cm.code != (int)eSQLStatus.close)
                        return cm;
                }

                return Tag.CM.Good;
            }
            catch (SqlException ex)
            {
                return new CodeMessage(ex.HResult, $"[{ex.Number}] {ex.Message}");
            }
            catch (Exception ex)
            {
                return CodeMessageFactory.FromException(ex);
            }
        }

        // Выполнение запроса с командой
        public CodeMessage SendCommand(out int rows, string SQL)
        {
            rows = 0;
            if (String.IsNullOrWhiteSpace(SQL))
            {
                return Tag.CM.EmptyRequest;
            }

            CodeMessage cm;
            bool weOpenedConnection = false;

            try
            {
                if (client.State == ConnectionState.Closed)
                {
                    cm = Open();
                    if (cm.code != (int)eSQLStatus.open)
                        return cm;

                    weOpenedConnection = true; // помним, что именно мы открыли соединение
                }

                using (SqlCommand cmd = client.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = SQL;
                    rows = cmd.ExecuteNonQuery();
                }

                if (weOpenedConnection)
                {
                    cm = Close();
                    if (cm.code != (int)eSQLStatus.close)
                        return cm;
                }

                return Tag.CM.Good;
            }
            catch (SqlException ex)
            {
                return new CodeMessage(ex.HResult, $"[{ex.Number}] {ex.Message}");
            }
            catch (Exception ex)
            {
                return CodeMessageFactory.FromException(ex);
            }
        }

        #endregion

        // ----------------------------------------------------------------------------------

        #region LIB

        // NEW: Поиск IP
        static string FindIP(string connectionString, string defaultIP = "")
        {
            var match = Regex.Match(connectionString, @"(?:server|data source)\s*=\s*([^\\;]+)", RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value.Trim() : defaultIP;
        }

        // NEW: Поиск timeout
        static int FindTimeout(string connectionString, int defaultTimeout = 100)
        {
            var match = Regex.Match(connectionString, @"\btimeout\s*=\s*(\d+)", RegexOptions.IgnoreCase);
            return match.Success && int.TryParse(match.Groups[1].Value, out int timeout) ? timeout : defaultTimeout;
        }

        // -----------------------------------------------------------------------------------
        // Проверка, является ли тип данных строковым
        static private bool DataTypeIsString(string dataType)
        {
            return dataType.Contains("char") || dataType.Contains("text");
        }

        // Проверка, является ли тип данных дискретным
        static private bool DataTypeIsBool(string dataType)
        {
            return dataType.Contains("bit") || dataType.Contains("bool");
        }

        // Получить тип данных из списка по индексу
        static public string GetDatatTypeFromList(string[] dataTypes, int index = 0)
        {
            return (dataTypes.Length > index) ? dataTypes[index] : defaultDataType;
        }

        // -----------------------------------------------------------------------------------
        // Конвертировать значение в строку (для INSERT)
        public string ValueToString(string value, string dataType)
        {
            return DataTypeIsString(dataType)
                ? $"'{value}'"
                : DataTypeIsBool(dataType)
                    ? SimpleConverter.StringToBoolString(value)
                    : $"{value.Replace(",", floatPoint).Replace(".", floatPoint)}";
        }
        
        #endregion

    }
}

