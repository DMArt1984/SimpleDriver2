using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DML.Log;
using LogCodeMessage;

namespace Connector
{
    public interface IGroupOff
    {
        ushort Id { get; }
        bool Off { get; set; }
    }

    public struct GroupParamStatus
    {
        public readonly ushort Id;
        public uint UpdateRate;
        public bool Off;
        public bool IsStopped;

        public GroupParamStatus(ushort id, uint updateRate, bool off, bool isStopped)
        {
            Id = id;
            UpdateRate = updateRate;
            Off = off;
            IsStopped = isStopped;
        }
    }

    public class Group : BaseLogger, IGroupOff, IDisposable
    {
        public ushort Id { get; }             // ID группы
        public string title { get; }          // Название группы
        public string description { get; }    // Описание группы

        // Родительский источник; теперь реализовано как полноценное свойство с get/set
        private Source _parentSource;
        public Source ParentSource
        {
            get => _parentSource;
            set
            {
                // Если ранее был назначен родитель, отписываем обработчик
                if (_parentSource != null)
                {
                    tikTakReq -= _parentSource.EventRequest;
                }
                _parentSource = value;
                // Если новый родитель назначен, подписываем его обработчик
                if (_parentSource != null)
                {
                    tikTakReq -= _parentSource.EventRequest; // чтобы избежать дублирования
                    tikTakReq += _parentSource.EventRequest;
                }
            }
        }

        // Единая коллекция тегов; если Tag реализует ICodeMessage, ее можно использовать для обновления статусов
        public List<Tag> Tags { get; } = new List<Tag>();

        public int TagsCountGood => Tags.Count(tag => tag.Good);

        // События для оповещения об изменениях параметров и статистике
        public delegate void HandlerParam(GroupParamStatus info);
        public event HandlerParam eventParams;

        public delegate void HandlerReq(IGroupOff group);
        public event HandlerReq tikTakReq;

        public delegate void HandlerInfo(ushort id, int tickCount, int all, int good);
        public event HandlerInfo tikTakInfo;

        // Таймер и связанные поля
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
                    // Если значение не положительное, используем 100 мс по умолчанию
                    _updateRate = (value <= 0 ? 100u : value);
                    RaiseParamStatusChanged();
                    if (_timer != null)
                    {
                        _timer.Change(0, (int)_updateRate);
                    }
                }
            }
        }

        // Флаг, управляющий состоянием группы (включена/выключена)
        private bool _off = true;
        public bool Off
        {
            get => _off;
            set
            {
                if (_off != value)
                {
                    _off = value;
                    if (!_off) // Включение группы – запускаем таймер
                    {
                        StartTimer();
                    }
                    else // Выключение – останавливаем таймер
                    {
                        StopTimer();
                    }
                    RaiseParamStatusChanged();
                }
            }
        }

        private bool _disable = false;
        public string sourceTitle = ""; // Название источника (если нужно)

        // Конструктор группы
        public Group(ushort id, string title, Source parentSource, uint updateRate = 100, bool disable = false, string description = "")
            : base(LogTarget.FileConsoleForm, null)
        {
            Id = id;
            this.title = title;
            this.description = description;
            UpdateRate = updateRate;
            _disable = disable;
            ParentSource = parentSource;
            logger.Info($"new group ID {Id} {title} {updateRate}", eMessageCategory.Source);
        }

        // Добавление тега в группу
        public void AddTag(Tag tag)
        {
            if (tag != null && !Tags.Contains(tag))
            {
                Tags.Add(tag);
            }
        }
        public void RemoveTag(Tag tag)
        {
            if (tag != null && Tags.Contains(tag))
            {
                Tags.Remove(tag);
            }
        }

        // Метод активации группы (например, при старте источника)
        public void Activate()
        {
            Off = _disable;
        }

        // Метод для задания тегов группы (перезаписывает существующий список)
        public void UseTags(List<Tag> tags)
        {
            Tags.Clear();
            if (tags != null)
            {
                Tags.AddRange(tags);
            }
        }

        // Освобождение ресурсов: останавливаем таймер
        public void Dispose()
        {
            _timer?.Dispose();
        }

        // Таймер: запуск
        private void StartTimer()
        {
            if (_timer == null)
            {
                _timerStop = false;
                _timer = new Timer(TimerCallback, null, 0, (int)UpdateRate);
                // При запуске таймера обновляем статусы тегов
                SendOn();
            }
        }

        // Таймер: остановка
        private void StopTimer()
        {
            if (_timer != null)
            {
                _timerStop = true;
            }
            SendOff();
        }

        // Свойство, возвращающее true, если таймер не запущен
        private bool IsTimerStopped => _timer == null;

        // Таймер-колбэк: каждое "тиканье"
        private void TimerCallback(object state)
        {
            _tickCount++;
            tikTakReq?.Invoke(this);
            Statistic();

            // Если остановка или группа выключена – завершаем таймер
            if (_timerStop || Id == 0 || Off)
            {
                _timer?.Dispose();
                _timer = null;
                _timerStop = false;
                _tickCount = 0;
                SendStatusOff();
            }
        }

        // Метод обновления статистики
        public void Statistic()
        {
            int all = Tags.Count;
            int good = Tags.Count(x => x.Good);
            tikTakInfo?.Invoke(Id, _tickCount, all, good);
        }

        // Обновление статусов тегов при выключении
        public void SendStatusOff()
        {
            SendOff();
            RaiseParamStatusChanged();
        }

        // Установка статуса "включено" для тегов группы
        public void SendOn()
        {
            // Приводим Tags к ICodeMessage
            Tag.CodeMessageList(Tags.Cast<ICodeMessage>().ToList(), CodeMessageFactory.FromEnumX(eTagCode.groupOn));
        }

        // Установка статуса "выключено" для тегов группы
        public void SendOff()
        {
            Tag.CodeMessageList(Tags.Cast<ICodeMessage>().ToList(), CodeMessageFactory.FromEnumX(eTagCode.groupOff));
        }

        // Метод для уведомления об изменении параметров группы
        private void RaiseParamStatusChanged()
        {
            eventParams?.Invoke(new GroupParamStatus(Id, _updateRate, _off, IsTimerStopped));
        }

        public void Refresh()
        {
            RaiseParamStatusChanged();
        }

        // Переопределяем Equals и GetHashCode для корректного сравнения групп
        public override bool Equals(object obj)
        {
            if (obj is Group other)
            {
                return this.Id == other.Id || this.title == other.title;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ (title?.GetHashCode() ?? 0);
        }

        // Статические члены для глобального управления группами
        static public List<Group> items = new List<Group>(); // Все группы
        static public ushort lastId = 0;
        static public bool log = false;

        static public bool Exist(Group group) => items.Any(x => x.Equals(group));

        static public void Clear()
        {
            lastId = 0;
            items = new List<Group>();
        }

        static public Group Item(ushort id) => items.FirstOrDefault(x => x.Id == id);
        static public Group Item(string title) => items.FirstOrDefault(x => x.title == title);

        // Привязка тегов к группам: обновление списка тегов для каждой группы
        static public void LinkGroups()
        {
            foreach (var group in items)
            {
                var useTags = Tag.items.Where(x => x.groupId == group.Id).ToList();
                group.UseTags(useTags);
            }
        }

        static public void ActivateItems()
        {
            foreach (var group in items)
            {
                if (group.Off)
                    group.Activate();
            }
        }
    }
}
