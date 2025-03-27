using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DML.Log
{

    /// <summary>
    /// Категории сообщений
    /// </summary>
    public enum eMessageCategory
    {
        none = 0,
        App = 1,
        Exception = 2,
        User = 4,
        Traffic = 8,
        FILE = 16,
        DATA = 32,
        SYNC = 64,
        SQL = 128,
        Source = 256
    }

    /// <summary>
    /// Типы сообщений
    /// </summary>
    public enum eMessageType
    {
        OK = 0,
        INFO = 1,
        ERROR = 2
    }

    /// <summary>
    /// Структура, содержащая код и сообщение
    /// </summary>
    public struct CodeMessage
    {
        public readonly int сode;
        public readonly string message;

        public CodeMessage(int code = 0, string message = "")
        {
            сode = code;
            this.message = message;
        }
    }

    public interface ICodeMessage
    {
        ushort Id { get; }
        CodeMessage codeMessage { get; set; }
        bool Good { get; }
    }

    /// <summary>
    /// Интерфейс логгера.
    /// </summary>
    public interface ILogger
    {
        void OK(string message, eMessageCategory category = eMessageCategory.none);
        void Info(string message, eMessageCategory category = eMessageCategory.none);
        void Error(int code, string message, eMessageCategory category = eMessageCategory.none);
        void CodeMessage(CodeMessage cm, eMessageCategory category = eMessageCategory.none);
        Task CodeMessageAsync(CodeMessage cm, eMessageCategory category = eMessageCategory.none);
    }

    /// <summary>
    /// Возможные варианты направления логирования.
    /// </summary>
    public enum LogTarget
    {
        FileOnly,
        ConsoleOnly,
        FormOnly,
        FileAndConsole,
        FileAndForm,
        FileConsoleForm
    }

    /// <summary>
    /// Вспомогательный класс для определения типа сообщения по коду.
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// Определяет тип сообщения на основе кода.
        /// </summary>
        /// <param name="code">Код сообщения.</param>
        /// <returns>
        /// Info, если код равен 0; Warning, если код больше 0; Error, если код меньше 0.
        /// </returns>
        public static eMessageType GetMessageType(int code)
        {
            return code == 0 ? eMessageType.OK : (code > 0 ? eMessageType.INFO : eMessageType.ERROR);
        }

        static public string Message(Exception ex)
        {
            return $"{ex.HResult}= {ex.Message}";
        }

        static public string GetExceptionMessage(CodeMessage cm)
        {
            return $"{cm.сode}= {cm.message}";
        }

        static public string TypeMessage(eMessageType mtype)
        {
            if (mtype == eMessageType.ERROR)
                return "Ошибка";

            if (mtype == eMessageType.OK)
                return "Выполнено";

            return "";
        }
        static public eMessageType TypeMessage(CodeMessage cm)
        {
            if (cm.сode < 0)
                return eMessageType.ERROR;

            if (cm.сode > 0)
                return eMessageType.INFO;

            return eMessageType.OK;
        }

        #region Form

        // Цвет сообщения
        static public Color GetColorForMessage(eMessageType mtype)
        {
            return (mtype == eMessageType.OK) ? Color.DarkGreen : (mtype == eMessageType.ERROR) ? Color.DarkRed : Color.Black;
        }
        #endregion

    }

    // ==========================================================================

    /// <summary>
    /// Композиционный логгер, который делегирует вызовы методам нескольких логгеров.
    /// Позволяет объединить несколько логгеров в один объект для одновременного логирования.
    /// </summary>
    public class CompositeLogger : ILogger
    {
        /// <summary>
        /// Массив логгеров, которым делегируются вызовы.
        /// </summary>
        private readonly ILogger[] loggers;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CompositeLogger"/> с заданными логгерами.
        /// </summary>
        /// <param name="loggers">Перечень логгеров, которым будут делегироваться вызовы логирования.</param>
        public CompositeLogger(params ILogger[] loggers)
        {
            this.loggers = loggers;
        }

        /// <summary>
        /// Логирует результат, вызывая метод OK у каждого из логгеров.
        /// </summary>
        /// <param name="message">Сообщение для логирования.</param>
        /// <param name="category">Категория лог-сообщения. По умолчанию: <see cref="eMessageCategory.none"/>.</param>
        public void OK(string message, eMessageCategory category = eMessageCategory.none)
        {
            foreach (var logger in loggers)
                logger.OK(message, category);
        }

        /// <summary>
        /// Логирует информационное сообщение, вызывая метод Info у каждого из логгеров.
        /// </summary>
        /// <param name="message">Сообщение для логирования.</param>
        /// <param name="category">Категория лог-сообщения. По умолчанию: <see cref="eMessageCategory.none"/>.</param>
        public void Info(string message, eMessageCategory category = eMessageCategory.none)
        {
            foreach (var logger in loggers)
                logger.Info(message, category);
        }

        /// <summary>
        /// Логирует сообщение об ошибке, вызывая метод Error у каждого из логгеров.
        /// </summary>
        /// <param name="code">Код ошибки.</param>
        /// <param name="message">Сообщение об ошибке.</param>
        /// <param name="category">Категория лог-сообщения. По умолчанию: <see cref="eMessageCategory.none"/>.</param>
        public void Error(int code, string message, eMessageCategory category = eMessageCategory.none)
        {
            foreach (var logger in loggers)
                logger.Error(code, message, category);
        }

        /// <summary>
        /// Логирует сообщение с кодом, вызывая метод CodeMessage у каждого из логгеров.
        /// </summary>
        /// <param name="cm">Структура <see cref="CodeMessage"/>, содержащая код и текст сообщения.</param>
        /// <param name="category">Категория лог-сообщения. По умолчанию: <see cref="eMessageCategory.none"/>.</param>
        public void CodeMessage(CodeMessage cm, eMessageCategory category = eMessageCategory.none)
        {
            foreach (var logger in loggers)
                logger.CodeMessage(cm, category);
        }

        /// <summary>
        /// Асинхронно логирует сообщение с кодом, вызывая асинхронный метод CodeMessageAsync у каждого из логгеров.
        /// </summary>
        /// <param name="cm">Структура <see cref="CodeMessage"/>, содержащая код и текст сообщения.</param>
        /// <param name="category">Категория лог-сообщения. По умолчанию: <see cref="eMessageCategory.none"/>.</param>
        /// <returns>Задача, представляющая асинхронную операцию логирования для всех логгеров.</returns>
        public async Task CodeMessageAsync(CodeMessage cm, eMessageCategory category = eMessageCategory.none)
        {
            Task[] tasks = new Task[loggers.Length];
            for (int i = 0; i < loggers.Length; i++)
                tasks[i] = loggers[i].CodeMessageAsync(cm, category);
            await Task.WhenAll(tasks);
        }
    }

    // ----------------------------------------------------------------------------

    /// <summary>
    /// Логгер, выводящий сообщения в консоль, реализованный как Singleton.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Экземпляр логгера, реализующий паттерн Singleton.
        /// </summary>
        private static readonly Lazy<ConsoleLogger> _instance = new Lazy<ConsoleLogger>(() => new ConsoleLogger());

        /// <summary>
        /// Получает единственный экземпляр класса <see cref="ConsoleLogger"/>.
        /// </summary>
        public static ConsoleLogger Instance => _instance.Value;

        private readonly object _lockObj = new object();

        /// <summary>
        /// Приватный конструктор для предотвращения создания экземпляров извне.
        /// </summary>
        private ConsoleLogger() { }

        /// <summary>
        /// Логирует результат в консоль с зелёным цветом.
        /// </summary>
        /// <param name="message">Сообщение для логирования.</param>
        /// <param name="mcategory">Категория сообщения. По умолчанию: <see cref="eMessageCategory.none"/>.</param>
        public void OK(string message, eMessageCategory mcategory = eMessageCategory.none)
        {
            lock (_lockObj)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{DateTime.Now} [INFO] [{mcategory}] {message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Логирует информационное сообщение в консоль с жёлтым цветом.
        /// </summary>
        /// <param name="message">Сообщение для логирования.</param>
        /// <param name="mcategory">Категория сообщения. По умолчанию: <see cref="eMessageCategory.none"/>.</param>
        public void Info(string message, eMessageCategory mcategory = eMessageCategory.none)
        {
            lock (_lockObj)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{DateTime.Now} [WARNING] [{mcategory}] {message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Логирует сообщение об ошибке в консоль с красным цветом.
        /// </summary>
        /// <param name="code">Код ошибки.</param>
        /// <param name="message">Сообщение об ошибке.</param>
        /// <param name="mcategory">Категория сообщения. По умолчанию: <see cref="eMessageCategory.none"/>.</param>
        public void Error(int code, string message, eMessageCategory mcategory = eMessageCategory.none)
        {
            lock (_lockObj)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{DateTime.Now} [ERROR] [{mcategory}] [{code}] {message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Логирует сообщение с использованием структуры <see cref="CodeMessage"/>,
        /// определяя тип сообщения по коду и вызывая соответствующий метод логирования.
        /// </summary>
        /// <param name="cm">Структура <see cref="CodeMessage"/>, содержащая код и текст сообщения.</param>
        /// <param name="mcategory">Категория сообщения. По умолчанию: <see cref="eMessageCategory.none"/>.</param>
        public void CodeMessage(CodeMessage cm, eMessageCategory mcategory = eMessageCategory.none)
        {
            string formattedMessage = cm.сode != 0 ? $"{cm.сode} {cm.message}" : cm.message;
            eMessageType mt = LogHelper.GetMessageType(cm.сode);
            switch (mt)
            {
                case eMessageType.OK:
                    OK(formattedMessage, mcategory);
                    break;
                case eMessageType.INFO:
                    Info(formattedMessage, mcategory);
                    break;
                case eMessageType.ERROR:
                    Error(cm.сode, formattedMessage, mcategory);
                    break;
            }
        }

        /// <summary>
        /// Асинхронно логирует сообщение, используя структуру <see cref="CodeMessage"/>.
        /// </summary>
        /// <param name="cm">Структура <see cref="CodeMessage"/>, содержащая код и текст сообщения.</param>
        /// <param name="mcategory">Категория сообщения. По умолчанию: <see cref="eMessageCategory.none"/>.</param>
        /// <returns>Задача, представляющая асинхронную операцию логирования.</returns>
        public async Task CodeMessageAsync(CodeMessage cm, eMessageCategory mcategory = eMessageCategory.none)
        {
            await Task.Run(() => CodeMessage(cm, mcategory));
        }
    }

    /// <summary>
    /// Логгер для таблицы формы с использованием делегата для передачи лог-сообщений, реализованный как Singleton.
    /// </summary>
    public class FormLogger : ILogger
    {
        private static readonly Lazy<FormLogger> _instance = new Lazy<FormLogger>(() => new FormLogger());
        public static FormLogger Instance => _instance.Value;

        /// <summary>
        /// Делегат для обработки строки логирования.
        /// Внешняя часть приложения должна присвоить этому делегату свою реализацию.
        /// </summary>
        public delegate void LogDelegate(eMessageType mtype, eMessageCategory mcategory, int code, string message);

        /// <summary>
        /// Делегат, который будет вызван для логирования.
        /// </summary>
        public LogDelegate FormLogDelegate { get; set; }

        private readonly object _lockObj = new object();

        // Приватный конструктор для реализации Singleton.
        private FormLogger() { }

        /// <summary>
        /// Проверяет, следует ли логировать сообщение. Метод можно расширить для более тонкой фильтрации.
        /// </summary>
        protected virtual bool ShouldLog(eMessageType mtype, eMessageCategory mcategory)
        {
            return true;
        }

        /// <summary>
        /// Логирование результата с возвратом исходного текста.
        /// </summary>
        public string OK(string message, eMessageCategory mcategory = eMessageCategory.none)
        {
            if (ShouldLog(eMessageType.OK, mcategory))
            {
                InvokeDelegate(eMessageType.OK, mcategory, 0, message);
            }
            return message;
        }
        void ILogger.OK(string message, eMessageCategory mcategory)
        {
            OK(message, mcategory);
        }

        /// <summary>
        /// Логирование информационного сообщения с возвратом исходного текста.
        /// </summary>
        public string Info(string message, eMessageCategory mcategory = eMessageCategory.none)
        {
            if (ShouldLog(eMessageType.INFO, mcategory))
            {
                InvokeDelegate(eMessageType.INFO, mcategory, 0, message);
            }
            return message;
        }
        void ILogger.Info(string message, eMessageCategory mcategory)
        {
            Info(message, mcategory);
        }

        /// <summary>
        /// Логирование ошибки с возвратом исходного текста.
        /// </summary>
        public string Error(int code, string message, eMessageCategory mcategory = eMessageCategory.none)
        {
            if (ShouldLog(eMessageType.ERROR, mcategory))
            {
                InvokeDelegate(eMessageType.ERROR, mcategory, code, message);
            }
            return message;
        }
        void ILogger.Error(int code, string message, eMessageCategory mcategory)
        {
            Error(code, message, mcategory);
        }

        /// <summary>
        /// Логирование сообщения с кодом и возврат отформатированного текста.
        /// </summary>
        public string CodeMessage(CodeMessage cm, eMessageCategory mcategory = eMessageCategory.none)
        {
            eMessageType mt = LogHelper.GetMessageType(cm.сode);
            string formattedMessage = cm.сode != 0 ? $"{cm.сode} {cm.message}" : cm.message;
            if (ShouldLog(mt, mcategory))
            {
                InvokeDelegate(mt, mcategory, cm.сode, formattedMessage);
            }
            return formattedMessage;
        }
        void ILogger.CodeMessage(CodeMessage cm, eMessageCategory mcategory)
        {
            CodeMessage(cm, mcategory);
        }

        /// <summary>
        /// Асинхронное логирование сообщения с кодом, возвращает отформатированный текст.
        /// </summary>
        public async Task<string> CodeMessageAsync(CodeMessage cm, eMessageCategory mcategory = eMessageCategory.none)
        {
            return await Task.Run(() => CodeMessage(cm, mcategory));
        }
        async Task ILogger.CodeMessageAsync(CodeMessage cm, eMessageCategory mcategory)
        {
            await CodeMessageAsync(cm, mcategory);
        }

        private void InvokeDelegate(eMessageType mtype, eMessageCategory mcategory, int code, string message)
        {
            LogDelegate logDelegate;
            lock (_lockObj)
            {
                logDelegate = FormLogDelegate;
            }
            logDelegate?.Invoke(mtype, mcategory, code, message);
        }
    }

    /// <summary>
    /// Логгер, который записывает сообщения в файл. Реализован как Singleton.
    /// </summary>
    public class FileLogger : ILogger
    {
        private static readonly Lazy<FileLogger> _instance = new Lazy<FileLogger>(() => new FileLogger());
        public static FileLogger Instance => _instance.Value;

        private static readonly object sync = new object();
        private readonly string pathToLog;

        // Приватный конструктор предотвращает создание экземпляров извне.
        private FileLogger()
        {
            string settingsFileName = "sett111.json"; // SettingsControl.settingsFileName;
            pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log_{settingsFileName}");
            if (!Directory.Exists(pathToLog))
                Directory.CreateDirectory(pathToLog);
        }

        private string GetFileName()
        {
            return Path.Combine(pathToLog, string.Format("{0}_{1:yyyy.MM.dd}.log", "AppDiagnostic", DateTime.Now));
        }

        /// <summary>
        /// Записывает сообщение лога в файл и, при необходимости, выводит его в консоль.
        /// Использует StringBuilder для оптимального построения строки сообщения.
        /// </summary>
        /// <param name="prefix">Префикс сообщения, например, описание категории.</param>
        /// <param name="code">Код сообщения (если не равен 0, добавляется в лог).</param>
        /// <param name="message">Основной текст сообщения лога.</param>
        /// <param name="ex">Исключение, информация о котором будет добавлена в сообщение, если не null.</param>
        private void WriteLog(string prefix, int code, string message = "", Exception ex = null)
        {
            try
            {
                string fileName = GetFileName();
                // Используем StringBuilder для оптимального построения строки лога
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0:yyyy.MM.dd HH:mm:ss.fff}]", DateTime.Now);

                // Если prefix не пустой, добавляем его
                if (!string.IsNullOrWhiteSpace(prefix))
                {
                    sb.Append(" ");
                    sb.Append(prefix);
                }

                // Если code не равен 0, добавляем его
                if (code != 0)
                {
                    sb.Append(" ");
                    sb.Append(code.ToString());
                }

                // Если message не пустой, добавляем его
                if (!string.IsNullOrWhiteSpace(message))
                {
                    sb.Append(" ");
                    sb.Append(message);
                }

                // Если присутствует исключение, добавляем информацию о нем
                if (ex != null)
                {
                    sb.AppendFormat(" {0} [{1}.{2}()] {3}",
                        ex.HResult.ToString("X2"),
                        ex.TargetSite.DeclaringType,
                        ex.TargetSite.Name,
                        ex.Message);
                }

                // Получаем итоговую строку
                string fullText = sb.ToString();

                // Записываем строку в файл с учетом синхронизации
                lock (sync)
                {
                    File.AppendAllText(fileName, fullText + Environment.NewLine, Encoding.GetEncoding("Windows-1251"));
                }
            }
            catch { }
        }


        private string GetPrefix(eMessageCategory mcategory, eMessageType mtype)
        {
            return $"[{mtype.ToString().ToUpper()}] \t <{mcategory.ToString().ToUpper()}> ";
            
            switch (mcategory)
            {
                case eMessageCategory.App:
                    return "[APP]";
                case eMessageCategory.User:
                    return "[USER]";
                case eMessageCategory.Exception:
                    return "[EXCEPTION]";
                case eMessageCategory.Traffic:
                    return "[TRAFFIC]";
                case eMessageCategory.Source:
                    return "[SOURCE]";
                default:
                    switch (mtype)
                    {
                        case eMessageType.OK:
                            return "<OK>";
                        case eMessageType.INFO:
                            return "<INFO>";
                        case eMessageType.ERROR:
                            return "<ERROR>";
                        default:
                            return "";
                    }
            }
        }

        /// <summary>
        /// Логирует результат и возвращает его.
        /// </summary>
        public string OK(string message, eMessageCategory mcategory = eMessageCategory.none)
        {
            WriteLog(GetPrefix(mcategory, eMessageType.OK), 0, message);
            return message;
        }
        void ILogger.OK(string message, eMessageCategory mcategory)
        {
            OK(message, mcategory);
        }

        /// <summary>
        /// Логирует информационное сообщение и возвращает сообщение.
        /// </summary>
        public string Info(string message, eMessageCategory mcategory = eMessageCategory.none)
        {
            WriteLog(GetPrefix(mcategory, eMessageType.INFO), 0, message);
            return message;
        }
        void ILogger.Info(string message, eMessageCategory mcategory)
        {
            Info(message, mcategory);
        }

        /// <summary>
        /// Логирует ошибку и возвращает сообщение.
        /// </summary>
        public string Error(int code, string message, eMessageCategory mcategory = eMessageCategory.none)
        {
            WriteLog(GetPrefix(mcategory, eMessageType.ERROR), code, message);
            return message;
        }
        void ILogger.Error(int code, string message, eMessageCategory mcategory)
        {
            Error(code, message, mcategory);
        }

        /// <summary>
        /// Логирует сообщение с кодом и возвращает отформатированный текст.
        /// </summary>
        public string CodeMessage(CodeMessage cm, eMessageCategory mcategory = eMessageCategory.none)
        {
            string formattedMessage = cm.сode != 0 ? $"{cm.сode} {cm.message}" : cm.message;
            eMessageType mt = LogHelper.GetMessageType(cm.сode);
            switch (mt)
            {
                case eMessageType.OK:
                    return OK(formattedMessage, mcategory);
                case eMessageType.INFO:
                    return Info(formattedMessage, mcategory);
                case eMessageType.ERROR:
                    return Error(cm.сode, formattedMessage, mcategory);
                default:
                    return Info(formattedMessage, mcategory);
            }
        }
        void ILogger.CodeMessage(CodeMessage cm, eMessageCategory mcategory)
        {
            CodeMessage(cm, mcategory);
        }

        /// <summary>
        /// Асинхронное логирование сообщения с кодом, возвращает отформатированный текст.
        /// </summary>
        public async Task<string> CodeMessageAsync(CodeMessage cm, eMessageCategory mcategory = eMessageCategory.none)
        {
            return await Task.Run(() => CodeMessage(cm, mcategory));
        }
        async Task ILogger.CodeMessageAsync(CodeMessage cm, eMessageCategory mcategory)
        {
            await CodeMessageAsync(cm, mcategory);
        }

        // --------------------------------------------------------------------
        // Вложенные классы для композитных логгеров
        // --------------------------------------------------------------------

        // FileLogger.ConsoleLogger -> комбинация: FileLogger + ConsoleLogger (файл и консоль)
        public static class WithConsole
        {
            private static readonly ILogger _instance = new CompositeLogger(
                FileLogger.Instance,
                ConsoleLogger.Instance);
            public static ILogger Instance => _instance;

            // FileLogger.ConsoleLogger.FormLogger -> комбинация: FileLogger + ConsoleLogger + FormLogger (файл, консоль и таблица)
            public static class AndForm
            {
                private static readonly ILogger _instance = new CompositeLogger(
                    FileLogger.Instance,
                    ConsoleLogger.Instance,
                    FormLogger.Instance);
                public static ILogger Instance => _instance;
            }
        }

        // FileLogger.FormLogger -> комбинация: FileLogger + FormLogger (файл и таблица)
        public static class WithForm
        {
            private static readonly ILogger _instance = new CompositeLogger(
                FileLogger.Instance,
                FormLogger.Instance);
            public static ILogger Instance => _instance;

            // FileLogger.FormLogger.ConsoleLogger -> комбинация: FileLogger + FormLogger + ConsoleLogger (файл, таблица и консоль)
            public static class AndConsole
            {
                private static readonly ILogger _instance = new CompositeLogger(
                    FileLogger.Instance,
                    FormLogger.Instance,
                    ConsoleLogger.Instance);
                public static ILogger Instance => _instance;
            }
        }
    }

    // ============================================================================

    /// <summary>
    /// Класс для вывода сообщений на форму
    /// </summary>
    public class LabelLogger
    {
        /// <summary>
        /// Делегат, определяющий метод для обработки лог-сообщений.
        /// </summary>
        /// <param name="mtype">Тип лог-сообщения (например, OK, INFO, ERROR).</param>
        /// <param name="message">Текст лог-сообщения.</param>
        public delegate void LogDelegate(eMessageType mtype, string message);

        private readonly LogDelegate _logDelegate;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LabelLogger"/> с заданным делегатом логирования.
        /// </summary>
        /// <param name="logDelegate">Делегат, который обрабатывает лог-сообщения.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="logDelegate"/> равен null.</exception>
        public LabelLogger(LogDelegate logDelegate)
        {
            _logDelegate = logDelegate ?? throw new ArgumentNullException(nameof(logDelegate));
        }

        /// <summary>
        /// Выполняет логирование сообщения с заданным типом, вызывая делегат логирования.
        /// </summary>
        /// <param name="mtype">Тип лог-сообщения.</param>
        /// <param name="message">Сообщение для логирования.</param>
        /// <returns>Возвращает тот же текст сообщения, который был залогирован.</returns>
        public string Log(eMessageType mtype, string message)
        {
            _logDelegate?.Invoke(mtype, message);
            return message;
        }

        /// <summary>
        /// Логирует сообщение, используя структуру <see cref="CodeMessage"/>, определяя тип сообщения по коду.
        /// </summary>
        /// <param name="cm">Структура <see cref="CodeMessage"/>, содержащая код и текст сообщения.</param>
        /// <returns>Возвращает текст сообщения, содержащийся в <paramref name="cm"/>.</returns>
        public string CodeMessage(CodeMessage cm)
        {
            eMessageType mtype = LogHelper.GetMessageType(cm.сode);
            Log(mtype, cm.message);
            return cm.message;
        }
    }

    // ============================================================================

    public static class UsageExamples
    {
        /// <summary>
        /// Демонстрирует использование различных логгеров: ConsoleLogger, CompositeLogger, LabelLogger.
        /// </summary>
        public static void Run()
        {
            // Пример 1: Использование ConsoleLogger
            ConsoleLogger.Instance.OK("Синхронизация данных завершена", eMessageCategory.App);
            ConsoleLogger.Instance.Info("Получено 1000 строк", eMessageCategory.User);
            ConsoleLogger.Instance.Error(-999, "Ошибка чтения файла", eMessageCategory.Exception);
            CodeMessage cm = new CodeMessage(-100, "Ошибка подключения к серверу.");
            ConsoleLogger.Instance.CodeMessage(cm, eMessageCategory.SQL);

            // Пример 2: Использование CompositeLogger с ConsoleLogger и FileLogger
            ILogger compositeLogger = new CompositeLogger(ConsoleLogger.Instance, FileLogger.Instance);
            compositeLogger.OK("Таблица создана", eMessageCategory.DATA);

            // Пример 3: Использование LabelLogger с пользовательским делегатом для вывода сообщений на форму
            LabelLogger labelLogger = new LabelLogger((mtype, message) =>
            {
                // Здесь можно реализовать вывод сообщения на форму или иной механизм отображения.
                Console.WriteLine($"LabelLogger: Тип: {mtype}, Сообщение: {message}");
            });
            labelLogger.Log(eMessageType.OK, "Пример сообщения через LabelLogger.");
            CodeMessage cm3 = new CodeMessage(0, "Операция выполнена успешно.");
            labelLogger.CodeMessage(cm3);

            // Пример 4: Использование ProcessMaster с применением LogTarget
            // Создаём экземпляр ProcessMaster, который логирует сообщения одновременно в файл и форму.
            //ProcessMaster autoMaster = new ProcessMaster(LogTarget.FileAndForm, labelLogger);
            //autoMaster.Info("Пример сообщения через ProcessMaster с LogTarget.FileAndForm");
        }

    }

}

