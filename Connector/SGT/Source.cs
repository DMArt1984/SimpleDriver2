using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DML.Log;
using System.Threading;
using System.Collections.Concurrent;
using LogCodeMessage;

namespace Connector
{
    
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

    public class Source : BaseLogger, ISource
    {
        public ushort Id { get; } // ID источника данных
        public string title { get; } // Название источника
        public string description { get; } // Описание источника

        // Вместо локального списка групп используем вычисляемое свойство
        public IEnumerable<Group> Groups => Group.items.Where(g => g.ParentSource == this);

        // Теги больше не хранятся локально, их можно вычислить через группы
        public int TagsCount => Groups.Sum(g => g.Tags.Count);
        public int TagsCountGood => Groups.Sum(g => g.Tags.Count(x => x.Good));

        // Справка
        public Dictionary<string, string> helpSource => SourceHelp.HelpDicSource(_driverType);
        public Dictionary<string, string> helpTag => SourceHelp.HelpDicTag(_driverType);
        static public Dictionary<string, string> GetHelpSource(eDriverType type)
        {
            return SourceHelp.HelpDicSource(type);
        }

        #region Events
        public delegate void HandlerError(ushort Id, CodeMessage activeError);
        public event HandlerError eventError;

        public delegate void HandlerStatus(ushort Id, eSourceStatus status);
        public event HandlerStatus eventStatus;

        public delegate void HandlerInfo(SourceParam info);
        public event HandlerInfo eventParams;

        public delegate void HandlerReq(ushort sourceId, ushort groupId, List<ITagResult> results, int counter, int fails, int all, int good);
        public event HandlerReq eventReq;
        #endregion

        #region Delegates
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
                codeMessage = result;
            Status = (result.code == 0) ? eSourceStatus.closed : eSourceStatus.noClient;

            logger.Info($" step4: _disable = ", eMessageCategory.Source);
            _disable = _disable || result.code != 0;
        }

        public void Activate()
        {
            Off = _disable;
        }

        // -----------------------------------------------------------------------------------------

        // Одиночный запрос
        public void OneRequest()
        {
            // Для запроса со всеми тегами передаем специальный идентификатор (например, 0)
            EventRequest(new Group(0, "", this));
        }

        // =========================================================================================

        bool UserUseClosed = false;

        public bool Off
        {
            get => _off;
            set
            {
                // Если значение не изменилось, ничего не делаем
                if (_off == value)
                    return;

                _off = value;

                if (!_off) // Включение источника
                {
                    UserUseClosed = false;
                    Open(); // синхронный вызов открытия
                }
                else // Отключение источника
                {
                    if (Status != eSourceStatus.closed && Status != eSourceStatus.closing)
                    {
                        UserUseClosed = true;
                        // Вызываем асинхронное закрытие, не дожидаясь завершения
                        CloseAsync(true).ConfigureAwait(false);
                    }
                }
            }
        }
        private bool _off = true;


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
           //var dic = ParamsForSource(Address);

