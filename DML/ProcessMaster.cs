using DML.Log;
using LogCodeMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DML
{
    /// <summary>
    /// Класс AutoMaster наследуется от BaseMaster и предоставляет функциональность для автоматического логирования.
    /// </summary>
    class ProcessMaster : BaseLogger
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса AutoMaster с заданным типом логгирования и опциональным логгером для меток.
        /// </summary>
        /// <param name="logTarget">Тип логгирования, определяющий, какой логгер использовать.</param>
        /// <param name="labelLogger">
        /// Опциональный логгер для меток. Если не передан, используется заглушка, которая не выполняет никаких действий.
        /// </param>
        public ProcessMaster(LogTarget logTarget, LabelLogger labelLogger = null)
            : base(logTarget, labelLogger)
        {
            // Вызов конструктора базового класса для инициализации логгеров.
        }

        /// <summary>
        /// Логирует сообщение с использованием логгера для меток.
        /// </summary>
        /// <param name="mt">Тип сообщения (например, info, error, OK).</param>
        /// <param name="message">Текст сообщения.</param>
        public void LabelLog(eMessageType mt, string message)
        {
            // Передаем сообщение и тип сообщения в логгер для меток.
            _labelLogger.Log(mt, message);
        }

        /// <summary>
        /// Логирует информационное сообщение с категорией "App".
        /// </summary>
        /// <param name="message">Текст информационного сообщения.</param>
        public void Info(string message)
        {
            // Логируем сообщение через основной логгер с категорией "App".
            logger.Info(message, eMessageCategory.App);
            // Дополнительно логируем через логгер для меток с типом сообщения info.
            _labelLogger.Log(eMessageType.INFO, message);
        }

        /// <summary>
        /// Логирует сообщение с кодом, используя основной логгер и логгер для меток.
        /// </summary>
        /// <param name="cm">Структура CodeMessage, содержащая код и текст сообщения.</param>
        public void CodeMessage(CodeMessage cm)
        {
            // Логирование сообщения с кодом через основной логгер с категорией "App".
            logger.CodeMessage(cm, eMessageCategory.App);
            // Также передаем сообщение с кодом в логгер для меток.
            _labelLogger.CodeMessage(cm);
        }
    }
}
