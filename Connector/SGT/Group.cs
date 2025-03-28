using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DML;
using DML.Log;

namespace WinSimpleIDriver.Connector.SGT
{
    public interface IGroupOff
    {
        ushort Id { get; }
        bool Off { get; set; }

    }
    
    public struct GroupParamStatus
    {
        public readonly ushort Id;
        public uint updateRate;
        public bool off;
        public bool isStop;
        public GroupParamStatus(ushort Id, uint updateRate, bool off, bool isStop)
        {
            this.Id = Id;
            this.updateRate = updateRate;
            this.off = off;
            this.isStop = isStop;
        }
    }

    public struct cellGroup
    {
        public ushort Id;
        public DataGridViewRow row;
        public DataGridViewCell updateRate;
        public DataGridViewCell on;
        public DataGridViewCell description;
        public DataGridViewCell step;
        public DataGridViewCell statistic;
    }

    public class GroupManager
    {
        public readonly ushort sourceId;

        public delegate void HandlerReq(IGroupOff group);
        public event HandlerReq tikTakReq;

        public readonly Group _parent;

        System.Threading.Timer timer;
        bool timerStop = false;

        public int counter = 0;

        public GroupManager(ushort Id, Group parent)
        {
            this.sourceId = Id;
            this._parent = parent;
        }
        public void Go(Group group)
        {
            tikTakReq?.Invoke(group);
        }

        public void On()
        {
            if (timer == null)
            {
                TimerCallback tm = new TimerCallback(TimerCB);
                timer = new System.Threading.Timer(tm, new { a = 0 }, 0, _parent.UpdateRate);

                // статусы тегов
                _parent.SendOn();
            }
        }

        public void Off()
        {
            if (timer != null)
                StopTimer();

            // статусы тегов
            _parent.SendOff();
        }

        public void StopTimer()
        {
            timerStop = true;
        }

        public bool IsStop => (timer == null);

        void TimerCB(object obj)
        {
            counter++;

            // v1
            tikTakReq?.Invoke(_parent);

            _parent.Statistic();

            if (timerStop || _parent.Id == 0 || _parent.Off)
            {
                if (timer != null)
                {
                    timer.Dispose();
                    timer = null;
                }
                timerStop = false;
                counter = 0;

                _parent.SendStatusOff();
            }
            //Task.Delay(10); // 
        }

    }

    public class Group : BaseLogger, IGroupOff
    {
        public ushort Id { get; } // ID группы
        public string title { get; } // Название тега
        public string description { get; } // Описание тега


        // теги для группы
        List<ICodeMessage> tags = new List<ICodeMessage>();

        public int TagsCount => _tagsCount;
        int _tagsCount = 0;
        public int TagsCountGood => tags.Count(x => x.Good);

        // События
        public delegate void HandlerParam(GroupParamStatus info);
        public event HandlerParam eventParams;

        public delegate void HandlerReq(IGroupOff group);
        public event HandlerReq tikTakReq;

        public GroupManager manager; // = new List<GroupManager>();

        public delegate void HandlerInfo(ushort Id, int counter, int all, int good);
        public event HandlerInfo tikTakInfo;

        // Одинаковы ?
        public bool Equals(Group group)
        {
            return this.Id == group.Id || this.title == group.title;
        }

        public bool AddManager(GroupManager gm)
        {
            this.manager = gm;
            return true;
        }
        public bool AddManager(ushort sourceId, out GroupManager gm)
        {
            gm = new GroupManager(this.Id, this);
            this.manager = gm;
            return true;
        }

        public bool RemoveManagers(GroupManager.HandlerReq myMethodName)
        {
            this.manager.tikTakReq -= myMethodName;
            return true;
        }

        public bool IsStop => manager.IsStop; // timer == null;

        public uint UpdateRate
        {
            get => _updateRate;
            set
            {
                if (_updateRate != value)
                {
                    if (value <= 0)
                    {
                        value = 100;
                    }
                    _updateRate = value;

                    EventChangeParamStatus();
                }
            }
        }
        uint _updateRate = 100;

