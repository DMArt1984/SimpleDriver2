using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DML;
using LogCodeMessage;

namespace Connector
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
        public TagResult(dynamic value)
        {
            this.value = value;
            this.codeMessage = CodeMessageFactory.FromEnumX(eTagCode.good);
        }
        public TagResult(dynamic value, Exception ex)
        {
            this.value = value;
            this.codeMessage = CodeMessageFactory.FromException(ex);
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

        public bool Good => codeMessage.code == (int)eTagCode.good; // Тег достоверный // _off == false && 

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


        //
        public Group ParentGroup { get; }

        // Конструктор
        public Tag(ushort Id, string title, Group parentGroup, eDataType dataType, string address, string description = "")
        {
            this.Id = Id;
            this.title = title;
            this.description = description;
            _dataType = dataType;
            _address = address;

            ParentGroup = parentGroup;
            ParentGroup.AddTag(this);
        }

        // Определить недостающие ID-Title
        public void SetLinkIdTitle()
        {
            // source ID and title
            if (String.IsNullOrWhiteSpace(_sourceTitle) == false)
            {
                var source = Source.Item(_sourceTitle);
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
                var source = Source.Item(_sourceId);
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
                //var group = Group.Item(_groupTitle);
                //if (group != null)
                //{
                //    _groupId = group.Id;
                //}
                //else // создаем группу с заданным названием
                //{
                //    _groupId = ++Group.lastId;
                //    Group.items.Add(new Group(_groupId, _groupTitle, 100, false, "Создан динамически по имени"));
                //}
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
                    //_groupId = ++Group.lastId;
                    //_groupTitle = $"GroupID{Group.lastId}";
                    //Group.items.Add(new Group(_groupId, _groupTitle, 100, false, "Создан динамически"));
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
                if (_codeMessage.code != value.code)
                {
                    _codeMessage = value;
                    CheckLastError();
                    EventChangeCodeMessage();
                }
                
            }
        }
        CodeMessage _codeMessage = CodeMessageFactory.FromEnumX(eTagCode.created);

        public CodeMessage LastError => _lastError;
        CodeMessage _lastError;

        private void CheckLastError()
        {
            if (_codeMessage.code < 0)
            {
                _lastError = new CodeMessage(_codeMessage.code, _codeMessage.message);
            }
        }

        public void ClearLastError()
        {
            _lastError = new CodeMessage();
            CheckLastError();
            EventChangeCodeMessage();
        }

        void EventChangeParam(bool noSetTagON = false)
        {
            if (Off)
            {
                codeMessage = CodeMessageFactory.FromEnumX(eTagCode.tagOff);
            } else if (noSetTagON == false)
            {
                codeMessage = CodeMessageFactory.FromEnumX(eTagCode.tagOn);
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
                codeMessage = CodeMessageFactory.FromEnumX(eTagCode.good);
                Value = value;
            } else
            {
                codeMessage = CodeMessageFactory.FromEnumX(eTagCode.created);
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


    }

}
