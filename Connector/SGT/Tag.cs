using System;
using System.Collections.Generic;
using System.Linq;
using LogCodeMessage;

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

    public struct TagResult
    {
        public dynamic value;
        public CodeMessage codeMessage;

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

    public class Tag : ICodeMessage, ITagClient, ITagResult, IAppendTag
    {
        public ushort Id { get; }
        public string title { get; }
        public string description { get; }

        private ushort _sourceId = 0;
        public ushort sourceId => _sourceId;
        private ushort _groupId = 0;
        public ushort groupId => _groupId;

        private string _sourceTitle = "";
        public string sourceTitle => _sourceTitle;
        private string _groupTitle = "";
        public string groupTitle => _groupTitle;

        public bool Good => codeMessage.code == (int)eTagCode.good;

        public bool SimEnable = false;
        public dynamic SimValue = null;

        public bool lic = false;

        public delegate void HandlerCode(ushort Id, CodeMessage activeCM, CodeMessage lastError);
        public event HandlerCode eventCode;

        public delegate void HandlerParam(TagParam info);
        public event HandlerParam eventParams;

        public delegate void HandlerValue(ushort Id);
        public event HandlerValue eventValue;

        public ushort[] InnerTagIds { get; set; }
        public Tag[] InnerTags { get; set; }

        public Group ParentGroup { get; }

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

        public void SetLinkIdTitle()
        {
            if (!string.IsNullOrWhiteSpace(_sourceTitle))
            {
                var source = Source.Item(_sourceTitle);
                if (source != null)
                {
                    _sourceId = source.Id;
                }
                else if (Source.lastId == 1)
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
                }
                else if (Source.lastId == 1)
                {
                    _sourceId = Source.items[0].Id;
                    _sourceTitle = Source.items[0].title;
                }
            }

            if (!string.IsNullOrWhiteSpace(_groupTitle))
            {
                //var group = Group.Item(_groupTitle);
                //if (group != null)
                //{
                //    _groupId = group.Id;
                //}
                //else
                //{
                //    _groupId = ++Group.lastId;
                //    Group.items.Add(new Group(_groupId, _groupTitle, 100, false, "Создан динамически по имени"));
                //}
            }
            else if (_groupId > 0)
            {
                var group = Group.Item(_groupId);
                if (group != null)
                {
                    _groupTitle = group.title;
                }
            }

            if (_groupId == 0 && string.IsNullOrWhiteSpace(_groupTitle))
            {
                if (Group.lastId == 1)
                {
                    _groupId = Group.items[0].Id;
                    _groupTitle = Group.items[0].title;
                }
                else
                {
                    //_groupId = ++Group.lastId;
                    //_groupTitle = $"GroupID{Group.lastId}";
                    //Group.items.Add(new Group(_groupId, _groupTitle, 100, false, "Создан динамически"));
                }
            }

            if (!string.IsNullOrWhiteSpace(_writeTagTitle) && title != _writeTagTitle)
            {
                var tag = Tag.Item(_writeTagTitle);
                if (tag != null)
                {
                    WriteTagId = tag.Id;
                }
                else
                {
                    _writeTagTitle = "";
                }
            }
            else if (WriteTagId > 0 && Id != WriteTagId)
            {
                var tag = Tag.Item(WriteTagId);
                if (tag != null)
                {
                    _writeTagTitle = tag.title;
                }
                else
                {
                    WriteTagId = 0;
                }
            }
        }

        public static void CalcId()
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
        }

        public void SetParam(bool off, bool command, string writeTagTitle = "", dynamic writeValue = null)
        {
            _off = off;
            _writeConstValue = writeValue;
            _writeTagTitle = writeTagTitle;
            Command = command ? eCommand.Wait : eCommand.None;
        }

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
        private eDataType _dataType;

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
        private string _address;

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
        private bool _off = false;

        public dynamic WriteConstValue
        {
            get => _writeConstValue;
            set
            {
                bool change;
                try
                {
                    change = _writeConstValue != value;
                }
                catch
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
        private dynamic _writeConstValue = null;

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
        private ushort _writeTagId = 0;
        public string WriteTagTitle => _writeTagTitle;
        private string _writeTagTitle;
        private IAppendTag appendValue = null;
        public dynamic WriteTagValue => appendValue?.LastGoodValue;

        public string GetWriteCell()
        {
            return !string.IsNullOrWhiteSpace(WriteTagTitle) ? WriteTagTitle : WriteConstValue == null ? null : string.Join(";", WriteConstValue);
        }

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
        private eCommand _command = eCommand.None;

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
        private dynamic _value = null;

        public string LastDTUpdate => _lastDTUpdate;
        private string _lastDTUpdate = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff");

        public dynamic LastGoodValue => _lastGoodValue;
        private dynamic _lastGoodValue = null;

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
        private CodeMessage _codeMessage = CodeMessageFactory.FromEnumX(eTagCode.created);

        public CodeMessage LastError => _lastError;
        private CodeMessage _lastError;

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

        private void EventChangeParam(bool noSetTagON = false)
        {
            if (Off)
            {
                codeMessage = CodeMessageFactory.FromEnumX(eTagCode.tagOff);
            }
            else if (!noSetTagON)
            {
                codeMessage = CodeMessageFactory.FromEnumX(eTagCode.tagOn);
            }
            eventParams?.Invoke(new TagParam(Id, Off, Address, DataType, GetWriteCell()));
        }

        private void EventChangeCodeMessage()
        {
            eventCode?.Invoke(Id, codeMessage, LastError);
        }

        public void SetResult(TagResult result)
        {
            if (!SimEnable)
            {
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
            }
            else
            {
                codeMessage = CodeMessageFactory.FromEnumX(eTagCode.created);
            }
        }

        public static List<Tag> items = new List<Tag>();
        public static ushort lastId = 0;
        public static bool log = false;

        public static void Clear()
        {
            lastId = 0;
            items = new List<Tag>();
        }

        public static Tag Item(ushort Id) => items.FirstOrDefault(x => x.Id == Id);
        public static Tag Item(string title) => items.FirstOrDefault(x => x.title == title);

        public static void CodeMessageList<T>(List<T> tags, CodeMessage codeMessage) where T : ICodeMessage
        {
            if (tags == null || !tags.Any())
                return;

            foreach (var tag in tags)
            {
                tag.codeMessage = codeMessage;
            }
        }

        public void SetInnerTagsForOneTag()
        {
            InnerTagIds = FindInnerTags();
            InnerTags = items.Where(x => InnerTagIds.Contains(x.Id)).ToArray();
        }

        public ushort[] FindInnerTags()
        {
            if (Address.Contains("{") && Address.Contains("}"))
            {
                return items.Where(item => Address.Contains($"{{{item.title}}}") || Address.Contains($"{{{item.title}.") || Address.Contains($"{{{item.title}[")).Select(item => item.Id).ToArray();
            }
            return Array.Empty<ushort>();
        }
    }
}
