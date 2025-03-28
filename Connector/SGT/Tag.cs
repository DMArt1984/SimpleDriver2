using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DML;
using DML.Log;

namespace WinSimpleIDriver.Connector.SGT
{
    public enum eDataType // типы данных
    {
        Bool = 0, // 1 бит
        Byte = 1, // 8 бит = 1 байт
        Binary = 2,
        Short = 3, UShort = 4,  // 2 байта
        Int = 5, UInt = 6,// 4 байта
        Float = 7, // 4 байта
        Long = 10, // 8 байт
        Double = 11, // 8 байт
        STRING = 21,
        HEX = 30,
        Char = 32,
        ArrayA = 101, // массив с разделителем ;
        ArrayB = 102, // массив с разделителем ПРОБЕЛ
        ArrayC = 103 // массив с разделителем ~
    }

    public enum eCommand // Команда
    {
        None = 0,
        Play = 1, // по запросу
        Update = 2, // по изменению значения записи
        Wait = 3
    }

    public enum eDirect // направление данных
    {
        Read = 0,
        Write = 10
    }

    public enum eDirectFull // расширенное направление данных 
    {
        Read = 0,
        WriteConstValue = 11,
        WriteTagValue = 12
    }

    public enum eTagCode // Коды тегов
    {
        good = 0,
        created = 1, // новый тег
        sourceOpened = 100,
        sourceClosed = 200,

        sourceFail = -200,
        groupOff = 300,
        groupOn = 301,
        noPing = -400,
        newValueIsNull = 404,
        tagOff = 50,
        tagOn = 51,
        IsNotSupport = -60, // тип данных не поддерживается
        connectionTimedOut = -70,
        tagTimeout = -71,
        noWrite = -80,
        noData = -30,
        breakError = -600, // возможно ошибка источника
        inconsistency = -90, // не соответствие типа данных

        notReliableA = -700, // нет достоверных данных в адресе
        notReliableTW = -701, // нет достоверных данных в теге для записи
        noTagForWrite = -702 // нет тега для записи

    }


    public interface ITagClient
    {
        ushort Id { get; }
        string title { get; }
        bool Good { get; }
        eDataType DataType { get; }
        string Address { get; }
        bool Off { get; }
        dynamic WriteConstValue { get; set;  }
        ushort WriteTagId { get; }
        eDirectFull directFull { get; }
        eCommand Command { get; set; }
        dynamic Value { get; set; }
        dynamic LastGoodValue { get; }
        dynamic WriteTagValue { get; }
        CodeMessage codeMessage { get; set; }
        void SetResult(TagResult result);

        ushort[] InnerTagIds { get; set; }
    }

    public interface ITagResult
    {
        ushort Id { get; }
        bool Good { get; }
        dynamic Value { get; set; }
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

    public struct cellTag
    {
        public ushort Id;
        public DataGridViewRow row;
        public DataGridViewCell value;
        public DataGridViewCell address;
        public DataGridViewCell dataType;
        public DataGridViewCell writeValue;
        public DataGridViewCell on;
        public DataGridViewCell code;
        public DataGridViewCell message;
        public DataGridViewCell comment;

    }
    public struct TagResult // Возвращаемое значение
    {
        public dynamic value; // Значение
        public CodeMessage codeMessage; // Код и Сообщение
        public TagResult(dynamic value, int code, string message)
        {
            this.value = value;
            this.codeMessage = new CodeMessage(code, message);
        }
        public TagResult(dynamic value, eTagCode tagCode)
        {
            this.value = value;
            this.codeMessage = new CodeMessage((int)tagCode);
        }
    }

    public static class eTagCodeExtensions
    {
        public static string GetText(this eTagCode code)
        {
            switch (code)
            {
                case eTagCode.good:
                    return "Норма";

                case eTagCode.created:
                    return "Создан";

                case eTagCode.tagOff:
                    return "Тег отключен";

                case eTagCode.tagOn:
                    return "Тег включен, нет опроса";

                case eTagCode.groupOff:
                    return "Группа отключена";

                case eTagCode.groupOn:
                    return "Группа включена, нет опроса";

                case eTagCode.sourceClosed:
                    return "Источник закрыт";

                case eTagCode.sourceOpened:
                    return "Источник открыт, нет опроса";

                case eTagCode.inconsistency:
                    return "Не соответствут типу данных";

                case eTagCode.notReliableA:
                    return "Не достоверны данные в адресе {?}";

                case eTagCode.notReliableTW:
                    return "Нет достоверных данных в теге для записи";

                case eTagCode.noTagForWrite:
                    return "нет тега для записи";

                case eTagCode.breakError:
                    return "Вероятна ошибка подключения";

                case eTagCode.connectionTimedOut:
                case eTagCode.IsNotSupport:
                case eTagCode.newValueIsNull:
                case eTagCode.noData:
                case eTagCode.noPing:
                case eTagCode.noWrite:
                case eTagCode.sourceFail:
                case eTagCode.tagTimeout:
                    return code.ToString();
                default:
                    return code.ToString();
            }
        }
    }

    public class Tag: ICodeMessage, ITagClient, ITagResult, IAppendTag
    {
        public ushort Id { get; } // ID тега
        public string title { get; } // Название тега
        public string description { get; } // Описание тега

        ushort _sourceId = 0; // ID устройства
        public ushort sourceId => _sourceId;
        ushort _groupId = 0; // ID группы
        public ushort groupId => _groupId;

