using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using DML.Log;
using System.Windows.Forms;
using WinSimpleIDriver.Connector.Driver;
using System.Threading;
using System.Collections.Concurrent;
using DML;

namespace WinSimpleIDriver.Connector.SGT
{
    public enum eSourceStatus
    {
        created = 99,
        noClient = 1, // нет клиента
        closing = 2, // закрытие...
        closed = 3, // закрыт
        opening = 4, // открытие...
        openedNoCycle = 5, // открыт, но нет опроса
        cycle = 0, // циклический опрос
        breaking = 6, // закрытие по ошибке
        wait = 7, // ожидание перезапуска...
        errOpen = -56, // ошибка открытия
        errClose = -57, // ошибка закрытия
    }

    static class eSourceStatusText
    {
        public static String GetText(this eSourceStatus status)
        {
            switch (status)
            {
                case eSourceStatus.cycle:
                    return "Работает";
                case eSourceStatus.breaking:
                    return "Закрытие по ошибке...";
                case eSourceStatus.closed:
                    return "Закрыто";
                case eSourceStatus.closing:
                    return "Закрытие...";
                case eSourceStatus.noClient:
                    return "Ошибка создания клиента";
                case eSourceStatus.openedNoCycle:
                    return "Открыто, но нет опроса";
                case eSourceStatus.opening:
                    return "Открытие...";
                case eSourceStatus.wait:
                    return "Ожидание...";
                case eSourceStatus.errOpen:
                    return "Ошибка открытия";
                case eSourceStatus.errClose:
                    return "Ошибка закрытия";
                default:
                    return status.ToString();
            }
        }
    }

    interface ISource
    {
        eSourceStatus Status { get; set; }

        void ClearError();
        CodeMessage ActiveError { get; set; }

        byte MaxBreak { get; set; }
        byte counterBreak { get; set; }

    }

    public struct SourceParam
    {
        public readonly ushort Id;
        public string address;
        public bool autoRequestAftereOpen;
        public bool autoOpenAfterFail;

        public SourceParam(ushort Id, string address, bool autoRequestAftereOpen, bool autoOpenAfterFail)
        {
            this.Id = Id;
            this.address = address;
            this.autoRequestAftereOpen = autoRequestAftereOpen;
            this.autoOpenAfterFail = autoOpenAfterFail;
        }
    }

    public struct cellSource
    {
        //ED public ushort Id;
        public DataGridViewRow row;
        public DataGridViewCell connection;
        public DataGridViewCell opened;
        public DataGridViewCell code;
        public DataGridViewCell message;
        public DataGridViewCell comment;
        public DataGridViewCell step;
        public DataGridViewCell statistic;
    }

    public class Source : ISource
    {
        public ushort Id { get; } // ID источника данных
        public string title { get; } // Название источника
        public string description { get; } // Описание источника

        object locker = new object();

        // теги для источника
        List<Tag> tags = new List<Tag>();
        Dictionary<ushort, List<Tag>> dicTagGroup = new Dictionary<ushort, List<Tag>>();
        public int TagsCount => _tagsCount;
        int _tagsCount = 0;
        public int TagsCountGood => tags.Count(x => x.Good);

        // Справка
        public Dictionary<string, string> helpSource => HelpDicSource(driverType);
        public Dictionary<string, string> helpTag => HelpDicTag(driverType);
        static public Dictionary<string, string> GetHelpSource(eDriverType type)
        {
            return HelpDicSource(type);
        }

        // события
        #region Event
        public delegate void HandlerError(ushort Id, CodeMessage activeError);
        public event HandlerError eventError;

        public delegate void HandlerStatus(ushort Id, eSourceStatus status);
        public event HandlerStatus eventStatus;

        public delegate void HandlerInfo(SourceParam info);
        public event HandlerInfo eventParams;

        public delegate void HandlerReq(ushort sourceId, ushort groupId, List<ITagResult> results, int counter, int fails, int all, int good);
        public event HandlerReq eventReq;

        public delegate void HandlerTrafficLog(ushort Id, string message);
        public event HandlerTrafficLog eventTraffic;
        #endregion

        // Устройство
        private eDriverType driverType;
        public eDriverType xdriverType => driverType;

        IRealDevice device;

