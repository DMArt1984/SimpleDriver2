using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DML.Log;
using System.Threading;
using System.Collections.Concurrent;
using DML;
using LogCodeMessage;

namespace Connector
{

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

    class Source : BaseLogger, ISource
    {
        public ushort Id { get; } // ID источника данных
        public string title { get; } // Название источника
        public string description { get; } // Описание источника

        object locker = new object();

        // Теги для источника
        List<Tag> tags = new List<Tag>();
        Dictionary<ushort, List<Tag>> dicTagGroup = new Dictionary<ushort, List<Tag>>();
        public int TagsCount => _tagsCount;
        int _tagsCount = 0;
        public int TagsCountGood => tags.Count(x => x.Good);

        // Справка
        public Dictionary<string, string> helpSource => SourceHelp.HelpDicSource(_driverType);
        public Dictionary<string, string> helpTag => SourceHelp.HelpDicTag(_driverType);
        static public Dictionary<string, string> GetHelpSource(eDriverType type)
        {
            return SourceHelp.HelpDicSource(type);
        }

        #region Delegate
        public delegate void HandlerError(ushort Id, CodeMessage activeError);
        public HandlerError eventError;

        public delegate void HandlerStatus(ushort Id, eSourceStatus status);
        public HandlerStatus eventStatus;

        public delegate void HandlerInfo(SourceParam info);
        public HandlerInfo eventParams;

        public delegate void HandlerReq(ushort sourceId, ushort groupId, List<ITagResult> results, int counter, int fails, int all, int good);
        public HandlerReq eventReq;

        public delegate void HandlerTrafficLog(ushort Id, string message);
        public HandlerTrafficLog logTraffic;

        public delegate void HandlerLog(CodeMessage cm);
        public HandlerLog log;
        #endregion

        // Строка подключения
        public string Address
        {
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

        // Автоматический запуск циклического опроса после открытия
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

        // Автоматическое переоткрытие после ошибки
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
        private int stepReOpen = 0;
        private readonly ReconnectTimer _reconnectTimer = new ReconnectTimer();
        private int[] rTimeMsec = new[] { 5000, 6000, 7000, 8000, 9000, 10000 };

        int counterReq = 0;
        int counterFailReq = 0;

        // Устройство
        private readonly IDevice _device;
        private readonly IDeviceFactory _deviceFactory;

        private eDriverType _driverType;
        public eDriverType driverType => _driverType;

        // Конструкторы
        public Source(ushort Id, string title,
            eDriverType driverType, IDeviceFactory deviceFactory,
            bool disable,
            bool auto, bool reopen, string address = "", string description = "") : base(LogTarget.FileConsoleForm, null)
        {
            _driverType = driverType;
            _deviceFactory = deviceFactory;
            _device = _deviceFactory.CreateDevice(driverType, address);

            if (_device is Device dr)
            {
                dr.logTraffic = LogTraffic;
                dr.log = Log;
            }

            this.Id = Id;
            this.title = title;
            this.description = description;
            this.AutoRequestAftereOpen = auto;
            this.AutoOpenAfterFail = reopen;
            this.Address = address;

            SetClient(address);

            logger.Info($"new SOURCE ID {Id} {title} {driverType} {address}", eMessageCategory.Source);
        }

        ~Source()
        {
            if (_device is Device dr)
            {
                dr.logTraffic = null;
                dr.log = null;
            }
        }

        private void SetClient(string address)
        {
            logger.Info($" step3: CreateClient(paramClient)", eMessageCategory.Source);
            CodeMessage result = (_device as Device)?.CreateClient(address) ?? new CodeMessage(-1, "Invalid device");
            if (result.code != 0)
                ActiveError = result;
            Status = (result.code == 0) ? eSourceStatus.closed : eSourceStatus.noClient;

            logger.Info($" step4: _disable = ", eMessageCategory.Source);
            _disable = _disable || result.code != 0;
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
                var dic = DeviceReal.ParamsToDic(address);
                if (dic.ContainsKey("fails"))
                {
                    MaxBreak = byte.Parse(dic["fails"]);
                    if (MaxBreak < 1)
                        MaxBreak = 1;
                    dic.Remove("fails");
                }
                return dic;
            }
            catch (Exception ex)
            {
                logger.Error(ex.HResult, $"ParamsForSource = {address}: {ex.Message}", eMessageCategory.Source);
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

        // ----------------------------------------------------------------------------
        public void AppendGroup(Group group)
        {
            SourceGroupsHelper.AddGroup(this, group, null);
        }

        public void RemoveGroup(Group group)
        {
            SourceGroupsHelper.RemoveGroup(this, group, null);
        }

        public void UseGroups(List<Group> groups)
        {
            SourceGroupsHelper.UseGroups(this, groups, null);
        }
        // ---------------------------------------------------------------------------

        // Одиночный запрос
        public void OneRequest()
        {
            EventRequest(new Group(0, ""));
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
                    if (Status != eSourceStatus.closed && Status != eSourceStatus.closing)
                    {
                        UserUseClosed = true;
                        Close(true);
                    }
                }
            }
        }
        bool _off = true;

