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
        public ushort Id { get; }
        public string title { get; }
        public string description { get; }

        private Source _parentSource;
        public Source ParentSource
        {
            get => _parentSource;
            set
            {
                if (_parentSource != null)
                {
                    tikTakReq -= _parentSource.EventRequest;
                }
                _parentSource = value;
                if (_parentSource != null)
                {
                    tikTakReq -= _parentSource.EventRequest;
                    tikTakReq += _parentSource.EventRequest;
                }
            }
        }

        public List<Tag> Tags { get; } = new List<Tag>();

        public int TagsCountGood => Tags.Count(tag => tag.Good);

        public delegate void HandlerParam(GroupParamStatus info);
        public event HandlerParam eventParams;

        public delegate void HandlerReq(IGroupOff group);
        public event HandlerReq tikTakReq;

        public delegate void HandlerInfo(ushort id, int tickCount, int all, int good);
        public event HandlerInfo tikTakInfo;

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
                    RaiseParamStatusChanged();
                }
            }
        }

        private bool _disable = false;
        public string sourceTitle = "";

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

        public void Activate()
        {
            Off = _disable;
        }

        public void UseTags(List<Tag> tags)
        {
            Tags.Clear();
            if (tags != null)
            {
                Tags.AddRange(tags);
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void StartTimer()
        {
            if (_timer == null)
            {
                _timerStop = false;
                _timer = new Timer(TimerCallback, null, 0, (int)UpdateRate);
                SendOn();
            }
        }

        private void StopTimer()
        {
            if (_timer != null)
            {
                _timerStop = true;
            }
            SendOff();
        }

        private bool IsTimerStopped => _timer == null;

        private void TimerCallback(object state)
        {
            _tickCount++;
            tikTakReq?.Invoke(this);
            Statistic();

            if (_timerStop || Id == 0 || Off)
            {
                _timer?.Dispose();
                _timer = null;
                _timerStop = false;
                _tickCount = 0;
                SendStatusOff();
            }
        }

        public void Statistic()
        {
            int all = Tags.Count;
            int good = Tags.Count(x => x.Good);
            tikTakInfo?.Invoke(Id, _tickCount, all, good);
        }

        public void SendStatusOff()
        {
            SendOff();
            RaiseParamStatusChanged();
        }

        public void SendOn()
        {
            Tag.CodeMessageList(Tags.Cast<ICodeMessage>().ToList(), CodeMessageFactory.FromEnumX(eTagCode.groupOn));
        }

        public void SendOff()
        {
            Tag.CodeMessageList(Tags.Cast<ICodeMessage>().ToList(), CodeMessageFactory.FromEnumX(eTagCode.groupOff));
        }

        private void RaiseParamStatusChanged()
        {
            eventParams?.Invoke(new GroupParamStatus(Id, _updateRate, _off, IsTimerStopped));
        }

        public void Refresh()
        {
            RaiseParamStatusChanged();
        }

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

        public static List<Group> items = new List<Group>();
        public static ushort lastId = 0;
        public static bool log = false;

        public static bool Exist(Group group) => items.Any(x => x.Equals(group));

        public static void Clear()
        {
            lastId = 0;
            items = new List<Group>();
        }

        public static Group Item(ushort id) => items.FirstOrDefault(x => x.Id == id);
        public static Group Item(string title) => items.FirstOrDefault(x => x.title == title);

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
                    group.Activate();
            }
        }
    }
}
