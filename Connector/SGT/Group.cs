using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DML.Log;
using LogCodeMessage;

namespace Connector
{
    public class Group : BaseLogger, IGroupOff, IDisposable
    {
        public ushort Id { get; }
        public string title { get; }
        public string description { get; }


        // Конструктор
        public Group(ushort id, string title, Source parentSource, uint updateRate = 100, bool waitOff = false, string description = "")
            : base(LogTarget.FileConsoleForm, null)
        {
            Id = id;
            this.title = title;
            this.description = description;
            UpdateRate = updateRate;
            _waitOff = waitOff;
            ParentSource = parentSource;
            logger.Info($"new group ID {Id} {title} {updateRate}", eMessageCategory.Source);
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }

        #region Events

        public delegate void HandlerParam(GroupParamStatus info);
        public event HandlerParam eventParams;

        public delegate void HandlerReq(IGroupOff group);
        public event HandlerReq tikTakReq;

        public delegate void HandlerGroupStatus(ushort Id, eGroupStatus status);
        public event HandlerGroupStatus eventStatus;

        private void RaiseParamStatusChanged()
        {
            SafeInvokeHandlerParam(eventParams, new GroupParamStatus(Id, _updateRate, _off, IsTimerStopped));
        }

        #endregion

        #region Runtime
        public eGroupStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    Tag.UpdateStatusForList(Tags.Cast<ITagStatus>().ToList());
                    SafeInvokeHandlerGroupStatus(eventStatus, this.Id, _status);
                }
            }
        }
        eGroupStatus _status = eGroupStatus.zero;
        #endregion

        #region Timer

        private Timer _timer;
        private bool _timerStop = false;
        private int _tickCount = 0;

        private uint _updateRate = 100;
        public uint UpdateRate
        {
            get => _updateRate;
            set
            {
                if (_updateRate != value)
                {
                    _updateRate = value <= 0 ? 100u : value;
                    RaiseParamStatusChanged();
                    _timer?.Change(0, (int)_updateRate);
                }
            }
        }

        private void StartTimer()
        {
            if (_timer == null)
            {
                _timerStop = false;
                // Вычисляем начальное смещение, например, на основе ID группы или случайное значение
                // Такой механизм поможет распределить нагрузку равномернее и снизить вероятность одновременного вызова обработчика событий
                int initialDelay = new Random().Next(0, 100); // случайное смещение от 0 до 100 мс
                _timer = new Timer(TimerCallback, null, initialDelay, (int)UpdateRate);
                OnAndTimerStart();
            }
        }

        private void StopTimer()
        {
            if (_timer != null)
            {
                _timerStop = true;
            }
            OffAndTimerStop();
        }

        private bool IsTimerStopped => _timer == null;

        private void TimerCallback(object state)
        {
            _tickCount++;
            SafeInvokeHandlerReq(tikTakReq, this);

            if (_timerStop || Id == 0 || Off)
            {
                _timer?.Dispose();
                _timer = null;
                _timerStop = false;
                _tickCount = 0;
                SendStatusOff();
            }
        }
        public void SendStatusOff()
        {
            OffAndTimerStop();
            RaiseParamStatusChanged();
        }

        public void OnAndTimerStart()
        {
            Status = eGroupStatus.On;
            RaiseParamStatusChanged();
        }

        public void OffAndTimerStop()
        {
            Status = eGroupStatus.Off;
            RaiseParamStatusChanged();
        }
        #endregion

        #region Setting
        private bool _off = true;
        public bool Off
        {
            get => _off;
            set
            {
                if (_off != value)
                {
                    _off = value;
                    if (!_off)
                    {
                        StartTimer();
                    }
                    else
                    {
                        StopTimer();
                    }
                    
                }
            }
        }

        private bool _waitOff = false;
        public void Start()
        {
            Off = _waitOff;
        }
        public void Stop()
        {
            _waitOff = Off;
            Off = false;
        }
        #endregion


        public override bool Equals(object obj)
        {
            if (obj is Group other)
            {
                return Id == other.Id || title == other.title;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ (title?.GetHashCode() ?? 0);
        }

        #region Builder
        private Source _parentSource;
        public Source ParentSource
        {
            get => _parentSource;
            set
            {
                if (_parentSource != null)
                {
                    // Отписываем старый обработчик события
                    tikTakReq -= _parentSource.EventRequest;
                }
                _parentSource = value;
                if (_parentSource != null)
                {
                    // Подписываем новый обработчик события
                    tikTakReq -= _parentSource.EventRequest; // на всякий случай отписываем (чтобы избежать дублирования)
                    tikTakReq += _parentSource.EventRequest;
                }
            }
        }

        /// <summary>
        /// Метод перепривязки Source для группы.
        /// Отписывает группу от старого источника и привязывает к новому.
        /// </summary>
        /// <param name="newSource">Новый объект Source, который будет установлен как родительский для группы.</param>
        public void RebindSource(Source newSource)
        {
            // Если новый источник совпадает со старым, ничего не меняем.
            if (newSource == _parentSource)
                return;

            // Отписываемся от событий старого источника, если он задан.
            if (_parentSource != null)
            {
                tikTakReq -= _parentSource.EventRequest;
            }

            // Устанавливаем новый источник
            ParentSource = newSource;

            // Если необходимо, можно вызвать метод обновления статуса группы
            // с учетом нового источника:
            RaiseParamStatusChanged();
        }

        /// <summary>
        /// Метод сброса привязки Source: отписывается от событий и обнуляет ссылку.
        /// </summary>
        public void UnbindSource()
        {
            if (_parentSource != null)
            {
                tikTakReq -= _parentSource.EventRequest;
                _parentSource = null;
                RaiseParamStatusChanged();
            }
        }
        public string sourceTitle => ParentSource?.title ?? "";

        // Приватная коллекция тегов с объектом-замком
        private readonly object _tagsLock = new object();
        private List<Tag> _tags = new List<Tag>();
        // Публичное свойство, возвращающее копию списка для потокобезопасного доступа
        public List<Tag> Tags
        {
            get
            {
                lock (_tagsLock)
                {
                    return _tags.ToList();
                }
            }
        }

        public int TagsCountGood => Tags.Count(tag => tag.Good);
        public void UseTags(List<Tag> tags)
        {
            lock (_tagsLock)
            {
                _tags.Clear();
                if (tags != null)
                {
                    foreach (var tag in tags)
                    {
                        tag.ParentGroup = this;
                        _tags.Add(tag);
                    }
                }
            }
        }
        // Метод для добавления тега
        public void AddTag(Tag tag)
        {
            if (tag != null)
            {
                lock (_tagsLock)
                {
                    if (!_tags.Contains(tag))
                    {
                        tag.ParentGroup = this;
                        _tags.Add(tag);
                    }
                }
            }
        }

        // Метод для удаления тега
        public void RemoveTag(Tag tag)
        {
            if (tag != null)
            {
                lock (_tagsLock)
                {
                    if (_tags.Contains(tag))
                    {
                        tag.ParentGroup = null;
                        _tags.Remove(tag);
                    }
                }
            }
        }


        #endregion

        // ==============================================================================

        #region Static

        // Статическая коллекция с объектом-замком
        private static readonly object _itemsLock = new object();
        public static List<Group> items = new List<Group>();

        public static ushort lastId = 0;
        public static bool log = false;

        public static bool Exist(Group group) => items.Any(x => x.Equals(group));

        // Пример обновления статической коллекции
        public static void Clear()
        {
            lock (_itemsLock)
            {
                lastId = 0;
                items = new List<Group>();
            }
        }

        public static Group Item(ushort id)
        {
            lock (_itemsLock)
            {
                return items.FirstOrDefault(x => x.Id == id);
            }
        }
        public static Group Item(string title)
        {
            lock (_itemsLock)
            {
                return items.FirstOrDefault(x => x.title == title);
            }
        }

        public static void LinkGroups()
        {
            foreach (var group in items)
            {
                var useTags = Tag.items.Where(x => x.groupId == group.Id).ToList();
                group.UseTags(useTags);
            }
        }

        public static void ActivateItems()
        {
            foreach (var group in items)
            {
                if (group.Off)
                    group.Start();
            }
        }

        #endregion

        // ==============================================================================

        // Безопасный вызов для HandlerParam (принимает GroupParamStatus)
        private void SafeInvokeHandlerParam(HandlerParam handler, GroupParamStatus info)
        {
            if (handler == null)
                return;
            foreach (HandlerParam subscriber in handler.GetInvocationList())
            {
                try
                {
                    subscriber(info);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.HResult, $"Ошибка в обработчике HandlerParam: {ex.Message}", eMessageCategory.Source);
                }
            }
        }

        // Безопасный вызов для HandlerReq (принимает IGroupOff)
        private void SafeInvokeHandlerReq(HandlerReq handler, IGroupOff group)
        {
            if (handler == null)
                return;
            foreach (HandlerReq subscriber in handler.GetInvocationList())
            {
                try
                {
                    subscriber(group);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.HResult, $"Ошибка в обработчике HandlerReq: {ex.Message}", eMessageCategory.Source);
                }
            }
        }

        // Безопасный вызов для HandlerGroupStatus (принимает ushort Id и eGroupStatus)
        private void SafeInvokeHandlerGroupStatus(HandlerGroupStatus handler, ushort id, eGroupStatus status)
        {
            if (handler == null)
                return;
            foreach (HandlerGroupStatus subscriber in handler.GetInvocationList())
            {
                try
                {
                    subscriber(id, status);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.HResult, $"Ошибка в обработчике HandlerGroupStatus: {ex.Message}", eMessageCategory.Source);
                }
            }
        }


    }
}