        bool _disable = false;

        public void OnControl(string newAddress)
        {
            logger.Info($"Подключить: {this.Id} {this.title} > {newAddress}. Шаг 1", eMessageCategory.Source);

            if (String.IsNullOrWhiteSpace(newAddress) || newAddress == Address)
            {
                Off = false;
                logger.Info($"Подключить: {this.Id} {this.title} > {newAddress}. Шаг 4 - завершено", eMessageCategory.Source);
                return;
            }

            Address = newAddress; // новый адрес

            logger.Info($"Подключить: {this.Id} {this.title} > {newAddress}. Шаг 2 - новые параметры", eMessageCategory.Source);
            var dic = ParamsForSource(Address);

            logger.Info($"Подключить: {this.Id} {this.title} > {newAddress}. Шаг 3 - пересоздание клиента", eMessageCategory.Source);
            _device.CreateClient(Address); // пересоздание клиента
            Off = false; // открыть
            logger.Info($"Подключить: {this.Id} {this.title} > {newAddress}. Шаг 4 - завершено", eMessageCategory.Source);
        }

        int Open(bool user = false)
        {
            if (Status == eSourceStatus.closed)
            {
                Status = eSourceStatus.opening;
                CodeMessage result = new CodeMessage();

                if (_device is INetDevice && (_device as DeviceNet).disableHostForOpen)
                {
                    var retval = IsHostReachable();
                    if (retval == false)
                        result = CodeMessageFactory.FromEnumX(eTagCode.noPing);
                }
                else
                {
                    result = TryTcpConnect();
                }

                if (result.code == 0)
                {
                    logger.Info($"Источник ID={Id} {title} > Соединение...", eMessageCategory.Source);
                    var xdevice = _device as IRealDevice;
                    if (xdevice != null)
                    {
                        result = xdevice.Connect(Address);
                    }
                    else
                    {
                        result = new CodeMessage();
                    }
                }
                else
                {
                    logger.Info($"Источник ID={Id} {title} > Нет связи с хостом/IP", eMessageCategory.Source);
                }

                if (result.code != 0)
                    ActiveError = result;

                if (result.code == 0)
                {
                    logger.Info($"Источник ID={Id} {title} > Открыть - успешно!", eMessageCategory.Source);
                    _opened = true;
                    _fail = false;
                    stepReOpen = 0;
                    ClearCounterBreak();
                    Tag.CodeMessageList(tags, CodeMessageFactory.FromEnumX(eTagCode.sourceOpened));

                    if (AutoRequestAftereOpen)
                        CyclicRequest = true;
                }
                else
                {
                    logger.Info($"Источник ID={Id} {title} > Открыть - ошибка {result.code} {result.message}", eMessageCategory.Source);
                    Status = eSourceStatus.breaking;
                    _fail = true;
                    ClearCounterBreak();
                    Tag.CodeMessageList(tags, CodeMessageFactory.FromEnumX(eTagCode.sourceFail));
                    OpenAfterFail();
                }

                EventStatus();
                return result.code;
            }
            return 1;
        }

        int Close(bool user = false)
        {
            logger.Info($"Источник ID={Id} {title} > Закрыть...", eMessageCategory.Source);
            if (Status != eSourceStatus.closed)
            {
                Status = eSourceStatus.closing;
                logger.Info($"Источник ID={Id} {title} > Ждем...", eMessageCategory.Source);
                WaitProcess();
                var xdevice = _device as IRealDevice;
                CodeMessage result = new CodeMessage();
                if (xdevice != null)
                {
                    result = xdevice.Disconnect();
                }
                if (result.code != 0)
                    ActiveError = result;

                if (result.code == 0)
                {
                    logger.Info($"Источник ID={Id} {title} > Закрыть - успешно!", eMessageCategory.Source);
                    CyclicRequest = false;
                    _opened = false;
                    counterReq = 0;
                    counterFailReq = 0;
                    logger.Info($"Источник ID={Id} {title} > Статусы тегов...", eMessageCategory.Source);
                    Tag.CodeMessageList(tags, CodeMessageFactory.FromEnumX(eTagCode.sourceClosed));
                    logger.Info($"Источник ID={Id} {title} > 5...", eMessageCategory.Source);

                    if (user == false)
                        OpenAfterFail();
                }
                else
                {
                    logger.Info($"Источник ID={Id} {title} > Закрыть - ошибка {result.code} {result.message}", eMessageCategory.Source);
                }

                EventStatus();
                logger.Info($"Источник ID={Id} {title} > Код {result.code}", eMessageCategory.Source);
                return result.code;
            }
            else
            {
                Tag.CodeMessageList(tags, CodeMessageFactory.FromEnumX(eTagCode.sourceClosed));
            }
            return 1;
        }

