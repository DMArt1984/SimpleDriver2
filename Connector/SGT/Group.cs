using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DML;
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

    public class Group : BaseLogger, IGroupOff
    {
        public ushort Id { get; } // ID группы
        public string title { get; } // Название группы
        public string description { get; } // Описание группы

        // Теги для группы
        private List<ICodeMessage> tags = new List<ICodeMessage>();
        private int _tagsCount = 0;
        public int TagsCount => _tagsCount;
        public int TagsCountGood => tags.Count(x => x.Good);

        // События
        public delegate void HandlerParam(GroupParamStatus info);
        public event HandlerParam eventParams;

        public delegate void HandlerReq(IGroupOff group);
        public event HandlerReq tikTakReq;

        public delegate void HandlerInfo(ushort id, int counter, int all, int good);
        public event HandlerInfo tikTakInfo;

        // Поля для таймера (объединяет функционал GroupManager)
        private System.Threading.Timer _timer;
        private bool _timerStop = false;
        private int _counter = 0;

        // Параметры обновления
        private uint _updateRate = 100;
        public uint UpdateRate
        {
            get => _updateRate;
            set
            {
                if (_updateRate != value)
                {
                    _updateRate = value <= 0 ? 100u : value;
                    EventChangeParamStatus();
                    // Если таймер уже запущен, можно пересоздать его с новым интервалом
                    if (_timer != null)
                    {
                        _timer.Change(0, (int)_updateRate);
                    }
                }
            }
        }

        // Свойство для включения/выключения группы
        private bool _off = true;
        public bool Off
        {
            get => _off;
            set
            {
                if (_off != value)
                {
                    _off = value;
                    if (!_off) // включаем группу
                    {
                        StartTimer();
                    }
                    else // выключаем группу
                    {
                        StopTimer();
                    }
                    EventChangeParamStatus();
                }
            }
        }

        private bool _disable = false;
        public string sourceTitle = ""; // Название источника (если необходимо)

        public Group(ushort id, string title, uint updateRate = 100, bool disable = false, string description = "")
            : base(LogTarget.FileConsoleForm, null)
        {
            Id = id;
            this.title = title;
            this.description = description;
            UpdateRate = updateRate;
            _disable = disable;
            logger.Info($"new group ID {Id} {title} {updateRate}", eMessageCategory.Source);
        }

        public void Activate()
        {
            Off = _disable;
        }

        // Назначение тегов группе
        public void UseTags(List<ICodeMessage> tags)
        {
            if (tags == null)
                tags = new List<ICodeMessage>();
            this.tags = tags;
            _tagsCount = this.tags.Count();
        }

        ~Group()
        {
            StopTimer();
        }

        private void EventChangeParamStatus()
        {
            eventParams?.Invoke(new GroupParamStatus(Id, _updateRate, _off, IsTimerStopped));
        }

        public void Refresh()
        {
            EventChangeParamStatus();
        }

        // Методы управления таймером

        private void StartTimer()
        {
            if (_timer == null)
            {
                _timerStop = false;
                _timer = new System.Threading.Timer(TimerCallback, null, 0, (int)UpdateRate);
                // При включении обновляем статусы тегов
                SendOn();
            }
        }

        private void StopTimer()
        {
            if (_timer != null)
            {
                _timerStop = true;
            }
            // При отключении отправляем статус "выключено"
            SendOff();
        }

        // Возвращает true, если таймер не запущен
        private bool IsTimerStopped => _timer == null;

        // Таймер-колбэк, заменяющий функциональность TimerCB из GroupManager
        private void TimerCallback(object state)
        {
            _counter++;
            tikTakReq?.Invoke(this);
            Statistic();

            if (_timerStop || Id == 0 || Off)
            {
                _timer?.Dispose();
                _timer = null;
                _timerStop = false;
                _counter = 0;
                SendStatusOff();
            }
        }

        // Метод статистики – можно вызвать для обновления информации о группе
        public void Statistic()
        {
            int all = tags.Count();
            int good = tags.Count(x => x.Good);
            tikTakInfo?.Invoke(Id, _counter, all, good);
        }

        public void SendStatusOff()
        {
            SendOff();
            EventChangeParamStatus();
        }

        public void SendOn()
        {
            // Обновляем статусы тегов при включении группы
            Tag.CodeMessageList(tags, CodeMessageFactory.FromEnumX(eTagCode.groupOn));
        }

        public void SendOff()
        {
            // Обновляем статусы тегов при выключении группы
            Tag.CodeMessageList(tags, CodeMessageFactory.FromEnumX(eTagCode.groupOff));
        }

        // ==========================================================================================================================

        #region Statuc

        // Статические методы для работы со списком групп (оставляем без изменений или оптимизируем отдельно)
        static public List<Group> items = new List<Group>(); // Все группы
        static public ushort lastId = 0;
        static public bool log = false;

        static public bool Exist(Group group) => items.Count(x => x.Equals(group)) > 0;
        public bool Equals(Group group)
        {
            return this.Id == group.Id || this.title == group.title;
        }

        static public void Clear()
        {
            lastId = 0;
            items = new List<Group>();
        }

        static public Group Item(ushort id) => items.FirstOrDefault(x => x.Id == id);
        static public Group Item(string title) => items.FirstOrDefault(x => x.title == title);

       

        // Привязка тегов к группам
        static public void LinkGroups()
        {
            foreach (var group in items)
            {
                var useTags = Tag.items.Where(x => x.groupId == group.Id).ToList();
                group.UseTags(useTags.Select(x => x as ICodeMessage).ToList());
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

        #endregion

    }
}