        // строка подключения
        public string Address {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    EventChangeParam();
                }
            }
        } 
        string _address = "";

        public bool Process => _process;
        bool _process = false; // выполняется запрос...

        // автоматический запуск циклического опроса после открытия
        public bool AutoRequestAftereOpen
        {
            get => _autoRequestAftereOpen;
            set
            {
                if (_autoRequestAftereOpen != value)
                {
                    _autoRequestAftereOpen = value;
                    EventChangeParam();
                }
            }
        }
        bool _autoRequestAftereOpen;

        // автоматическое переоткрытие после ошибки
        public bool AutoOpenAfterFail
        {
            get => _autoOpenAfterFail;
            set
            {
                if (_autoOpenAfterFail != value)
                {
                    _autoOpenAfterFail = value;
                    EventChangeParam();
                }
            }
        }
        bool _autoOpenAfterFail;

        public bool Fail => _fail; 
        bool _fail = false; // была ошибка с последующим закрытием

        // Переподключения устройства
        private System.Timers.Timer timerReopen;
        private int stepReOpen = 0;
        private int[] rTimeMsec = new[] { 5000, 6000, 7000, 8000, 9000, 10000};

        int counterReq = 0;
        int counterFailReq = 0;

        // Конструкторы
        public Source(ushort Id, string title, eDriverType driverType, bool disable, bool auto, bool reopen, string address = "", string description = "")
        {
            Console.WriteLine($"Source ID={Id} {title}");

            ChangeDriver(driverType, address);

            Console.WriteLine($" step2: this...");
            this.Id = Id;
            this.title = title;
            this.description = description;
            this.AutoRequestAftereOpen = auto;
            this.AutoOpenAfterFail = reopen;
            this.Address = address;

            SetClient(address);

            LogHelper.LogApp($"new source ID{Id} {title} {driverType} {Address}");
        }

        ~Source()
        {
            (device as Device).eventTraffic -= EventTraffic;
        }

        // Переназначить драйвер
        private void ChangeDriver(eDriverType driverType, string address)
        {
            if (device != null)
                (device as Device).eventTraffic -= EventTraffic;

            this.driverType = driverType;
            string paramClient = address;

            switch (this.driverType)
            {
                case eDriverType.Formula:
                    device = new Formula();
                    break;
                case eDriverType.Application:
                    device = new AppDevice();
                    break;
                case eDriverType.ModbusTCPclient:
                    device = new ModbusTCPClient();
                    break;
                case eDriverType.ModbusRTUclient:
                    device = new ModbusRTUClient();
                    break;
                case eDriverType.AppUDP:
                    device = new AppUDP(address);
                    break;
                case eDriverType.MSSQLclient:
                    device = new MSSQLclient();
                    break;
                case eDriverType.OPCUAclient:
                    device = new HylasoftOPCUA();
                    break;
                default:
                    this.device = new Device();
                    break;
            }

            Console.WriteLine($" step1: += EventTraffic");
            (device as Device).eventTraffic += EventTraffic;

        }

        private void SetClient(string address)
        {
            Console.WriteLine($" step3: CreateClient(paramClient)");
            CodeMessage result = this.device.CreateClient(address);
            if (result.code != 0)
                ActiveError = result;
            Status = (result.code == 0) ? eSourceStatus.closed : eSourceStatus.noClient;

            Console.WriteLine($" step4: _disable = ");
            _disable = _disable || result.code != 0; // new!!!

            //LoggerConsole.Log($"Source ID={Id} created!", log);
        }

        public void Activate()
        {
            Off = _disable;
        }

        // Параметры для Source из адреса
        private Dictionary<string, string> ParamsForSource(string address)
        {
            try
            {
                var dic = Device.ParamsToDic(address);
                if (dic.ContainsKey("fails"))
                {
                    MaxBreak = byte.Parse(dic["fails"]);
                    if (MaxBreak < 1)
                        MaxBreak = 1;
                    dic.Remove("fails");
                }
                return dic;
            } catch (Exception ex)
            {
                LogHelper.LogException(ex, $"ParamsForSource = {address}");
                return new Dictionary<string, string>();
            }
        }

        // Использовать теги
        public void UseTags(List<Tag> tags)
        {
            if (tags == null)
                tags = new List<Tag>();
            this.tags = tags;
            _tagsCount = this.tags.Count();
        }

        // Использовать группы
        public void UseGroups(List<Group> groups)
        {
            if (groups == null)
                return;

            foreach (var item in groups)
            {
                AppendGroup(item);
            }
        }

        // Добавить группу
        public void AppendGroup (Group group)
        {
            // v1
            //group.tikTakReq += EventRequest;

            // v2
            //var gm = new GroupManager(this.Id, group);
            //group.managers.Add(gm);

            //if (group.AddManager(gm))
            if (group.AddManager(this.Id, out GroupManager gm))
            {
                gm.tikTakReq += EventRequest;
                //
                roll.TryAdd(group.Id, false);
            }

        }

        // Убрать группу
        public void RemoveGroup(Group group)
        {
            // v1
            //group.tikTakReq -= EventRequest;

            // v2
            //...

            // отключить события
            group.RemoveManagers(EventRequest);

            // убравть из опроса
            roll.TryRemove(group.Id, out bool retval);
        }
        
        // Одиночный запрос
        public void OneRequest()
        {
            EventRequest(new Group(0, ""));
        }

        // =======================================================================

        public void SetLogTraffic(bool enable)
        {
            (device as ITrafficLog).enableLog = enable;
        }

        public bool IsLogTraffic()
        {
            return (device as ITrafficLog).enableLog;
        }

        public bool IsSupportLog()
        {
            return (device as ITrafficLog).supportLog;
        }

        // ========================================================================

        bool UserUseClosed = false;

        public bool Off
        {
            get => _off;
            set
            {
                _off = value;
                if (_off == false) // включить
                {
                    UserUseClosed = false;
                    Open(true);
                }
                else // отключить
                {
                    //AutoOpenAfterFail = false;
                    if (Status != eSourceStatus.closed && Status != eSourceStatus.closing)
                    {
                        UserUseClosed = true;
                        //await Task.Run(() => Close(true));
                        Close(true);
                    }
                }
            }
        }
        bool _off = true;

        bool _disable = false;

        public void OnControl(string newAddress)
        {
            LogHelper.LogUser($"Подключить: {this.Id} {this.title} > {newAddress}. Шаг 1");
            if (String.IsNullOrWhiteSpace(newAddress) || newAddress == Address)
            {
                Off = false;
                LogHelper.LogUser($"Подключить: {this.Id} {this.title} > {newAddress}. Шаг 4 - завершено");
                return;
            }

            Address = newAddress; // новый адрес

            LogHelper.LogUser($"Подключить: {this.Id} {this.title} > {newAddress}. Шаг 2 - новые параметры");
            var dic = ParamsForSource(Address);

            LogHelper.LogUser($"Подключить: {this.Id} {this.title} > {newAddress}. Шаг 3 - пересоздание клиента");
            device.CreateClient(Address); // пересоздание клиента
            Off = false; // открыть
            //...
            LogHelper.LogUser($"Подключить: {this.Id} {this.title} > {newAddress}. Шаг 4 - завершено");
        }

        int Open(bool user = false)
        {
            //LogHelper.LogApp($"Источник ID={Id} {title} > Открыть...");
            if (Status == eSourceStatus.closed)
            {
                Status = eSourceStatus.opening;
                CodeMessage result = new CodeMessage(0);

                if (device is INetDevice && (device as DeviceNet).disableHostForOpen)
                {
                    var retval = IsPing();
                    if (retval == false)
                        result = new CodeMessage((int)eTagCode.noPing, eTagCode.noPing.GetText());
                } else
                {
                    result = IsHost();
                }

                // Host
                if (result.code == 0)
                {
                    // Connect
                    LogHelper.LogApp($"Источник ID={Id} {title} > Соединение...");
                    result = this.device.Connect(Address);
                } else
                {
                    LogHelper.LogApp($"Источник ID={Id} {title} > Нет связи с хостом/IP");
                    //result = new CodeMessage(-404, "Нет связи с хостом");
                }

                if (result.code != 0)
                    ActiveError = result;

                if (result.code == 0)
                {
                    LogHelper.LogApp($"Источник ID={Id} {title} > Открыть - успешно!");

                    _opened = true;
                    _fail = false;
                    stepReOpen = 0;
                    ClearCounterBreak(); // сброс неудачных запросов

                    // статусы тегов
                    Tag.CodeMessageList(tags, new CodeMessage((int)eTagCode.sourceOpened, eTagCode.sourceOpened.GetText()));

                    if (AutoRequestAftereOpen) // автоматический запуск опроса
                        CyclicRequest = true;
                } else
                {
                    LogHelper.LogApp($"Источник ID={Id} {title} > Открыть - ошибка {result.code} {result.message}");

                    Status = eSourceStatus.breaking;
                    _fail = true;
                    ClearCounterBreak(); // сброс неудачных запросов

                    // статусы тегов
                    Tag.CodeMessageList(tags, new CodeMessage((int)eTagCode.sourceFail, eTagCode.sourceFail.GetText()));

                    OpenAfterFail();
                }

                EventStatus();
                return result.code;
            }
            return 1;
        }

        int Close(bool user = false)
        {
            LogHelper.LogApp($"Источник ID={Id} {title} > Закрыть...");
            if (Status != eSourceStatus.closed)
            {
                Status = eSourceStatus.closing;

                LogHelper.LogApp($"Источник ID={Id} {title} > Ждем...");
                WaitProcess(); // ждем завершения текущего запроса...

                CodeMessage result = this.device.Disconnect();
                if (result.code != 0)
                    ActiveError = result;

                if (result.code == 0)
                {
                    LogHelper.LogApp($"Источник ID={Id} {title} > Закрыть - успешно!");

                    CyclicRequest = false;
                    //LogHelper.LogApp($"Источник ID={Id} {title} > 1...");
                    _opened = false;
                    //LogHelper.LogApp($"Источник ID={Id} {title} > 2...");
                    counterReq = 0;
                    //LogHelper.LogApp($"Источник ID={Id} {title} > 3...");
                    counterFailReq = 0;
                    LogHelper.LogApp($"Источник ID={Id} {title} > Статусы тегов...");

                    // статусы тегов
                    Tag.CodeMessageList(tags, new CodeMessage((int)eTagCode.sourceClosed, eTagCode.sourceClosed.GetText()));

                    LogHelper.LogApp($"Источник ID={Id} {title} > 5...");

                    if (user == false)
                        OpenAfterFail();
                } else
                {
                    LogHelper.LogApp($"Источник ID={Id} {title} > Закрыть - ошибка {result.code} {result.message}");
                }

                EventStatus();
                LogHelper.LogApp($"Источник ID={Id} {title} > Код {result.code}");
                return result.code;
            } else
            {
                // статусы тегов
                Tag.CodeMessageList(tags, new CodeMessage((int)eTagCode.sourceClosed, eTagCode.sourceClosed.GetText()));
            }
            return 1;
        }

        void OpenAfterFail()
        {
            // нужно ли переоткрытие?
            if (AutoOpenAfterFail && Fail && UserUseClosed == false && timerReopen == null)
            {
                LogHelper.LogApp($"Source ID={Id} Reopen {stepReOpen}-{rTimeMsec[stepReOpen]}...");

                try
                {
                    timerReopen = null; // new
                    timerReopen = new System.Timers.Timer(rTimeMsec[stepReOpen]);
                    timerReopen.Elapsed += new ElapsedEventHandler(TimerCB);
                    timerReopen.AutoReset = false;
                    timerReopen.Enabled = true;

                    stepReOpen++;
                    if (stepReOpen >= rTimeMsec.Length)
                        stepReOpen = 0;

                    LogHelper.LogApp($"Source ID={Id} Reopen {stepReOpen}-{rTimeMsec[stepReOpen]}, STEP={stepReOpen}");
                }
                catch (Exception ex)
                {
                    LogHelper.LogException(ex, "OpenAfterFail()");
                    TimerCB_Inner(); // NEW
                }
            }
        }

        // Таймер переоткытия
        private void TimerCB(object source, ElapsedEventArgs e)
        {
            TimerCB_Inner();
        }

        private void TimerCB_Inner()
        {
            LogHelper.LogApp("REOPEN: TimerCB");
            if (AutoOpenAfterFail == false || UserUseClosed == true)
            {
                LogHelper.LogApp("REOPEN: AutoOpenAfterFail == false...");
                timerReopen?.Stop();
                timerReopen?.Close();
                timerReopen = null;
                return;
            }

            //Console.WriteLine("TimerCB: Off=false");
            timerReopen = null;

            if (Off == false)
            {
                LogHelper.LogApp("REOPEN: Open()...");
                Open(false);
            }
            else
            {
                LogHelper.LogApp("REOPEN: Off = false...");
                Off = false;
            }

        }

        void WaitProcess()
        {
            //LoggerConsole.Log("wait process [", log);
            LogHelper.LogApp("wait process [");
            DateTime dt = DateTime.Now;
            while (_process)
            {
                // ждем выполнение текущего запроса...
                //LoggerConsole.Log("wait process...", log);
                LogHelper.LogApp("wait process...");
                Task.Delay(100);
                TimeSpan ts = DateTime.Now.Subtract(dt);
                if (ts.TotalMilliseconds > 5000)
                    break;
                break;
            }
            //LoggerConsole.Log("wait process ]", log);
            LogHelper.LogApp("wait process ]");
        }

        public bool Opened => _opened;
        protected bool _opened = false;

        public bool CyclicRequest
        {
            get { return _cyclicRequest; }
            set
            {
                if (_cyclicRequest != value)
                {
                    _cyclicRequest = value;
                    if (value == false)
                    {
                        WaitProcess(); // ждем завершения текущего запроса...

                        // статусы тегов
                        Tag.CodeMessageList(tags, new CodeMessage((int)eTagCode.sourceOpened, eTagCode.sourceOpened.GetText()));
                    }
                    EventStatus();
                }
            }
        }
        bool _cyclicRequest = false;

        public eSourceStatus Status { 
            get => _status;
            set {
                if (_status != value)
                {
                    _status = value;
                    eventStatus?.Invoke(Id, value);
                }
            } 
        }
        eSourceStatus _status = eSourceStatus.created;

        public CodeMessage ActiveError
        {
            get => _activeError;
            set
            {
                bool newCode = _activeError.code != value.code;

                _activeError = value;
                if (_activeError.code < 0)
                {
                    if (Fail == false)
                    {
                        _fail = true;
                        //Console.WriteLine($"_fail = true;");
                        if (Off)
                        {
                            // уже закрыто...
                            //Console.WriteLine($" -> OpenAfterFail();");
                            OpenAfterFail();
                        }
                        else
                        {
                            Console.WriteLine($" -> Off = true;");
                            Off = true; // закрываем для перезапуска
                        }
                        //...
                    } else
                    {
                        //Console.WriteLine($" ERROR: wait...");
                    }
                }

                if (newCode)
                {
                    eventError?.Invoke(Id, _activeError);
                }
            }
        }
        CodeMessage _activeError;

        public void ClearError()
        {
            ActiveError = new CodeMessage(0, "");
        }

        public void NewBreak()
        {
            counterBreak++; // считаем неудачные запросы для последующего перезапуска
            //Console.WriteLine($"Break = {counterBreak} / {MaxBreak}");
            LogHelper.LogError($"NEW BREAK = {counterBreak} / {MaxBreak}");

            if (counterBreak >= MaxBreak)
            {
                ClearCounterBreak();
                ActiveError = new CodeMessage((int)eTagCode.breakError, eTagCode.breakError.GetText());
                LogHelper.LogError($"NEW BREAK = ActiveError");
                //...
            }
        }

        public void ClearCounterBreak()
        {
            counterBreak = 0;
        }

        public byte MaxBreak { get; set; } = 10;
        public byte counterBreak { get; set; }

        // ================================================================================================
        // Параметры
        void EventChangeParam()
        {
            eventParams?.Invoke(new SourceParam(Id, Address, AutoRequestAftereOpen, AutoOpenAfterFail));
        }

        public void Refresh()
        {
            EventChangeParam();
            eventStatus?.Invoke(Id, Status);
            eventError?.Invoke(Id, ActiveError);
        }

        // Статус
        void EventStatus()
        {
            if (Opened)
            {
                if (CyclicRequest)
                {
                    Status = eSourceStatus.cycle;
                } else
                {
                    Status = eSourceStatus.openedNoCycle;
                }
            } else
            {
                Status = eSourceStatus.closed;
            }
        }

        ConcurrentDictionary<ushort, bool> roll = new ConcurrentDictionary<ushort, bool>();
        ushort groupNow = 0;

        // Запросы
        void EventRequest(IGroupOff group)
        {
            if (group == null)
                return;

            if (group.Off)
                return;

            if (Opened == false)
                return;

            if (CyclicRequest == false && group.Id > 0)
                return;

            EventRequestRUN(group.Id);
        }

        void EventRequestRUN(ushort groupId)
        {
            if (Monitor.TryEnter(locker, new TimeSpan(0, 0, 10)))
            {
                try
                {
                    _process = true;
                    groupNow = groupId;

                    //LoggerConsole.Log($"Source ID={groupId} for group={groupId}...", log);

                    // Выбор тегов для опроса
                    List<Tag> clientTags = tags.Where(x => (x.groupId == groupId) && x.Off == false).ToList();

                    if (clientTags.Any())
                    {

                        // Запрос к устройству...
                        device.Request(clientTags);
                        counterReq++;

                        // Анализ ответов
                        bool breakError = clientTags.Any(x => x.codeMessage.code == (int)eTagCode.breakError);
                        bool anyGood = clientTags.Any(x => x.Good && x.Command == eCommand.None && x.WriteTagId == 0 && x.WriteTagValue == null);
                        if (breakError && anyGood == false)
                        {
                            counterFailReq++;
                            NewBreak();
                        }
                        else
                        {
                            ClearCounterBreak();
                        }
                        //...
                    }

                    // Дополнительный контроль:
                    // источник отключен
                    //if (Opened == false)
                    //    Tag.CodeMessageList(tags, new CodeMessage((int)eTagCode.sourceClosed, Tag.CodeText(eTagCode.sourceClosed)));
                    // группа отключена
                    //if (group.Off)
                    //    Tag.CodeMessageList(tags.Where(x => x.groupId == group.Id).ToList(), new CodeMessage((int)eTagCode.groupOff, Tag.CodeText(eTagCode.groupOff)));
                    // теги отключены
                    //Tag.CodeMessageList(tags.Where(x => x.Off).ToList(), new CodeMessage((int)eTagCode.tagOff, Tag.CodeText(eTagCode.tagOff)));

                    // Вывод
                    Task.Delay(10);

                    int all = tags.Count();
                    int good = tags.Count(x => x.Good == true);
                    eventReq?.Invoke(Id, groupId, clientTags.Select(x => x as ITagResult).ToList(), counterReq, counterFailReq, all, good);
                }
                finally
                {
                    Monitor.Exit(locker);
                }

                // если это было в очереди то удаляем из очереди
                if (roll[groupId])
                {
                    roll[groupId] = false;
                }

                _process = false;
                groupNow = 0;

                // смотрим, что еще есть в очереди
                if (roll.Count > 0)
                {
                    var next = roll.FirstOrDefault(x => x.Value == true);
                    if (next.Key > 0)
                    {
                        //Console.WriteLine($"source ID {Id} group ID {next.Key} recalc");
                        EventRequestRUN(next.Key);
                    }
                }

            }
            else
            {
                //Console.WriteLine($"source ID {Id} group ID {groupId} lock...");
                // там занято, ставим в очередь
                if (groupId != groupNow)
                {
                    //Console.WriteLine($"source ID {Id} group ID {groupId} add roll");
                    roll[Id] = true;
                }
            }
        }

        void EventTraffic(string message)
        {
            eventTraffic?.Invoke(Id, message);
        }

        // --------------------------------------------------------------------------------------------------

        // is NetDevice?
        public bool IsNet()
        {
            return (device is INetDevice);
        }

        // Есть ли ping?
        public bool IsPing()
        {
            if (IsNet() == false)
                return true;

            return (device as INetDevice).IsPing("", 0);
        }

        // Есть ли host?
        public CodeMessage IsHost()
        {
            if (IsNet() == false)
                return new CodeMessage(0,"");

            if ((device as DeviceNet).disableHostForOpen)
                return (IsPing()) ? new CodeMessage(0, "") : new CodeMessage((int)eTagCode.noPing, "No ping");

            return (device as INetDevice).IsHost("", 0);
        }

        public void Link()
        {
            var useTags = Tag.items.Where(x => x.sourceId == this.Id).ToList();

            this.UseTags(useTags);
            this.UseGroups(Group.items.Where(x => useTags.Select(y => y.groupId).Contains(x.Id)).ToList());
        }

        // ================================================================================================

        static public List<Source> items = new List<Source>(); // все группы
        static public ushort lastId = 0;
        static public bool log = false;

        static public void Clear()
        {
            Source.lastId = 0;
            Source.items = new List<Source>();
        }

        static public Source Item(ushort Id) => items.FirstOrDefault(x => x.Id == Id);
        static public Source Item(string title) => items.FirstOrDefault(x => x.title == title);

        static public bool InProject(dynamic output) => JsonControl.IsProp(output, "Sources");

        static public string StatusTextDelete(eSourceStatus status)
        {
            switch (status)
            {
                case eSourceStatus.cycle:
                    return "Работает";
                case eSourceStatus.breaking:
                    return "Закрытие по ошибке...";
                case eSourceStatus.closed:
                    return "Закрыто";
                case eSourceStatus.closing:
                    return "Закрытие...";
                case eSourceStatus.noClient:
                    return "Ошибка создания клиента";
                case eSourceStatus.openedNoCycle:
                    return "Открыто, но нет опроса";
                case eSourceStatus.opening:
                    return "Открытие...";
                case eSourceStatus.wait:
                    return "Ожидание...";
                default:
                    return status.ToString();
            }
        }

        static public void ActivateItems()
        {
            foreach (var item in items)
            {
                item.Activate();
            }
        }

        // Получение параметров источника
        static public void ParseItemSource(dynamic item, uint forId, out string title, out eDriverType driver, out string connection, out string groupTitle, out bool off, out string description, out dynamic tags, out bool auto, out bool reopen)
        {
            title = JsonControl.GetString(item, "Title", $"Source #{forId}");
            driver = JsonControl.GetTypeEnum<eDriverType>(item, "Driver", eDriverType.None);
            connection = JsonControl.GetString(item, "Address");
            groupTitle = JsonControl.GetString(item, "Group");
            off = JsonControl.GetBool(item, "Off");
            description = JsonControl.GetString(item, "Desc");
            tags = JsonControl.IsProp(item, "Tags") ? item.Tags : null;
            auto = JsonControl.GetBool(item, "Auto");
            reopen = JsonControl.GetBool(item, "Reconnect");
        }

        // Привязки драйверов
        static public void LinkSources()
        {
            foreach (var item in Source.items)
            {
                var useTags = Tag.items.Where(x => x.sourceId == item.Id).ToList();

                item.UseTags(useTags);
                item.UseGroups(Group.items.Where(x => useTags.Select(y => y.groupId).Contains(x.Id)).ToList());
            }
        }

        // -----------------------------------------------------------------------------------------------------------

        // Справки
        static public Dictionary<string, string> HelpDicSource(eDriverType driverType)
        {
            switch (driverType)
            {
                case eDriverType.None:
                    break;

                case eDriverType.Formula:
                    return Formula.GetHelpSource();

                case eDriverType.Application:
                    return AppDevice.GetHelpSource();

                case eDriverType.ModbusTCPclient:
                    return ModbusTCPClient.GetHelpSource();

                case eDriverType.ModbusRTUclient:
                    return ModbusRTUClient.GetHelpSource();

                case eDriverType.AppUDP:
                    return AppUDP.GetHelpSource();

                case eDriverType.MSSQLclient:
                    return MSSQLclient.GetHelpSource();

                case eDriverType.OPCUAclient:
                    return HylasoftOPCUA.GetHelpSource();
            }
            return new Dictionary<string, string>();
        }
        static public Dictionary<string, string> HelpDicTag(eDriverType driverType)
        {
            switch (driverType)
            {
                case eDriverType.None:
                    break;

                case eDriverType.Formula:
                    return Formula.GetHelpTag();

                case eDriverType.Application:
                    return AppDevice.GetHelpTag();

                case eDriverType.ModbusTCPclient:
                    return ModbusTCPClient.GetHelpTag();

                case eDriverType.ModbusRTUclient:
                    return ModbusRTUClient.GetHelpTag();

                case eDriverType.AppUDP:
                    return AppUDP.GetHelpTag();

                case eDriverType.MSSQLclient:
                    return MSSQLclient.GetHelpTag();

                case eDriverType.OPCUAclient:
                    return HylasoftOPCUA.GetHelpTag();
            }
            return new Dictionary<string, string>();
        }

    }

    public class SourceEditor // Редактирование
    {
        public uint Id; // Уникальный идентификатор (0 - нет Id)
        public uint groupId; // ID группы (0 - нет Id)
        public string groupTitle; // Название группы
        public eDriverType driver; // Тип драйвера
        public string title; // Название драйвера
        public string address; // Строка подключения
        public bool off; // Отключение
        public string description; // Описание
        public bool auto; // Запуск опроса после открытия файла
        public bool reconnect; // Автоматическое переподключение
    }
}
