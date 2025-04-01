using LogCodeMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connector
{
    public enum eDataType
    {
        Bool = 0,
        Byte = 1,
        Binary = 2,
        Short = 3, UShort = 4,
        Int = 5, UInt = 6,
        Float = 7,
        Long = 10,
        Double = 11,
        STRING = 21,
        HEX = 30,
        Char = 32,
        ArrayA = 101,
        ArrayB = 102,
        ArrayC = 103
    }
    public enum eCommand
    {
        None = 0,
        Play = 1,
        Update = 2,
        Wait = 3
    }
    public enum eDirect
    {
        Read = 0,
        Write = 10
    }
    public enum eDirectFull
    {
        Read = 0,
        WriteConstValue = 11,
        WriteTagValue = 12
    }

    public interface ITagClient
    {
        ushort Id { get; }
        string title { get; }
        bool Good { get; }
        eDataType DataType { get; }
        string Address { get; }
        bool Off { get; }
        dynamic WriteConstValue { get; set; }
        ushort WriteTagId { get; }
        eDirectFull directFull { get; }
        eCommand Command { get; set; }
        dynamic value { get; set; }
        dynamic LastGoodValue { get; }
        dynamic WriteTagValue { get; }
        CodeMessage codeMessage { get; set; }
        void SetResult(TagResult result);
        // Свойство InnerTagIds удалено
    }
    public interface ITagResult
    {
        ushort Id { get; }
        bool Good { get; }
        dynamic value { get; set; }
        CodeMessage codeMessage { get; set; }
    }
    public interface IAppendTag
    {
        dynamic LastGoodValue { get; }
    }

    public struct TagParam
    {
        public readonly ushort Id;
        public string address;
        public eDataType dataType;
        public string writeValue;
        public bool off;

        public TagParam(ushort Id, bool off, string address, eDataType dataType, string writeValue)
        {
            this.Id = Id;
            this.off = off;
            this.address = address;
            this.dataType = dataType;
            this.writeValue = writeValue;
        }
    }
    public struct TagResult
    {
        public dynamic value;
        public CodeMessage codeMessage;

        public TagResult(dynamic value, int code, string message)
        {
            this.value = value;
            this.codeMessage = new CodeMessage(code, message);
        }

        public TagResult(dynamic value, CodeMessage cm)
        {
            this.value = value;
            this.codeMessage = cm;
        }

        public TagResult(dynamic value)
        {
            this.value = value;
            this.codeMessage = CodeMessageFactory.FromEnumX(eTagStatus.good);
        }

        public TagResult(dynamic value, Exception ex)
        {
            this.value = value;
            this.codeMessage = CodeMessageFactory.FromException(ex);
        }
    }
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
