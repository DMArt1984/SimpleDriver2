using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using WindowsFormsIDevice;
using System.Windows.Forms;

namespace WinSimpleIDriver.Log
{
    //
    public enum eLogCategory
    {
        Unknown,
        App,
        Exeption,
        User,
        Error,
        Traffic
    }

    public struct LogMessage
    {
        public DateTime DT;
        public eLogCategory category;
        public string description;
        public LogMessage(DateTime DT, eLogCategory category, string description)
        {
            this.DT = DT;
            this.category = category;
            this.description = description;
        }
    }

    public class LogHelper
    {
        private static object syncFile = new object();
        private static object syncDGV = new object();

        static public DataGridView dgv; // DGV для записи лога

        public static void Log(string message = "", Exception ex = null, bool writeConsole = false)
        {
            try
            {
                // Путь .\\Log
                string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                if (!Directory.Exists(pathToLog))
                    Directory.CreateDirectory(pathToLog); // Создаем директорию, если нужно

                string fileName = Path.Combine(pathToLog, string.Format("{0}_{1:yyyy.MM.dd}.log", "AppDiagnostic", DateTime.Now));

                string fullText = string.Format("[{0:yyyy.MM.dd HH:mm:ss.fff}]", DateTime.Now);
                if (!String.IsNullOrWhiteSpace(message))
                {
                    fullText += " " + message;
                }
                if (ex != null)
                {
                    fullText += string.Format(" {0} [{1}.{2}()] {3}", ex.HResult.ToString("X2"), ex.TargetSite.DeclaringType, ex.TargetSite.Name, ex.Message);
                }

                if (writeConsole)
                {
                    Console.WriteLine(message);  // Пишем в консоль
                }

                lock (syncFile)
                {
                    File.AppendAllText(fileName, fullText + "\r\n", Encoding.GetEncoding("Windows-1251")); // Пишем в файл
                }
            }
            catch
            {
                // Перехватываем все и ничего не делаем
            }
        }

        public static void DGV(eLogCategory category, string message)
        {
            if (dgv == null)
                return;

            if (dgv is DataGridView == false)
                return;

            lock (syncDGV)
            {
                try
                {
                    dgv.Rows.Add(dgv.Rows.Count + 1, DateTime.Now, category.ToString().ToUpper(), message);
                } catch
                {

                }
            }
        }

        public static void LogApp(string message)
        {
            if (String.IsNullOrWhiteSpace(message))
                return;

            Log($"[{eLogCategory.App.ToString().ToUpper()}] " + message, null, true);
            DGV(eLogCategory.App, message);
        }

        public static void LogUser(string message)
        {
            if (String.IsNullOrWhiteSpace(message))
                return;

            Log($"[{ eLogCategory.User.ToString().ToUpper()}] " + message);
            DGV(eLogCategory.User, message);
        }

        public static void LogError(string message)
        {
            if (String.IsNullOrWhiteSpace(message))
                return;

            Log($"[{ eLogCategory.Error.ToString().ToUpper()}] " + message);
            DGV(eLogCategory.Error, message);
        }

        public static void LogException(Exception ex, string message = "")
        {
            Log($"[{eLogCategory.Exeption.ToString().ToUpper()}] " + message, ex, false);
            DGV(eLogCategory.Exeption, message);
        }

        public static void LogTraffic(string message)
        {
            if (String.IsNullOrWhiteSpace(message))
                return;

            Log($"[{eLogCategory.Traffic.ToString().ToUpper()}] " + message);
        }

        // ------------------------------------------------------------------

        // Чтение диапазона данных из файла
        public static bool ReadRange(out List<LogMessage> messages, string selectedFileName = null, string Start = null, string Stop = null)
        {
            messages = new List<LogMessage>();
            List<string> data = new List<string>();

            // дата и время диапазона
            bool OK1 = DateTime.TryParse(Start, out DateTime dtStart);
            bool OK2 = DateTime.TryParse(Stop, out DateTime dtStop);

            // получаем данные из файла(ов)
            lock (syncFile)
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(selectedFileName)) // если смотрим все файлы
                    {
                        string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                        string[] files = Directory.GetFiles(pathToLog, "*.log");

                        foreach (string oneFileName in files)
                        {
                            string[] parts = oneFileName.Split('_');
                            if (parts.Length == 2 && (OK1 || OK2))
                            {
                                bool OK = DateTime.TryParse(parts[1], out DateTime itDate);
                                if (OK)
                                {
                                    if ((OK1 == true && itDate < dtStart) || (OK2 == true || itDate > dtStop))
                                        continue;
                                }

                            }
                            data.AddRange(File.ReadAllLines(oneFileName, Encoding.Default).ToList());
                        }
                    }
                    else // если смотрим один файл
                    {
                        data.AddRange(File.ReadAllLines(selectedFileName, Encoding.Default).ToList());
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogException(ex, $"LogHelper.ReadRange");
                    return false;
                }
            }

            // перебор данных
            if (OK1 == false && OK2 == false) // нужны все значения
            {
                foreach (var item in data)
                {
                    List<string> parameters = JsonControl.GetBetweenString(item, "[", "]");
                    if (parameters.Count() == 3) // если есть Дата Категория Сообщение
                    {
                        bool OK = DateTime.TryParse(parameters[0], out DateTime itDate);
                        if (OK)
                        {
                            bool catOK = eLogCategory.TryParse(parameters[1], true, out eLogCategory category);
                            messages.Add(new LogMessage(itDate, category, parameters[2].Trim())); // добавляем сообщение
                        }

                    }
                }
            }
            else // нужен диапазон значений
            {
                foreach (var item in data)
                {
                    List<string> parameters = JsonControl.GetBetweenString(item, "[", "]");
                    if (parameters.Count() == 3) // если есть Дата Категория Сообщение
                    {
                        bool OK = DateTime.TryParse(parameters[0], out DateTime itDate);
                        if (OK && (OK1 == false || itDate >= dtStart) && (OK2 == false || itDate <= dtStop))
                        {
                            bool catOK = eLogCategory.TryParse(parameters[1], true, out eLogCategory category);
                            messages.Add(new LogMessage(itDate, category, parameters[2].Trim())); // добавляем сообщение
                        }

                    }
                }
            }
            return true;
        }


    }
}