        string _sourceTitle = ""; // Название устройства
        public string sourceTitle => _sourceTitle;
        string _groupTitle = ""; // Название группы
        public string groupTitle => _groupTitle;

        public string block { get; set; } // Блок

        public bool Good => codeMessage.сode == (int)eTagCode.good; // Тег достоверный // _off == false && 

        // симуляция
        public bool SimEnable = false; // использование симуляции
        public dynamic SimValue = null; // значение симуляции

        // контроль лицензии
        public bool lic = false;

        // события
        public delegate void HandlerCode(ushort Id, CodeMessage activeCM, CodeMessage lastError);
        public event HandlerCode eventCode;

        public delegate void HandlerParam(TagParam info);
        public event HandlerParam eventParams;

        public delegate void HandlerValue(ushort Id);
        public event HandlerValue eventValue;

        // Теги в адресе
        public ushort[] InnerTagIds { get; set; }
        public Tag[] InnerTags { get; set; }

        // Конструктор
        public Tag(ushort Id, string title, eDataType dataType, string address, string description = "")
        {
            this.Id = Id;
            this.title = title;
            this.description = description;
            _dataType = dataType;
            _address = address;
        }

        public Tag(ushort Id, string title, eDataType dataType, string address, string sourceTitle, string groupTitle, ushort sourceId, ushort groupId, string description = "") : this (Id, title, dataType, address, description)
        {
            this._sourceTitle = sourceTitle;
            this._groupTitle = groupTitle;
            this._sourceId = sourceId;
            this._groupId = groupId;
        }

        // Определить недостающие ID-Title
        public void SetLinkIdTitle()
        {
            // source ID and title
            if (String.IsNullOrWhiteSpace(_sourceTitle) == false)
            {
                var source = Source.Item(_sourceTitle); //.items.FirstOrDefault(x => x.title == _sourceTitle);
                if (source != null)
                {
                    _sourceId = source.Id;
                } else if (Source.lastId == 1)
                {
                    _sourceId = Source.items[0].Id;
                    _sourceTitle = Source.items[0].title;
                }
            }
            else if (_sourceId > 0)
            {
                var source = Source.Item(_sourceId); //.items.FirstOrDefault(x => x.Id == _sourceId);
                if (source != null)
                {
                    _sourceTitle = source.title;
                } else if (Source.lastId == 1)
                {
                    _sourceId = Source.items[0].Id;
                    _sourceTitle = Source.items[0].title;
                }
            }

            // group ID and title
            if (String.IsNullOrWhiteSpace(_groupTitle) == false)
            {
                var group = Group.Item(_groupTitle); //.items.FirstOrDefault(x => x.title == _groupTitle);
                if (group != null)
                {
                    _groupId = group.Id;
                }
                else // создаем группу с заданным названием
                {
                    _groupId = ++Group.lastId;
                    Group.items.Add(new Group(_groupId, _groupTitle, 100, false, "Создан динамически по имени"));
                }
            }
            else if (_groupId > 0)
            {
                var group = Group.Item(_groupId); //.items.FirstOrDefault(x => x.Id == _groupId);
                if (group != null)
                {
                    _groupTitle = group.title;
                }
            }

            if (_groupId == 0 && String.IsNullOrWhiteSpace(_groupTitle))
            {
                if (Group.lastId == 1) // группа только одна, значит это она
                {
                    _groupId = Group.items[0].Id;
                    _groupTitle = Group.items[0].title;
                }
                else // создаем новую группу
                {
                    _groupId = ++Group.lastId;
                    _groupTitle = $"GroupID{Group.lastId}";
                    Group.items.Add(new Group(_groupId, _groupTitle, 100, false, "Создан динамически"));
                }
                
            }

            // write tag ID and title
            if (String.IsNullOrWhiteSpace(_writeTagTitle) == false && title != _writeTagTitle)
            {
                var tag = Tag.Item(_writeTagTitle); // Tag.items.FirstOrDefault(x => x.title == _writeTagTitle);
                if (tag != null)
                {
                    WriteTagId = tag.Id;
                } else
                {
                    _writeTagTitle = "";
                }
            }
            else if (WriteTagId > 0 && Id != WriteTagId)
            {
                var tag = Tag.Item(WriteTagId); // Tag.items.FirstOrDefault(x => x.Id == WriteTagId);
                if (tag != null)
                {
                    _writeTagTitle = tag.title;
                } else
                {
                    WriteTagId = 0;
                }
            }
        }

        // Установить ID-Title для тегов
        static public void CalcId()
        {
            foreach (var item in Tag.items)
            {
                item.SetLinkIdTitle();
            }
        }

        public void SetParam(bool off, bool command, ushort writeTagId = 0, dynamic writeValue = null)
        {
            _off = off;
            _writeConstValue = writeValue;
            WriteTagId = writeTagId;
            Command = command ? eCommand.Wait : eCommand.None;
            //EventChangeParam();
        }

        public void SetParam(bool off, bool command, string writeTagTitle = "", dynamic writeValue = null)
        {
            _off = off;
            _writeConstValue = writeValue;
            this._writeTagTitle = writeTagTitle;
            Command = command ? eCommand.Wait : eCommand.None;
            //EventChangeParam();
        }

        // Тип данных
        public eDataType DataType
        {
            get => _dataType;
            set
            {
                if (_dataType != value)
                {
                    _dataType = value;
                    Value = null;
                    _lastGoodValue = null;
                    EventChangeParam();
                }
            }
        }
        eDataType _dataType;

