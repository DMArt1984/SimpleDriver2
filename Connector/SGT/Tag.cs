using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DML.Log;
using LogCodeMessage;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Connector
{
    public class Tag : BaseLogger, ITagStatus, ITagClient, ITagResult, IAppendTag
    {
        public static class CM
        {
            public static readonly CodeMessage Good = new CodeMessage(0, null);
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
            public static readonly CodeMessage NotSupport = new CodeMessage(-702, "Тип данных не поддерживается");
        }

        public ushort Id { get; }
        public string title { get; }
        public string description { get; }

        public bool lic = false; // тег для контроля лицензии

        // Конструктор
        public Tag(ushort Id, string title, Group parentGroup, eDataType dataType, string address, string description = "")
            : base(LogTarget.FileConsoleForm, null)
        {
            this.Id = Id;
            this.title = title;
            this.description = description;
            _dataType = dataType;
            _address = address;

            ParentGroup = parentGroup;
            ParentGroup.AddTag(this);
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

        public void SetResult(TagResult result)
        {
            if (!SimEnable)
            {
                _codeMessage = result.codeMessage;
                if (_codeMessage.code == 0)
                {
                    _status = eTagStatus.good;
                }
                else
                {
                    _status = eTagStatus.error;
                }
                value = result.value;
            }
        }

        public dynamic value
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

                RaiseRuntimeEvent();
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
                    RaiseRuntimeEvent();
                }
            }
        }
        private CodeMessage _codeMessage = new CodeMessage();

        public eTagStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    RaiseRuntimeEvent();
                }
            }
        }
        private eTagStatus _status = eTagStatus.zero;

        public bool Good => Status == eTagStatus.good;

        #region Runtime

        public void UpdateStatus()
        {
            Status = ReSelectStatus(Status);
        }

        public eTagStatus ReSelectStatus(eTagStatus status)
        {
            if (ParentGroup == null)
                return eTagStatus.groupDisable;

            if (ParentGroup.ParentSource == null)
                return eTagStatus.sourceDisable;

            eGroupStatus groupStatus = ParentGroup.Status;
            switch (groupStatus)
            {
                case eGroupStatus.zero:
                case eGroupStatus.Off:
                    return eTagStatus.groupDisable;
            }

            eSourceStatus sourceStatus = ParentGroup.ParentSource.Status;
            switch (sourceStatus)
            {
                case eSourceStatus.closed:
                case eSourceStatus.closing:
                case eSourceStatus.opening:
                case eSourceStatus.errOpen:
                case eSourceStatus.errClose:
                case eSourceStatus.noClient:
                case eSourceStatus.wait:
                case eSourceStatus.breaking:
                    return eTagStatus.sourceDisable;
            }

            return status;
        }
        #endregion

        #region Events

        public delegate void HandlerTagRuntime(ushort Id);
        public event HandlerTagRuntime eventRuntime;

        public delegate void HandlerTagParam(TagParam info);
        public event HandlerTagParam eventParam;

        private void RaiseRuntimeEvent()
        {
            SafeInvokeHandlerTagRuntime(eventRuntime, Id);
        }
        private void EventChangeParam(bool noSetTagON = false)
        {
            if (Off)
            {
                Status = eTagStatus.tagOff;
            }
            else if (!noSetTagON)
            {
                Status = eTagStatus.tagOn;
            }
            SafeInvokeHandlerTagParam(eventParam, new TagParam(Id, Off, Address, DataType, GetWriteCell()));
        }
        public string GetWriteCell()
        {
            return !string.IsNullOrWhiteSpace(WriteTagTitle) ? WriteTagTitle : WriteConstValue == null ? null : string.Join(";", WriteConstValue);
        }
        #endregion

        #region Setting

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
                    this.value = null;
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
                    this.value = null;
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
                    appendTag = Tag.Item(value);
                    EventChangeParam(true);
                }
            }
        }
        private ushort _writeTagId = 0;
        public string WriteTagTitle => _writeTagTitle;
        private string _writeTagTitle;
        private IAppendTag appendTag = null;
        public dynamic WriteTagValue => appendTag?.LastGoodValue;

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
        public eDirectFull directFull =>
            (WriteTagId == 0 && WriteConstValue == null)
                ? eDirectFull.Read
                : (WriteTagId > 0 ? eDirectFull.WriteTagValue : eDirectFull.WriteConstValue);

        public bool SimEnable = false;
        public dynamic SimValue = null;
        public void SimOnOff(bool enable, dynamic value = null)
        {
            SimEnable = enable;
            SimValue = value;
            if (SimEnable)
            {
                codeMessage = Tag.CM.Good;
                this.value = value;
            }
            else
            {
                codeMessage = Tag.CM.Good;
            }
        }
        #endregion

        #region Builder

        public Tag[] InnerTags { get; set; } // Массив внутренних тегов, соответствующих тегам, найденным в адресе

        public Group ParentGroup { get; set; } // Группа тега

        private ushort _groupId = 0;
        public ushort groupId => _groupId;
        public string groupTitle => ParentGroup?.title ?? "";

        public string sourceTitle => ParentGroup?.ParentSource?.title ?? "";
        public ushort sourceId => ParentGroup?.ParentSource?.Id ?? 0;

        public void RebindGroup(Group newGroup)
        {
            if (ParentGroup == newGroup)
                return;

            if (ParentGroup != null)
            {
                ParentGroup.RemoveTag(this);
            }

            ParentGroup = newGroup;

            if (newGroup != null)
            {
                newGroup.AddTag(this);
            }
        }

        public void SetLinkIdTitle()
        {
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

        public void SetInnerTagsForOneTag()
        {
            InnerTags = items
                .Where(item => Address.Contains($"{{{item.title}}}") ||
                               Address.Contains($"{{{item.title}.") ||
                               Address.Contains($"{{{item.title}["))
                .ToArray();
        }
        #endregion

        #region Static

        // Объект-замок для статической коллекции тегов
        private static readonly object _tagItemsLock = new object();

        public static List<Tag> items = new List<Tag>();
        public static ushort lastId = 0;
        public static bool log = false;

        public static void Clear()
        {
            lock (_tagItemsLock)
            {
                lastId = 0;
                items = new List<Tag>();
            }
        }

        public static Tag Item(ushort Id)
        {
            lock (_tagItemsLock)
            {
                return items.FirstOrDefault(x => x.Id == Id);
            }
        }
        public static Tag Item(string title)
        {
            lock (_tagItemsLock)
            {
                return items.FirstOrDefault(x => x.title == title);
            }
        }

        public static void CalcId()
        {
            lock (_tagItemsLock)
            {
                foreach (var item in items)
                {
                    item.SetLinkIdTitle();
                }
            }
        }

        public static void UpdateStatusForList<T>(List<T> tags) where T : ITagStatus
        {
            if (tags == null || !tags.Any())
                return;

            foreach (var tag in tags)
            {
                tag.UpdateStatus();
            }
        }
        #endregion

        // ==========================

        // Безопасный вызов для HandlerTagRuntime (принимает ushort Id)
        private void SafeInvokeHandlerTagRuntime(HandlerTagRuntime handler, ushort id)
        {
            if (handler == null)
                return;
            foreach (HandlerTagRuntime subscriber in handler.GetInvocationList())
            {
                try
                {
                    subscriber(id);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.HResult, $"Ошибка в обработчике HandlerTagRuntime: {ex.Message}", eMessageCategory.Source);
                }
            }
        }

        // Безопасный вызов для HandlerTagParam (принимает TagParam)
        private void SafeInvokeHandlerTagParam(HandlerTagParam handler, TagParam info)
        {
            if (handler == null)
                return;
            foreach (HandlerTagParam subscriber in handler.GetInvocationList())
            {
                try
                {
                    subscriber(info);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.HResult, $"Ошибка в обработчике HandlerTagParam: {ex.Message}", eMessageCategory.Source);
                }
            }
        }

    }
}