            logger.Info($"Подключить: {this.Id} {this.title} > {newAddress}. Шаг 3 - пересоздание клиента", eMessageCategory.Source);
            _device.CreateClient(Address); // пересоздание клиента
            Off = false; // открыть
            logger.Info($"Подключить: {this.Id} {this.title} > {newAddress}. Шаг 4 - завершено", eMessageCategory.Source);
        }

        private int Open()
        {
            if (Status != eSourceStatus.closed)
                return 1;

            Status = eSourceStatus.opening;
            CodeMessage result = TryOpenConnection();

            if (result.code == 0)
            {
                UpdateSuccessState();
            }
            else
            {
                UpdateFailureState(result);
            }

            EventStatus();
            return result.code;
        }

        /// <summary>
        /// Пытается открыть соединение с устройством. Если устройство является сетевым и настроено без проверки хоста,
        /// проверяет доступность хоста; иначе пытается выполнить TCP-подключение, а затем подключиться через IRealDevice.
        /// </summary>
        private CodeMessage TryOpenConnection()
        {
            CodeMessage result = new CodeMessage();

            if (_device is INetDevice && (_device as DeviceNet).disableHostForOpen)
            {
                if (!IsHostReachable())
                    result = Tag.CM.NoPing;
            }
            else
            {
                result = TryTcpConnect();
            }

            if (result.code == 0)
            {
                logger.Info($"Источник ID={Id} {title} > Соединение...", eMessageCategory.Source);
                if (_device is IRealDevice xdevice)
                    result = xdevice.Connect(Address);
                else
                    result = new CodeMessage();
            }
            else
            {
                logger.Info($"Источник ID={Id} {title} > Нет связи с хостом/IP", eMessageCategory.Source);
            }

            if (result.code != 0)
                codeMessage = result;

            return result;
        }

        /// <summary>
        /// Обновляет внутреннее состояние при успешном открытии соединения.
        /// </summary>
        private void UpdateSuccessState()
        {
            logger.Info($"Источник ID={Id} {title} > Открыть - успешно!", eMessageCategory.Source);
            _opened = true;
            _fail = false;
            stepReOpen = 0;
            ClearCounterBreak();
            // Обновляем статус тегов во всех группах (при необходимости раскомментируйте)
            // var allTags = Groups.SelectMany(g => g.Tags).ToList();
            // Tag.SetCodeMessageForList(allTags, CodeMessageFactory.FromEnumX(eTagStatus.sourceOpened));

            if (AutoRequestAftereOpen)
                CyclicRequest = true;
        }

        /// <summary>
        /// Обновляет внутреннее состояние и инициирует переподключение при ошибке открытия.
        /// </summary>
        private void UpdateFailureState(CodeMessage result)
        {
            logger.Info($"Источник ID={Id} {title} > Открыть - ошибка {result.code} {result.message}", eMessageCategory.Source);
            Status = eSourceStatus.breaking;
            _fail = true;
            ClearCounterBreak();
            var allTags = Groups.SelectMany(g => g.Tags).ToList();
            Tag.SetCodeMessageForList(allTags, CodeMessageFactory.FromEnumX(eTagStatus.sourceDisable));
            OpenAfterFail();
        }

        // ------------------------------------------

        private async Task<int> CloseAsync(bool user = false)
        {
            logger.Info($"Источник ID={Id} {title} > Закрыть...", eMessageCategory.Source);

            // Если источник уже закрыт, обновляем статусы тегов и возвращаем код по умолчанию.
            if (Status == eSourceStatus.closed)
            {
                UpdateAllTagsStatus(eTagStatus.sourceDisable);
                return 1;
            }

            // Переходим в состояние закрытия.
            Status = eSourceStatus.closing;
            logger.Info($"Источник ID={Id} {title} > Ждем завершения процесса...", eMessageCategory.Source);
            await WaitUntilProcessCompletesAsync();

            // Пытаемся отключить устройство.
            CodeMessage result = DisconnectDevice();

            if (result.code != 0)
            {
                codeMessage = result;
                logger.Info($"Источник ID={Id} {title} > Закрыть - ошибка {result.code} {result.message}", eMessageCategory.Source);
            }
            else
            {
                logger.Info($"Источник ID={Id} {title} > Закрыть - успешно!", eMessageCategory.Source);
                ResetSourceState();
                UpdateAllTagsStatus(eTagStatus.sourceDisable);

                if (!user)
                    AttemptReopenAfterFail();
            }

            EventStatus();
            logger.Info($"Источник ID={Id} {title} > Код {result.code}", eMessageCategory.Source);
            return result.code;
        }

        private CodeMessage DisconnectDevice()
        {
            var xdevice = _device as IRealDevice;
            if (xdevice != null)
            {
                return xdevice.Disconnect();
            }
            return new CodeMessage();
        }

        private void UpdateAllTagsStatus(eTagStatus newStatus)
        {
            var allTags = Groups.SelectMany(g => g.Tags).ToList();
            Tag.SetCodeMessageForList(allTags, CodeMessageFactory.FromEnumX(newStatus));
        }

        private void ResetSourceState()
        {
            CyclicRequest = false;
            _opened = false;
            counterReq = 0;
            counterFailReq = 0;
        }

        private async Task WaitUntilProcessCompletesAsync()
        {
            logger.Info("Ожидание завершения процесса...", eMessageCategory.Source);
            DateTime dt = DateTime.Now;
            while (_process && (DateTime.Now - dt).TotalMilliseconds < 5000)
            {
                await Task.Delay(100);
            }
            logger.Info("Ожидание завершено.", eMessageCategory.Source);
        }

        private void AttemptReopenAfterFail()
        {
            if (!AutoOpenAfterFail || !Fail || UserUseClosed)
                return;

            logger.Info($"Источник ID={Id} Reopen: delay = {rTimeMsec[stepReOpen]} ms", eMessageCategory.App);
            try
            {
                _reconnectTimer.Start(rTimeMsec[stepReOpen], TimerCB_Inner);
                stepReOpen++;
                if (stepReOpen >= rTimeMsec.Length)
                    stepReOpen = 0;
                logger.Info($"Источник ID={Id} Reconnect step = {stepReOpen}", eMessageCategory.App);
            }
            catch (Exception ex)
            {
                logger.Error(ex.HResult, $"OpenAfterFail() error: {ex.Message}", eMessageCategory.Source);
                TimerCB_Inner();
            }
        }

        private void OpenAfterFail()
        {
            // Просто вызываем метод, отвечающий за попытку переподключения
            AttemptReopenAfterFail();
        }


        // ------------------------------------------

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
                Open();
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
                        var allTags = Groups.SelectMany(g => g.Tags).ToList();
                        //Tag.SetCodeMessageForList(allTags, CodeMessageFactory.FromEnumX(eTagStatus.sourceOpened));
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
        eSourceStatus _status = eSourceStatus.zero;

        public CodeMessage codeMessage
        {
            get => _codeMessage;
            set
            {
                bool newCode = _codeMessage.code != value.code;
                _codeMessage = value;
                if (_codeMessage.code < 0)
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
                    eventError?.Invoke(Id, _codeMessage);
                }
            }
        }
        CodeMessage _codeMessage;

        

        // ================================================================================================
        // Параметры
        void EventChangeParam()
        {
            eventParams?.Invoke(new SourceParam(Id, Address, AutoRequestAftereOpen, AutoOpenAfterFail));
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
        ushort groupNow = 0;
        public async Task EventRequestRUNAsync(ushort groupId)
        {
            if (!await _requestSemaphore.WaitAsync(TimeSpan.FromSeconds(10)))
            {
                _requestQueue.Enqueue(groupId);
                return;
            }
            try
            {
                _process = true;
                groupNow = groupId;

                // Выбираем теги для опроса
                List<Tag> clientTags;
                if (groupId == 0)
                {
                    return;
                    //clientTags = Groups.SelectMany(g => g.Tags).Where(x => !x.Off).ToList();
                }

                var group = Groups.FirstOrDefault(g => g.Id == groupId);
                clientTags = group != null ? group.Tags.Where(x => !x.Off).ToList() : new List<Tag>();
                
                if (clientTags.Any())
                {
                    await Task.Run(() => _device.Request(clientTags));
                    counterReq++;
                    HandleTagErrors(clientTags);
                }

                await Task.Delay(10); // отдохнем!

                // все группы
                int all = Groups.SelectMany(g => g.Tags).Count();
                int good = Groups.SelectMany(g => g.Tags).Count(x => x.Good);
                // только текущей группы
                int allx = clientTags.Count;
                int goodx = clientTags.Count(x => x.Good);
                // 
                eventReq?.Invoke(Id, groupId, clientTags.Select(x => (ITagResult)x).ToList(), counterReq, counterFailReq, all, good);
            }
            finally
            {
                _requestSemaphore.Release();
                _process = false;
                groupNow = 0;
            }
        }

        private TagErrorHandler _errorHandler = new TagErrorHandler(10);
        /// <summary>
        /// Обработка ошибок при опросе тегов: если найден breakError и нет корректных тегов, увеличиваем счётчик неудачных запросов и вызываем NewBreak.
        /// Иначе сбрасываем счётчик.
        /// </summary>
        /// <param name="clientTags">Список опрашиваемых тегов</param>
        private void HandleTagErrors(List<Tag> clientTags)
        {
            if (_errorHandler.ProcessErrors(clientTags))
            {
                counterFailReq++;
                NewBreak();
            }
        }
        public void NewBreak()
        {
            counterBreak++;
            logger.Info($"NEW BREAK = {counterBreak} / {MaxBreak}", eMessageCategory.Source);
            if (counterBreak >= MaxBreak)
            {
                ClearCounterBreak();
                codeMessage = Tag.CM.BreakError;
                logger.Info($"NEW BREAK = ActiveError", eMessageCategory.Source);
            }
        }

        public void ClearCounterBreak()
        {
            counterBreak = 0;
        }

        public byte MaxBreak { get; set; } = 10;
        public byte counterBreak { get; set; }

        // ======= Конец реализации очереди запросов =============

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
                return (IsHostReachable()) ? new CodeMessage() : Tag.CM.NoPing;
            return (_device as INetDevice).TryTcpConnect("", 0, 0);
        }

        // ==================================================================================================
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
        #endregion
    }


}