        private void OpenAfterFail()
        {
            if (!AutoOpenAfterFail || !Fail || UserUseClosed)
                return;

            logger.Info($"Source ID={Id} Reopen: delay = {rTimeMsec[stepReOpen]} ms", eMessageCategory.App);

            try
            {
                _reconnectTimer.Start(rTimeMsec[stepReOpen], TimerCB_Inner);
                stepReOpen++;
                if (stepReOpen >= rTimeMsec.Length)
                    stepReOpen = 0;

                logger.Info($"Source ID={Id} Reconnect step = {stepReOpen}", eMessageCategory.App);
            }
            catch (Exception ex)
            {
                logger.Error(ex.HResult, $"OpenAfterFail() error: {ex.Message}", eMessageCategory.Source);
                TimerCB_Inner();
            }
        }

        private void TimerCB_Inner()
        {
            logger.Info("REOPEN: TimerCB", eMessageCategory.Source);

            if (!AutoOpenAfterFail)
            {
                logger.Info("REOPEN: AutoOpenAfterFail == false — отмена", eMessageCategory.Source);
                return;
            }

            if (UserUseClosed)
            {
                logger.Info("REOPEN: Закрытие выполнено пользователем — отмена", eMessageCategory.Source);
                return;
            }

            if (!Off)
            {
                logger.Info("REOPEN: Повторная попытка Open()", eMessageCategory.Source);
                Open(false);
            }
            else
            {
                logger.Info("REOPEN: Установка Off = false (будет вызван Open через set)", eMessageCategory.Source);
                Off = false;
            }
        }