        // Адрес
        public string Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    Value = null;
                    _lastGoodValue = null;

                    SetInnerTagsForOneTag();
                    EventChangeParam();
                }
            }
        }
        string _address;

        // Тег отключен
        public bool Off
        {
            get => _off;
            set
            {
                if (_off != value)
                {
                    _off = value;
                    EventChangeParam();
                }
            }
        }
        bool _off = false;

        // Значение для записи
        public dynamic WriteConstValue
        {
            get => _writeConstValue;
            set
            {
                bool change;
                try
                {
                    change = _writeConstValue != value;
                } catch
                {
                    change = true;
                }

                if (change)
                {
                    _writeConstValue = value;
                    EventChangeParam(true);
                }
            }
        }
        dynamic _writeConstValue = null;

        // ID тега для записи
        public ushort WriteTagId
        {
            get => _writeTagId;
            set
            {
                if (_writeTagId != value)
                {
                    _writeTagId = value;
                    appendValue = Tag.items.FirstOrDefault(x => x.Id == value);
                    EventChangeParam(true);
                }
            }
        }
        ushort _writeTagId = 0;
        public string WriteTagTitle => _writeTagTitle;
        string _writeTagTitle;
        IAppendTag appendValue = null;
        public dynamic WriteTagValue => appendValue?.LastGoodValue;

        // То, что пишем в тег
        public string GetWriteCell()
        {
            return (!String.IsNullOrWhiteSpace(WriteTagTitle)) ? WriteTagTitle : (WriteConstValue == null) ? null : String.Join(";", WriteConstValue);
        }

        // Направление: чтение/запись
        public eDirectFull directFull
        {
            get
            {
                if (WriteTagId == 0 && WriteConstValue == null)
                {
                    return eDirectFull.Read;
                }
                else if (WriteTagId > 0)
                {
                    return eDirectFull.WriteTagValue;
                }
                else
                {
                    return eDirectFull.WriteConstValue;
                }
            }
        }

        public eDirect direct
        {
            get
            {
                if (WriteTagId == 0 && WriteConstValue == null)
                {
                    return eDirect.Read;
                }
                else
                {
                    return eDirect.Write;
                }
            }
        }

        // Команда
        public eCommand Command
        {
            get => _command;
            set
            {
                if (_command != value)
                {
                    _command = value;
                    //if (value == eCommand.Wait)
                    //    codeMessage = new CodeMessage((int)eTagCode.waitCommand);

                    EventChangeParam(true);
                }
            }
        }
        eCommand _command = eCommand.None;

        // Текущее значение
        public dynamic Value
        {
            get => _value;
            set
            {
                _value = value;
                if (Good)
                {
                    _lastGoodValue = value;
                    _lastDTUpdate = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff");
                }

                eventValue?.Invoke(Id);
            }
        }
        dynamic _value = null;

        public string LastDTUpdate => _lastDTUpdate;
        string _lastDTUpdate = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff");

        // Последнее достоверное значение
        public dynamic LastGoodValue => _lastGoodValue;
        dynamic _lastGoodValue = null;

        // Статус
        public CodeMessage codeMessage
        {
            get => _codeMessage;
            set
            {
                if (_codeMessage.сode != value.сode)
                {
                    _codeMessage = value;
                    CheckLastError();
                    EventChangeCodeMessage();
                }
                
            }
        }
        CodeMessage _codeMessage = CodeMessageFactory.FromEnum(eTagCode.created);

        public CodeMessage LastError => _lastError;
        CodeMessage _lastError;

        private void CheckLastError()
        {
            if (_codeMessage.сode < 0)
            {
                _lastError = new CodeMessage(_codeMessage.сode, _codeMessage.message);
            }
        }

        public void ClearLastError()
        {
            _lastError = new CodeMessage(0, "");
            CheckLastError();
            EventChangeCodeMessage();
        }

        void EventChangeParam(bool noSetTagON = false)
        {
            if (Off)
            {
                codeMessage = CodeMessageFactory.FromEnum(eTagCode.tagOff);
            } else if (noSetTagON == false)
            {
                codeMessage = CodeMessageFactory.FromEnum(eTagCode.tagOn);
            }
            eventParams?.Invoke(new TagParam(Id, Off, Address, DataType, GetWriteCell()));
        }

        void EventChangeCodeMessage()
        {
            eventCode?.Invoke(Id, codeMessage, LastError);
        }

        // Установление результата
        public void SetResult(TagResult result)
        {
            if (SimEnable == false) // new 2024.01.28
            {
                // Value было здесь! 2024.01.28
                codeMessage = result.codeMessage;
                Value = result.value;
            }
        }

        public void Refresh()
        {
            EventChangeParam(true);
            EventChangeCodeMessage();
            eventValue?.Invoke(Id);
        }

        public void SetSim(bool enable, dynamic value = null)
        {
            SimEnable = enable;
            SimValue = value;
            if (SimEnable)
            {
                codeMessage = CodeMessageFactory.FromEnum(eTagCode.good);
                Value = value;
            } else
            {
                codeMessage = CodeMessageFactory.FromEnum(eTagCode.created);
            }
        }

        // =====================================================================================

        static public List<Tag> items = new List<Tag>(); // все теги
        static public ushort lastId = 0;
        static public bool log = false;

        static public void Clear()
        {
            Tag.lastId = 0;
            Tag.items = new List<Tag>();
        }

        
        

        static public Tag Item(ushort Id) => items.FirstOrDefault(x => x.Id == Id);
        static public Tag Item(string title) => items.FirstOrDefault(x => x.title == title);

        static public string CodeTextDelete(eTagCode code) // !!!
        {
            switch (code)
            {
                case eTagCode.good:
                    return "Норма";

                case eTagCode.created:
                    return "Создан";

                case eTagCode.tagOff:
                    return "Тег отключен";

                case eTagCode.tagOn:
                    return "Тег включен, нет опроса";

                case eTagCode.groupOff:
                    return "Группа отключена";

                case eTagCode.groupOn:
                    return "Группа включена, нет опроса";

                case eTagCode.sourceClosed:
                    return "Источник закрыт";

                case eTagCode.sourceOpened:
                    return "Источник открыт, нет опроса";

                case eTagCode.inconsistency:
                    return "Не соответствут типу данных";

                case eTagCode.notReliableA:
                    return "Не достоверны данные в адресе {?}";

                case eTagCode.notReliableTW:
                    return "Нет достоверных данных в теге для записи";

                case eTagCode.noTagForWrite:
                    return "нет тега для записи";

                case eTagCode.breakError:
                    return "Вероятна ошибка подключения";

                case eTagCode.connectionTimedOut:
                case eTagCode.IsNotSupport:
                case eTagCode.newValueIsNull:
                case eTagCode.noData:
                case eTagCode.noPing:
                case eTagCode.noWrite:
                case eTagCode.sourceFail:
                case eTagCode.tagTimeout:
                    return code.ToString();
                default:
                    return code.ToString();
            }
        }

        static public bool InProject(dynamic output) => JsonControl.IsProp(output, "Tags");
        static public bool IsStructures(dynamic output) => JsonControl.IsProp(output, "Structures");
        static public bool IsTargetTags(dynamic output) => JsonControl.IsProp(output, "TargetTags");
        static public bool IsListBlocks(dynamic output) => JsonControl.IsProp(output, "Blocks");

        // -----------------------------------------------------------------------------------------------------

        // Получение параметров тега
        static public void ParseItemTag(dynamic item, uint forId, out string title, out string source, out eDataType dataType, out bool off, out string address, out string description, out string writeTitle, out string groupTitle, out string constValue, out bool isCommand)
        {
            title = JsonControl.GetString(item, "Title", $"Tag #{forId}");
            source = JsonControl.GetString(item, "Source");
            groupTitle = JsonControl.GetString(item, "Group");
            dataType = JsonControl.GetTypeEnum<eDataType>(item, "DataType", eDataType.Binary);
            address = JsonControl.GetString(item, "Addr");
            off = JsonControl.GetBool(item, "Off");
            description = JsonControl.GetString(item, "Desc");
            writeTitle = JsonControl.GetString(item, "Write");
            constValue = JsonControl.GetString(item, "Value", null);
            isCommand = JsonControl.GetBool(item, "Command");
        }

        // Получение параметров структуры
        static public void ParseItemStructure(dynamic item, out string title, out string join, out string templateAddress, out eDataType dataType, out string tagSource, out string group, out string[] sourceTags, out List<TargetTag> targetTags)
        {
            title = JsonControl.GetString(item, "Title", $"noname #{DateTime.Now.Millisecond}");
            join = JsonControl.GetString(item, "Join", ".");
            templateAddress = JsonControl.GetString(item, "Address", "{#Source.[#Target]}");
            dataType = JsonControl.GetTypeEnum<eDataType>(item, "DataType", eDataType.Binary);
            tagSource = JsonControl.GetString(item, "Source", "");
            group = JsonControl.GetString(item, "Group", "");
            sourceTags = JsonControl.GetArrayString(item, "SourceTags");
            targetTags = new List<TargetTag>();
            if (IsTargetTags(item))
            {
                foreach (var target in item.TargetTags)
                {
                    ParseTargetTag(target, out string ttitle, out string taddress, out string tdesc);
                    if (String.IsNullOrWhiteSpace(taddress) == false)
                    {
                        targetTags.Add(new TargetTag { title = ttitle, address = taddress, desc = tdesc });
                    }
                }
            }
        }
        static public void ParseTargetTag(dynamic item, out string title, out string address, out string desc)
        {
            address = JsonControl.GetString(item, "Address", "");
            title = JsonControl.GetString(item, "Title", $"index{address}");
            desc = JsonControl.GetString(item, "Desc", title);
        }

        // ------------------------------------------------------------------------------

        // Установить код для списка тегов
        static public void CodeMessageList<T>(List<T> tags, CodeMessage codeMessage) where T : ICodeMessage
        {
            if (tags == null || tags.Any() == false)
                return;

            var count = tags.Count();
            for (var i = 0; i < count; i++)
            {
                //LogHelper.LogApp($"CodeMessageList={i}");
                tags[i].codeMessage = codeMessage;
            }
            return;
        }


        // Запомнить теги для подстановок
        public void SetInnerTagsForOneTag()
        {
            InnerTagIds = FindInnerTags();
            InnerTags = items.Where(x => InnerTagIds.Contains(x.Id)).ToArray();
        }

        // Определение тегов для подстановок
        public ushort[] FindInnerTags()
        {
            string addr = Address;
            List<ushort> result = new List<ushort>();
            if (addr.Contains("{") && addr.Contains("}"))
            {
                foreach (var item in Tag.items)
                {
                    string nameValue = "{" + item.title + "}"; // значение
                    string paramValue = "{" + item.title + "."; // параметр тега
                    string nameIndex = "{" + item.title + "["; // значение из списка

                    if (addr.Contains(nameValue))
                    {
                        result.Add(item.Id);
                        continue;
                    }
                    if (addr.Contains(paramValue))
                    {
                        result.Add(item.Id);
                        continue;
                    }
                    if (addr.Contains(nameIndex))
                    {
                        result.Add(item.Id);
                        continue;
                    }
                }
                return result.ToArray();
            }
            return new ushort[] { };
        }

        // Запомнить теги для подстановок
        static public void SetInnerTagsForAllTags()
        {
            foreach (var item in items)
                item.SetInnerTagsForOneTag();
        }

        // Получение адреса тега с подстановками
        static public string ExpTagAddress(string address, out bool success, ITagClient tag = null)
        {
            success = true;

            if (tag != null && (tag.InnerTagIds == null || tag.InnerTagIds.Any() == false))
                return address;

            if (address.Contains("{") && address.Contains("}"))
            {
                //var itemsList = items.ToList();
                foreach (var item in (tag == null) ? items : items.Where(x => tag.InnerTagIds.Contains(x.Id)))
                {
                    string nameValue = "{" + item.title + "}"; // значение
                    string goodValue = "{" + item.title + ".good}"; // тег хорошего качества
                    string codeValue = "{" + item.title + ".code}"; // код тега
                    string titleValue = "{" + item.title + ".title}"; // имя тега
                    string descValue = "{" + item.title + ".desc}"; // описание тега
                    string nameIndex = "{" + item.title + "["; // значение из списка

                    bool good = item.Good; // || item.SimEnable; // new item.SimEnable
                    int code = item.codeMessage.сode;
                    dynamic lastValue = item.LastGoodValue;
                    dynamic actualValue = item.Value;

                    if (address.Contains(goodValue)) // качество тега
                    {
                        address = address.Replace(goodValue, Convert.ToString(good));
                    }
                    if (address.Contains(codeValue)) // код тега
                    {
                        address = address.Replace(codeValue, Convert.ToString(code));
                    }
                    if (address.Contains(titleValue)) // имя тега
                    {
                        address = address.Replace(titleValue, item.title);
                    }
                    if (address.Contains(descValue)) // описание тега
                    {
                        address = address.Replace(descValue, item.description);
                    }
                    if (address.Contains(nameValue)) // значение тега
                    {
                        //
                        if (code < 0 && tag?.title != item.title) // если тег с ошибкой и это не тот же тег
                            success = false;

                        //
                        if (lastValue != null) // && good
                        {
                            if (Tag.IsArray(lastValue))
                            {
                                address = address.Replace(nameValue, string.Join(";", lastValue));
                            }
                            else
                            {
                                address = address.Replace(nameValue, Convert.ToString(lastValue));
                            }
                        }
                        else
                        {
                            if (tag?.title != item.title) // если это не тот же тег
                                success = false;

                            if (item.DataType == eDataType.STRING || item.DataType == eDataType.Char)
                            {
                                address = address.Replace(nameValue, "");
                            }
                            else
                            {
                                address = address.Replace(nameValue, "0");
                            }
                        }
                    }
                    if (address.Contains(nameIndex)) // значение индекса тега
                    {
                        //
                        if (code < 0 && tag?.title != item.title) // если тег с ошибкой и это не тот же тег
                            success = false;

                        var iter = 0;
                        while (address.IndexOf(nameIndex) >= 0 && iter <= 1024)
                        {
                            var index1 = address.IndexOf(nameIndex);
                            if (index1 >= 0)
                            {
                                var index2 = address.IndexOf("]", index1);
                                if (index2 > 0)
                                {
                                    int start = index1 + nameIndex.Length;
                                    string num = address.Substring(start, index2 - start);
                                    bool ok = int.TryParse(num, out int indexArr);
                                    // Если индекс текстовый
                                    if (ok == false || indexArr == 0)
                                    {
                                        if (Tag.IsArray(lastValue))
                                        {
                                            foreach (string element in lastValue)
                                            {
                                                var partElement = element.Split('~'); // было =
                                                if (partElement.Length > 1)
                                                {
                                                    if (partElement[0] == num)
                                                    {
                                                        var partElementAt2 = partElement.Where(x => x != partElement[0]).ToArray(); // удаляем первый элемент из массива
                                                        //address = address.Replace(nameIndex + num + "]}", partElement[1]);
                                                        address = address.Replace(nameIndex + num + "]}", String.Join("~",partElementAt2));
                                                        break;
                                                    }
                                                } 
                                            }
                                        } else
                                        {

                                            if (lastValue != null)
                                            {
                                                var partElement = lastValue.Split('~'); // было =
                                                if (partElement.Length > 1)
                                                {
                                                    if (partElement[0] == num)
                                                    {
                                                        address = address.Replace(nameIndex + num + "]}", partElement[1]);
                                                    }
                                                }
                                            } else
                                            {
                                                if (tag?.title != item.title) // если это не тот же тег
                                                    success = false;
                                            }
                                        }
                                    }
                                    // Если индекс числовой
                                    if (ok)
                                    {
                                        try
                                        {
                                            if (lastValue != null) // && good
                                            {
                                                if (Tag.IsArray(lastValue))
                                                {
                                                    address = address.Replace(nameIndex + num + "]}", Convert.ToString(lastValue[indexArr - 1]));
                                                }
                                                else
                                                {
                                                    if (lastValue.GetType() == typeof(string))
                                                    {
                                                        string s = lastValue as string;
                                                        address = address.Replace(nameIndex + num + "]}", s[indexArr - 1].ToString());
                                                    }
                                                    else if (lastValue.GetType() == typeof(int))
                                                    {
                                                        BitArray b = new BitArray(new int[] { lastValue });
                                                        address = address.Replace(nameIndex + num + "]}", b[indexArr - 1].ToString());
                                                    }
                                                    else if (lastValue.GetType() == typeof(byte))
                                                    {
                                                        BitArray b = new BitArray(new byte[] { lastValue });
                                                        address = address.Replace(nameIndex + num + "]}", b[indexArr - 1].ToString());
                                                    }
                                                    else
                                                    {
                                                        address = address.Replace(nameIndex + num + "]}", Convert.ToString(lastValue));
                                                    }

                                                }

                                            }
                                            else
                                            {
                                                if (tag?.title != item.title) // если это не тот же тег
                                                    success = false;

                                                if (item.DataType == eDataType.STRING || item.DataType == eDataType.Char)
                                                {
                                                    address = address.Replace(nameIndex + num + "]}", "");
                                                }
                                                else
                                                {
                                                    address = address.Replace(nameIndex + num + "]}", "0");
                                                }
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            success = false;
                                            address = address.Replace(nameIndex + num + "]}", ex.Message);
                                        }
                                    }


                                }
                            }
                            iter++;
                        }

                    }
                }
            }
            return address;
        }

        #region View
        static public string ValueToSimpleText(ITagResult item)
        {
            try
            {
                if (item == null)
                    return "";

                dynamic value = item.Value;
                string text = "";

                if (value != null && item.Good)
                {
                    if (Tag.IsArray(value))
                    {
                        int count = 0;
                        foreach (var itemVal in value)
                        {
                            count++;
                        }
                        if (count >= 1)
                        {
                            text = $"[{count}]..{value[0]}";
                        }
                        else
                        {
                            text = $"[{value}]";
                        }
                    }
                    else
                    {
                        text = Convert.ToString(value);
                    }

                }
                else
                {
                    text = "";
                }

                return text;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        static public string ValueToAdvText(Tag item, string valueTrue = "True", string valueFalse = "False", string[] valueList = null)
        {
            var tagValue = item.LastGoodValue; // последнее достоверное значение
            if (tagValue != null)
            {
                if (Tag.IsArray(tagValue)) // tagValue is IEnumerable && tagValue.GetType() != typeof(string)
                {
                    return String.Join(GetSepatator(item.DataType), tagValue);
                }
                else
                {
                    if (item.DataType == eDataType.Bool)
                    {
                        tagValue = (tagValue == true) ? valueTrue : valueFalse;
                    }
                    else if (IndexDataType(item.DataType) && valueList != null && valueList.Length > 0)
                    {
                        if (tagValue >= 0 && tagValue < valueList.Length)
                        {
                            tagValue = valueList[tagValue];
                        }
                    }
                    return Convert.ToString(tagValue);
                }
            }
            else
            {
                return "";
            }
        }
        static public string GetSepatator(eDataType dataType)
        {
            if (dataType == eDataType.ArrayA)
                return ";";
            if (dataType == eDataType.ArrayB)
                return " ";
            if (dataType == eDataType.ArrayC)
                return "~";

            return "; ";
        }
        #endregion

        #region Info

        // Зависимости адреса
        static public string AddressLinks(string address, string tagTitle = "")
        {
            string retval = "";
            if (address.Contains("{") && address.Contains("}"))
            {
                foreach (var item in items)
                {
                    if (item.title != tagTitle) // защита от бесконечного цикла (?)
                    {
                        string nameValue = "{" + item.title + "}"; // значение
                        string goodValue = "{" + item.title + ".good}"; // тег хорошего качества
                        string codeValue = "{" + item.title + ".code}"; // код тега
                        string titleValue = "{" + item.title + ".title}"; // имя тега
                        string descValue = "{" + item.title + ".desc}"; // описание тега
                        string nameIndex = "{" + item.title + "["; // значение из списка


                        if (address.Contains(goodValue) ||  // качество тега
                             address.Contains(codeValue) || // код тега
                             address.Contains(titleValue) || // имя тега
                             address.Contains(descValue) || // описание тега
                             address.Contains(nameValue) || // значение тега
                             address.Contains(nameIndex)) // значение индекса тега
                        {
                            string sk = AddressLinks(item.Address, item.title);
                            if (String.IsNullOrWhiteSpace(sk) == false)
                            {
                                sk = $"<-({sk})";
                            }
                            retval += $" {item.title}={item.Address}{sk} ";
                        }
                    }
                }
            }
            return retval;
        }

        // Зависимости тега
        static public List<string> TagLinks(List<Tag> tagItems, string address, string tagTitle = "", bool levels = true)
        {
            List<string> retval = new List<string>();
            if (address.Contains("{") && address.Contains("}"))
            {
                foreach (var item in tagItems)
                {
                    if (item.title != tagTitle) // защита от бесконечного цикла (?)
                    {
                        string nameValue = "{" + item.title + "}"; // значение
                        string goodValue = "{" + item.title + ".good}"; // тег хорошего качества
                        string codeValue = "{" + item.title + ".code}"; // код тега
                        string titleValue = "{" + item.title + ".title}"; // имя тега
                        string descValue = "{" + item.title + ".desc}"; // описание тега
                        string nameIndex = "{" + item.title + "["; // значение из списка

                        if (address.Contains(goodValue) ||  // качество тега
                             address.Contains(codeValue) || // код тега
                             address.Contains(titleValue) || // имя тега
                             address.Contains(descValue) || // описание тега
                             address.Contains(nameValue) || // значение тега
                             address.Contains(nameIndex)) // значение индекса тега
                        {
                            retval.Add(item.title);

                            if (levels)
                                retval.AddRange(TagLinks(tagItems, item.Address, item.title));

                        }
                    }
                }
            }
            return retval;
        }
        #endregion

        #region Converter

        // Преобразование типов
        static public dynamic ConvertValue(dynamic value, eDataType dataType)
        {
            if (value == null)
            {
                if (dataType == eDataType.STRING)
                    return "";
                return 0;
            }

            switch (dataType)
            {
                case eDataType.Bool:
                    if (value is string)
                    {
                        var svalue = ((string)value).ToLower();
                        if (svalue == "1" || svalue == "true" || svalue == "yes")
                            return true;
                        return (bool.TryParse(value, out bool result)) ? result : false;
                    }
                    return Convert.ToBoolean(value);

                case eDataType.Byte:
                    if (value is string)
                    {
                        bool boolOK = byte.TryParse(value, out byte result);
                        if (boolOK)
                            return result;

                        byte[] arr = Encoding.ASCII.GetBytes(value);
                        if (arr.Length == 1)
                            return arr[0];
                        return arr;
                    }
                    if (value < 0)
                        value = 0;
                    if (value > 255)
                        value = 255;
                    return Convert.ToByte(value);

                case eDataType.Binary:
                    if ((value is string) == false)
                    {
                        value = Convert.ToInt16(value);
                        BitArray b = new BitArray(new int[] { value });
                        bool[] bits = new bool[b.Count];
                        b.CopyTo(bits, 0);
                        return bits;
                    }
                    break;

                case eDataType.ArrayA:
                    if ((value is string))
                    {
                        string[] arr = ((string)value).Split(';');
                        return arr;
                    }
                    else
                    {
                        return new string[] { Convert.ToString(value) };
                    }

                case eDataType.ArrayB:
                    if ((value is string))
                    {
                        string[] arr = ((string)value).Split(' ');
                        return arr;
                    }
                    else
                    {
                        return new string[] { Convert.ToString(value) };
                    }

                case eDataType.ArrayC:
                    if ((value is string))
                    {
                        string[] arr = ((string)value).Split('~');
                        return arr;
                    }
                    else
                    {
                        return new string[] { Convert.ToString(value) };
                    }

                case eDataType.HEX:
                    if ((value is string) == false)
                    {
                        return Convert.ToInt32(value).ToString("X");
                    }
                    break;

                case eDataType.Char:
                    if (value is string)
                    {
                        dynamic SValue = (string)value;
                        if (SValue.Length > 1)
                            return ((string)SValue).ToCharArray();
                        return (char.TryParse(SValue, out char result)) ? result : ' ';
                    }
                    return Convert.ToChar(Convert.ToInt32(value));

                case eDataType.Short:
                    if (value is string)
                    {
                        value = DecimalString(value);
                        return short.TryParse(value, out short retvalInt1) ? retvalInt1 : 0;
                    }
                    return Convert.ToInt16(value);

                case eDataType.UShort:
                    if (value is string)
                    {
                        value = DecimalString(value);
                        return ushort.TryParse(value, out ushort retvalInt2) ? retvalInt2 : 0;
                    }
                    return Convert.ToUInt16(value);

                case eDataType.Int:
                    if (value is string)
                    {
                        value = DecimalString(value);
                        return int.TryParse(value, out int retvalInt3) ? retvalInt3 : 0;
                    }
                    return Convert.ToInt32(value);

                case eDataType.UInt:
                    if (value is string)
                    {
                        value = DecimalString(value);
                        return uint.TryParse(value, out uint retvalInt4) ? retvalInt4 : 0;
                    }
                    return Convert.ToUInt32(value);

                case eDataType.Long:
                    if (value is string)
                    {
                        value = DecimalString(value);
                        return long.TryParse(value, out long retvalInt5) ? retvalInt5 : 0;
                    }
                    return Convert.ToInt64(value);

                case eDataType.Float:
                    if (value is string)
                    {
                        return ToFloat(DecimalString(value));
                    }
                    return Convert.ToSingle(value);

                case eDataType.Double:
                    if (value is string)
                    {
                        return ToDouble(DecimalString(value));
                    }
                    return Convert.ToDouble(value);

                case eDataType.STRING:
                    return Convert.ToString(value);
            }

            return value;
        }


        // Получить значение из строки в зависимости от типа
        //static public dynamic GetValueByType(string value, eDataType dataType)
        //{
        //    dynamic retval = null;
        //    switch (dataType)
        //    {
        //        case eDataType.Bool:
        //            if (value.ToLower() == "true" || (value != "0" && value.ToLower() != "false"))
        //            {
        //                retval = true;
        //            }
        //            else
        //            {
        //                retval = false; // bool.TryParse(value, out bool retvalBool) ? retvalBool : false;
        //            }

        //            break;

        //        case eDataType.Byte:
        //            retval = byte.TryParse(value, out byte retvalByte) ? retvalByte : 0;
        //            break;

        //        case eDataType.Short:
        //        case eDataType.UShort:
        //        case eDataType.Int:
        //            retval = int.TryParse(value, out int retvalInt) ? retvalInt : 0;
        //            break;

        //        case eDataType.Long:
        //            retval = long.TryParse(value, out long retvalLong) ? retvalLong : 0;
        //            break;

        //        case eDataType.Float:
        //            retval = float.TryParse(value, out float retvalFloat) ? retvalFloat : 0;
        //            break;

        //        case eDataType.Double:
        //            retval = double.TryParse(value, out double retvalDouble) ? retvalDouble : 0;
        //            break;

        //        case eDataType.STRING:
        //            retval = value;
        //            break;
        //    }

        //    return retval;
        //}
        // ---------------------------------------------------
        static private float ToFloat(dynamic value)
        {
            string SValue = DecimalString(value);
            bool floatOK1 = float.TryParse(SValue.Replace(",", "."), out float result);
            if (floatOK1)
            {
                return result;
            }
            else
            {
                bool floatOK2 = float.TryParse(SValue.Replace(".", ","), out float result2);
                if (floatOK2)
                {
                    return result2;
                }
                else
                {
                    return 0;
                }
            }
        }

        static private double ToDouble(dynamic value)
        {
            string SValue = DecimalString(value);
            bool doubleOK1 = double.TryParse(SValue.Replace(",", "."), out double result);
            if (doubleOK1)
            {
                return result;
            }
            else
            {
                bool doubleOK2 = double.TryParse(SValue.Replace(".", ","), out double result2);
                if (doubleOK2)
                {
                    return result2;
                }
                else
                {
                    return 0;
                }
            }
        }

        // ---------------------------------------------------
        // Преобразование типов с поддержкой массивов
        static public dynamic ConvertValueWithArray(dynamic value, eDataType dataType)
        {
            if (Tag.IsArray(value)) // && dataType != eDataType.STRING && dataType != eDataType.Char
            {
                List<dynamic> arrValues = new List<dynamic>();
                foreach (var item in value)
                {
                    arrValues.Add(Tag.ConvertValue(item, dataType));
                }
                value = arrValues;
            }
            else
            {
                value = Tag.ConvertValue(value, dataType);
            }

            return value;
        }
        
        // Получение строки, содержащей только числовые символы
        static private string DecimalString(dynamic value)
        {
            string SValue = value;
            SValue = String.Join("", SValue.Where(x => x == '-' || x == '.' || x == ',' || x == '-' || (x >= '0' && x <= '9')).ToArray());
            if (SValue[0] == '-')
            {
                SValue = "-" + SValue.Replace("-", "");
            }
            return SValue;
        }
        
        // Значение или хначения в строку
        static public string ValuesString(dynamic value)
        {
            if (Tag.IsArray(value))
            {
                return String.Join($";", value);
            }
            else
            {
                return Convert.ToString(value);
            }
        }

        #endregion

        #region Check types
        // Является ли тип числовым?
        static public bool NumberDataType(eDataType dataType)
        {
            switch (dataType)
            {
                case eDataType.Byte:
                case eDataType.Short:
                case eDataType.UShort:
                case eDataType.Int:
                case eDataType.UInt:
                case eDataType.Long:
                case eDataType.Float:
                case eDataType.Double:
                    return true;
            }

            return false;
        }
        // Подходит ли тип для индекса?
        static public bool IndexDataType(eDataType dataType)
        {
            switch (dataType)
            {
                case eDataType.Byte:
                case eDataType.Short:
                case eDataType.UShort:
                case eDataType.Int:
                case eDataType.UInt:
                case eDataType.Long:
                    return true;
            }

            return false;
        }
        static public bool IsArray(dynamic value)
        {
            if (value == null)
                return false;

            if (value is IEnumerable && value.GetType() != typeof(string))
                return true;

            return false;
        }

        #endregion


    }

    // ===================================================================================================================

    #region EDITOR

    public class TagEditor // Редактирование
    {
        public uint Id; // Уникальный идентификатор (0 - нет Id)
        public string title; // Название
        public eDataType dataType;
        public uint sourceId; // ID драйвера
        public uint groupId; // ID группы
        public string sourceTitle; // Название драйвера
        public string groupTitle; // Название группы
        public string address; // адрес
        public bool off; // отключение
        public bool isCommand; // запрос по команде
        public string writeTitle; // Источник новых значений (имя тега)
        public string constValue; // Записываемое значение
        public string description; // Описание
        public string block; // Блок
    }

    public struct TargetTag
    {
        public string title;
        public string desc;
        public string address;
    }

    public class StructureEditor
    {
        public uint Id; // Уникальный идентификатор (0 - нет Id)
        public string title { get; set; } // Название
        public string join { get; set; } // Соединитель
        public eDataType dataType { get; set; } // Тип данных
        public string tagSource { get; set; } // тег-источник
        public string templateAddress { get; set; } // шаблон адреса
        public string group { get; set; } // группа
        
        //public string sourceTags { get; set; } // tag1;tag2;tag3
    }
    public class StructTargetEditor
    {
        public uint Id; // Уникальный идентификатор (0 - нет Id)
        public string structureTitle { get; set; }  // Название структуры
        public string title { get; set; }  // Название тега
        public string innerAddress { get; set; }  // Адрес
        public string desc { get; set; }  // Описание тега
    }

    public class StructTagEditor
    {
        public uint Id; // Уникальный идентификатор (0 - нет Id)
        public string structureTitle { get; set; }  // Название структуры
        public string title { get; set; }  // Название тега
    }

    #endregion
}
