using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DML.Log
{
    /// <summary>
    /// Абстрактный базовый класс для мастеров, предоставляющий общую функциональность логирования.
    /// </summary>
    public abstract class BaseLogger
    {
        /// <summary>
        /// Выбранный логгер для логирования сообщений.
        /// </summary>
        protected readonly ILogger logger;

        /// <summary>
        /// Логгер для меток, используемый для передачи лог-сообщений в пользовательский интерфейс или для другой обработки.
        /// </summary>
        protected LabelLogger _labelLogger;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BaseLogger"/> с заданным типом логгера.
        /// </summary>
        /// <param name="logTarget">Тип логгирования, определяющий, какой логгер использовать.</param>
        /// <param name="labelLogger">
        /// Необязательный логгер для меток. Если не указан, используется заглушка, которая ничего не делает.
        /// </param>
        protected BaseLogger(LogTarget logTarget, LabelLogger labelLogger = null)
        {
            // Инициализируем основной логгер, выбирая его на основе типа логгирования.
            logger = GetLogger(logTarget);
            // Если логгер для меток передан, используем его; иначе создаем новый, который ничего не делает.
            _labelLogger = labelLogger ?? new LabelLogger((msgType, message) => { /* Ничего не делаем */ });
        }

        /// <summary>
        /// Выбирает и возвращает нужный логгер на основании переданного параметра.
        /// </summary>
        /// <param name="logTarget">Тип логгирования, определяющий, какой логгер вернуть.</param>
        /// <returns>Инстанс, реализующий интерфейс <see cref="ILogger"/>.</returns>
        protected ILogger GetLogger(LogTarget logTarget)
        {
            // Используем конструкцию switch для выбора логгера по типу.
            switch (logTarget)
            {
                case LogTarget.FileOnly:
                    // Возвращаем файловый логгер.
                    return FileLogger.Instance;
                case LogTarget.ConsoleOnly:
                    // Возвращаем логгер для консоли.
                    return ConsoleLogger.Instance;
                case LogTarget.FormOnly:
                    // Возвращаем логгер для формы.
                    return FormLogger.Instance;
                case LogTarget.FileAndConsole:
                    // Возвращаем комбинированный логгер: файл + консоль.
                    return FileLogger.WithConsole.Instance;
                case LogTarget.FileAndForm:
                    // Возвращаем комбинированный логгер: файл + форма.
                    return FileLogger.WithForm.Instance;
                case LogTarget.FileConsoleForm:
                    // Возвращаем комбинированный логгер: файл + консоль + форма.
                    return FileLogger.WithConsole.AndForm.Instance;
                default:
                    // Если тип логгирования не распознан, возвращаем файловый логгер по умолчанию.
                    return FileLogger.Instance;
            }
        }
    }
}

