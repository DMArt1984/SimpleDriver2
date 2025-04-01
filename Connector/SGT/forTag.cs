using LogCodeMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connector
{
    public enum eTagStatus
    {
        zero = 1000, // не определено
        good = 0, // Тег работает корректно
        error = -400, // Ошибка тега
        sourceOpened = 100, // Источник открыт – тег получает данные от открытого источника
        sourceClosed = 200, // Источник закрыт – тег не может получать данные, так как источник закрыт
        sourceFail = -200, // Ошибка источника – возникла проблема с источником, из-за которой тег не обновляется
        groupOff = 300, // Группа отключена – теги в данной группе не активны
        groupOn = 301, // Группа включена – теги в группе активны, но опрос может быть не запущен
        tagOff = 50, // Тег отключен – тег не участвует в опросе
        tagOn = 51 // Тег включен – тег участвует в опросе 
    }

    public static class eTagCodeExtensions
    {
        public static string GetText(this eTagStatus status)
        {
            switch (status)
            {
                case eTagStatus.good:
                    return "Норма";
                case eTagStatus.error:
                    return "Ошибка тега";
                case eTagStatus.sourceOpened:
                    return "Источник открыт – тег получает данные от открытого источника";
                case eTagStatus.sourceClosed:
                    return "Источник закрыт – тег не может получать данные, так как источник закрыт";
                case eTagStatus.sourceFail:
                    return "Источник в ошибке – возникла проблема с источником, из-за которой тег не обновляется";
                case eTagStatus.groupOff:
                    return "Группа отключена – теги в данной группе не активны";
                case eTagStatus.groupOn:
                    return "Группа включена – теги в группе активны, но опрос может быть не запущен";
                case eTagStatus.tagOff:
                    return "Тег отключен – тег не участвует в опросе";
                case eTagStatus.tagOn:
                    return "Тег включен – тег участвует в опросе";
                default:
                    return status.ToString();
            }
        }
    }

    public static class CodeMessageConstants
    {
        public static readonly CodeMessage Created = new CodeMessage(1, "Новый тег");
        public static readonly CodeMessage EmptyRequest = new CodeMessage(-30, "Пустой запрос");
        public static readonly CodeMessage NoPing = new CodeMessage(-400, "Нет пинга");
        public static readonly CodeMessage NewValueIsNull = new CodeMessage(404, "Новое значение равно null");
        public static readonly CodeMessage ConnectionTimedOut = new CodeMessage(-70, "Превышено время ожидания подключения");
        public static readonly CodeMessage TagTimeout = new CodeMessage(-71, "Таймаут тега");
        public static readonly CodeMessage NoWrite = new CodeMessage(-80, "Запись невозможна");
        public static readonly CodeMessage NoData = new CodeMessage(-31, "Нет данных");
        public static readonly CodeMessage BreakError = new CodeMessage(-600, "Возможна ошибка источника");
        public static readonly CodeMessage Inconsistency = new CodeMessage(-90, "Несоответствие типа данных");
        public static readonly CodeMessage NotReliableA = new CodeMessage(-700, "Нет достоверных данных в адресе");
        public static readonly CodeMessage NotReliableTW = new CodeMessage(-701, "Нет достоверных данных в теге для записи");
        public static readonly CodeMessage NoTagForWrite = new CodeMessage(-702, "Нет тега для записи");
    }

}