        public bool Off
        {
            get => _off;
            set
            {
                if (_off != value)
                {
                    _off = value;
                    if (_off == false) // включить
                    {
                        manager.On();
                    }
                    else // отключить
                    {
                        manager.Off();
                    } 

                    EventChangeParamStatus();
                }
            }
        }
        bool _off = true;

        bool _disable = false;

        public string sourceTitle = ""; // ...

        //int counter = 0;

        public Group(ushort Id, string title, uint updateRate = 100, bool disable = false, string description = "") : base(LogTarget.FileConsoleForm, null)
        {
            this.Id = Id;
            this.title = title;
            this.description = description;
            UpdateRate = updateRate;
            _disable = disable;

            logger.Info($"new group ID{Id} {title} {updateRate}", eMessageCategory.Source);
        }

        public void Activate()
        {
            Off = _disable;
        }

        // Добавить теги
        public void UseTags(List<ICodeMessage> tags)
        {
            if (tags == null)
                tags = new List<ICodeMessage>();
            this.tags = tags;
            _tagsCount = this.tags.Count();
        }

        ~Group()
        {
            manager.StopTimer();
        }

        void EventChangeParamStatus()
        {
            eventParams?.Invoke(new GroupParamStatus(Id, _updateRate, _off, IsStop));
        }

        public void Refresh()
        {
            EventChangeParamStatus();
        }

        // -------------------------------------

        public void Statistic()
        {
            int all = tags.Count();
            int good = tags.Count(x => x.Good == true);
            int counter = manager.counter;
            tikTakInfo?.Invoke(Id, counter, all, good);
        }

        public void SendStatusOff()
        {
            SendOff();

            EventChangeParamStatus();
        }

        public void SendOn()
        {
            // статусы тегов
            Tag.CodeMessageList(tags, CodeMessageFactory.FromEnum(eTagCode.groupOn));
        }

        public void SendOff()
        {
            // статусы тегов
            Tag.CodeMessageList(tags, CodeMessageFactory.FromEnum(eTagCode.groupOff));
        }

        // ===============================================================================

        static public List<Group> items = new List<Group>(); // все группы
        static public ushort lastId = 0;
        static public bool log = false;

        static public bool Exist(Group group) => items.Count(x => x.Equals(group)) > 0; // есть ли такая группа уже в списке

        static public void Clear()
        {
            Group.lastId = 0;
            Group.items = new List<Group>();
        }
        static public Group Item(ushort Id) => items.FirstOrDefault(x => x.Id == Id);
        static public Group Item(string title) => items.FirstOrDefault(x => x.title == title);

        static public bool InProject(dynamic output) => JsonControl.IsProp(output, "Groups");

        // Привязки групп
        static public void LinkGroups()
        {
            foreach (var item in Group.items)
            {
                var useTags = Tag.items.Where(x => x.groupId == item.Id).ToList();
                item.UseTags(useTags.Select( x => x as ICodeMessage).ToList());
            }
        }

        static public void ActivateItems()
        {
            foreach (var item in Group.items)
            {
                item.Activate();
            }
        }


        // Получение параметров группы
        static public void ParseItemGroup(dynamic item, uint forindex, out string title, out uint updateRate, out bool off, out string description, out string sourceTitle, out dynamic tags)
        {
            title = JsonControl.GetString(item, "Title", $"Group #{forindex}");
            updateRate = (uint)JsonControl.GetInt(item, "UpdateRate");
            off = JsonControl.GetBool(item, "Off");
            description = JsonControl.GetString(item, "Desc");
            sourceTitle = JsonControl.GetString(item, "Source");
            tags = JsonControl.IsProp(item, "Tags") ? item.Tags : null;
        }
    }

    public class GroupEditor // Редактирование
    {
        public uint Id; // Уникальный идентификатор (0 - нет Id)
        public string title; // Название
        public bool off; // Отключение
        public uint updateRate; // Период опроса (мсек)
        public string description; // Описание
        public string sourceTitle; // Название источника
    }
}