        void WaitProcess()
        {
            logger.Info("wait process [", eMessageCategory.Source);
            DateTime dt = DateTime.Now;
            while (_process)
            {
                logger.Info("wait process...", eMessageCategory.Source);
                Thread.Sleep(100);
                TimeSpan ts = DateTime.Now.Subtract(dt);
                if (ts.TotalMilliseconds > 5000)
                    break;
            }
            logger.Info("wait process ]", eMessageCategory.Source);
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
                        WaitProcess();
                        Tag.CodeMessageList(tags, CodeMessageFactory.FromEnumX(eTagCode.sourceOpened));
                    }
                    EventStatus();
                }
            }
        }
        bool _cyclicRequest = false;

        public eSourceStatus Status
        {
            get => _status;
            set
            {
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
                        if (Off)
                        {
                            OpenAfterFail();
                        }
                        else
                        {
                            logger.Info($" -> Off = true;", eMessageCategory.Source);
                            Off = true;
                        }
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
            ActiveError = new CodeMessage();
        }

        public void NewBreak()
        {
            counterBreak++;
            logger.Info($"NEW BREAK = {counterBreak} / {MaxBreak}", eMessageCategory.Source);
            if (counterBreak >= MaxBreak)
            {
                ClearCounterBreak();
                ActiveError = CodeMessageFactory.FromEnumX(eTagCode.breakError);
                logger.Info($"NEW BREAK = ActiveError", eMessageCategory.Source);
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
                }
                else
                {
                    Status = eSourceStatus.openedNoCycle;
                }
            }
            else
            {
                Status = eSourceStatus.closed;
            }
        }

        // ======= Новая реализация очереди запросов =============

        private ConcurrentQueue<ushort> _requestQueue = new ConcurrentQueue<ushort>();
        private int _processing = 0; // 0 - не обрабатывается, 1 - идет обработка

        // Запросы
        internal void EventRequest(IGroupOff group)
        {
            if (group == null || group.Off || !Opened || (!CyclicRequest && group.Id > 0))
                return;
            _requestQueue.Enqueue(group.Id);
            ProcessQueue();
        }

        private async void ProcessQueue()
        {
            if (Interlocked.CompareExchange(ref _processing, 1, 0) != 0)
                return; // уже обрабатывается
            try
            {
                while (_requestQueue.TryDequeue(out ushort groupId))
                {
                    await EventRequestRUNAsync(groupId);
                }
            }
            finally
            {
                Interlocked.Exchange(ref _processing, 0);
            }
        }

        // Асинхронная версия метода обработки запроса
        private SemaphoreSlim _requestSemaphore = new SemaphoreSlim(1, 1);
        public async Task EventRequestRUNAsync(ushort groupId)
        {
            if (!await _requestSemaphore.WaitAsync(TimeSpan.FromSeconds(10)))
            {
                // Если не удалось получить семафор, повторно ставим запрос в очередь
                _requestQueue.Enqueue(groupId);
                return;
            }
            try
            {
                _process = true;
                groupNow = groupId;

                // Выбираем теги для опроса
                List<Tag> clientTags = tags.Where(x => x.groupId == groupId && !x.Off).ToList();
                if (clientTags.Any())
                {
                    await Task.Run(() => _device.Request(clientTags));
                    counterReq++;

                    // Вызов вынесенного метода для обработки ошибок
                    HandleTagErrors(clientTags);
                }

                await Task.Delay(10);

                int all = tags.Count;
                int good = tags.Count(x => x.Good);
                eventReq?.Invoke(Id, groupId, clientTags.Select(x => (ITagResult)x).ToList(), counterReq, counterFailReq, all, good);
            }
            finally
            {
                _requestSemaphore.Release();
                _process = false;
                groupNow = 0;
            }
        }

        /// <summary>
        /// Вынесенная обработка ошибок при опросе тегов.
        /// Если обнаружен breakError и нет корректных тегов, увеличивается счётчик неудачных запросов и вызывается NewBreak.
        /// Иначе, счётчик сбрасывается.
        /// </summary>
        /// <param name="clientTags">Список опрашиваемых тегов</param>
        private void HandleTagErrors(List<Tag> clientTags)
        {
            bool breakError = clientTags.Any(x => x.codeMessage.code == (int)eTagCode.breakError);
            bool anyGood = clientTags.Any(x => x.Good && x.Command == eCommand.None && x.WriteTagId == 0 && x.WriteTagValue == null);
            if (breakError && !anyGood)
            {
                counterFailReq++;
                NewBreak();
            }
            else
            {
                ClearCounterBreak();
            }
        }

        ushort groupNow = 0;
        // ======= Конец новой реализации очереди запросов =============

        // --------------------------------------------------------------------------------------------------

        public void SetLogTraffic(bool enable)
        {
            (_device as IControlTrafficLog).EnableTLog = enable;
        }

        public bool IsLogTraffic()
        {
            return (_device as IControlTrafficLog).EnableTLog;
        }

        public bool IsSupportLog()
        {
            return (_device as IControlTrafficLog).SupportTLog;
        }

        void LogTraffic(string message)
        {
            logTraffic?.Invoke(Id, message);
        }
        void Log(CodeMessage cm)
        {
            log?.Invoke(cm);
        }

        // --------------------------------------------------------------------------------------------------

        // is NetDevice?
        public bool IsNet()
        {
            return (_device is INetDevice);
        }

        public bool IsHostReachable()
        {
            if (IsNet() == false)
                return true;
            return (_device as INetDevice).IsHostReachable("", 0);
        }

        public CodeMessage TryTcpConnect()
        {
            if (IsNet() == false)
                return new CodeMessage();
            if ((_device as DeviceNet).disableHostForOpen)
                return (IsHostReachable()) ? new CodeMessage() : CodeMessageFactory.FromEnumX(eTagCode.noPing);
            return (_device as INetDevice).TryTcpConnect("", 0, 0);
        }

        // ------------------------------------------------------------------------------------------------------

        public void Link()
        {
            var useTags = Tag.items.Where(x => x.sourceId == this.Id).ToList();
            this.UseTags(useTags);
            this.UseGroups(Group.items.Where(x => useTags.Select(y => y.groupId).Contains(x.Id)).ToList());
        }

        // ======================================================================================================

        #region Static

        static public List<Source> items = new List<Source>(); // все источники
        static public ushort lastId = 0;

        static public void Clear()
        {
            Source.lastId = 0;
            Source.items = new List<Source>();
        }

        static public Source Item(ushort Id) => items.FirstOrDefault(x => x.Id == Id);
        static public Source Item(string title) => items.FirstOrDefault(x => x.title == title);

        

        static public void ActivateItems()
        {
            foreach (var item in items)
            {
                item.Activate();
            }
        }

        static public void LinkSources()
        {
            foreach (var item in Source.items)
            {
                var useTags = Tag.items.Where(x => x.sourceId == item.Id).ToList();
                item.UseTags(useTags);
                item.UseGroups(Group.items.Where(x => useTags.Select(y => y.groupId).Contains(x.Id)).ToList());
            }
        }

        #endregion

    }

}
