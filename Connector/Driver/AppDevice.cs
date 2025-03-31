
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connector.Driver
{
    class AppDevice : Device
    {
        public const string driverName = "Application";

        public static Dictionary<string, dynamic> CashValues = new Dictionary<string, dynamic>();

        public struct DMTimer
        {
            public byte status;
            public DateTime dt;
        }

        public static Dictionary<string, DMTimer> Timers = new Dictionary<string, DMTimer>();

        // Справка
        // Описание адреса устройства
        public static Dictionary<string, string> HelpSource
            => new Dictionary<string, string> {
                        { "Адрес", "Адресом должна быть пустая строка" }
                    };
        // Описание адреса тега для данного устройства
        public static Dictionary<string, string> HelpTag
            => new Dictionary<string, string> {
                        { "app@title", "Название файла" },
                        { "source@count", "Количество источников" },
                        { "source@list", "Список источников" },
                        { "source@ ID или Название @ Свойство", "Получение свойства" },
                        { "source@1@title", "Название источника с ID 1" },
                        { "source@3@title", "Название источника с ID 3" },
                        { "source@Segnetics@id", "ID источника с названием Segnetics" },
                        { "source@...@Свойства:", "id - ID источника \r\ntitle - Название источника \r\ndriver - Название драйвера источника \r\ncode - Код состояния источника \r\nmessage - Описание кода источника \r\ndescription - Описание источника \r\nlasterrorcode - Код последней ошибки \r\nlasterrormessage - Описание последней ошибки \r\ntagscount - Количество тегов источника \r\ntagsgood - Количество тегов источника хорошего качества \r\non - Включен ли источник \r\naddress - Адрес источника" },
                        { "Продолжение следует", "" }
                    };

        private Dictionary<string, bool> fronts = new Dictionary<string, bool>(); // фронт для записи тегов один раз

        public AppDevice()
        {
            CashValues = new Dictionary<string, dynamic>();
            Timers = new Dictionary<string, DMTimer>();
        }

        ~AppDevice()
        {

        }

        // ---------------------------------------------------------------------------------------------

        // Получить значение тега (с типом)
        public override TagResult GetValue(string address, eDataType DataType)
        {
            return StaticGetValue(address, DataType);
        }

        static public TagResult StaticGetValue (string address, eDataType DataType)
        {
            dynamic Value = 0; // итоговое значение

            try
            {
                if (String.IsNullOrWhiteSpace(address) == false)
                {
                    // Вычисления...
                    string[] part = address.Split('@');
                    if (part.Length >= 2)
                    {
                        switch (part[0].ToLower())
                        {
                            case "app": // приложение
                                switch (part[1].ToLower())
                                {
                                    case "title":
                                        //Value = (DataType == eDataType.STRING) ? Dispatcher.screenTitle : "";
                                        break;

                                    case "time":
                                        Value = (DataType == eDataType.STRING) ? DateTime.Now.TimeOfDay.ToString() : "";
                                        break;

                                    case "date":
                                        Value = (DataType == eDataType.STRING) ? DateTime.Now.Date.ToString() : "";
                                        break;

                                    default:

                                        break;
                                }
                                break;

                            case "license": // лицензия
                                switch (part[1].ToLower())
                                {
                                    case "enable":
                                        //Value = License.LicenseControl.TagControlEnabled;
                                        break;

                                    case "check":
                                        //Value = License.LicenseControl.CheckTagValue();
                                        break;
                                }
                                break;

                            case "webserver": // web сервер
                                switch (part[1].ToLower())
                                {
                                    case "run":
                                        //Value = WEB.WebServer.run;
                                        break;

                                    case "port":
                                        //Value = WEB.WebServer.port;
                                        break;

                                    case "requests":
                                        //Value = WEB.WebServer.items.Select(x => x.urlRequest).ToArray();
                                        break;
                                }
                                break;

                            case "source": // источник данных
                                switch (part[1].ToLower())
                                {
                                    case "count":
                                        Value = Source.items.Count();
                                        break;
                                    case "list":
                                        Value = Source.items.Select(x => x.title).ToArray();
                                        break;

                                    default:
                                        Source source = null;
                                        bool idOK = ushort.TryParse(part[1], out ushort Id);
                                        if (idOK)
                                        {
                                            source = Source.Item(Id); // .items.FirstOrDefault(x => x.Id == Id);
                                        }
                                        else
                                        {
                                            source = Source.Item(part[1]); //.items.FirstOrDefault(x => x.title == part[1]);
                                        }

                                        if (source != null && part.Length >= 3)
                                        {
                                            switch (part[2].ToLower())
                                            {
                                                case "title":
                                                    Value = source.title;
                                                    break;
                                                case "id":
                                                    Value = source.Id;
                                                    break;
                                                case "driver":
                                                    Value = source.driverType.ToString();
                                                    break;
                                                case "code":
                                                    Value = (int)source.Status;
                                                    break;
                                                case "infomessage":
                                                    Value = source.Status.GetText();
                                                    break;
                                                case "error":
                                                    Value = source.ActiveError.code;
                                                    break;
                                                case "message":
                                                    Value = source.ActiveError.message;
                                                    break;
                                                case "description":
                                                    Value = source.description;
                                                    break;
                                                case "lasterrorcode":
                                                    Value = 0; // source;
                                                    break;
                                                case "lasterrormessage":
                                                    Value = ""; // source.adapter.lastError.message;
                                                    break;
                                                case "tagscount":
                                                    Value = source.TagsCount;
                                                    break;
                                                case "tagsgood":
                                                    Value = source.TagsCountGood;
                                                    break;
                                                case "on":
                                                    Value = !source.Off;
                                                    break;
                                                case "address":
                                                    Value = source.Address;
                                                    break;
                                                case "ping":
                                                    Value = source.IsHostReachable();
                                                    break;
                                                case "host":
                                                    Value = source.TryTcpConnect();
                                                    break;
                                                case "dicvalue":
                                                    var resItems = Tag.items.Where(x => x.sourceId == source.Id).ToArray();
                                                    if (resItems != null && resItems.Any())
                                                    {
                                                        Value = resItems.Select(x => $"{x.title}~{Tag.ValuesString(x.LastGoodValue)}~{Tag.ValuesString(x.codeMessage.code)}").ToArray();
                                                    }
                                                    else
                                                    {
                                                        Value = new string[] { };
                                                    }
                                                    break;

                                                case "cmd": // Подключить / Отключить
                                                    if (part.Length >= 4)
                                                    {
                                                        switch (part[3].ToLower())
                                                        {
                                                            case "on": // Подключить
                                                            case "true":
                                                            case "1":
                                                                source.Off = false;
                                                                Value = true;
                                                                break;

                                                            case "off": // Отключить
                                                            case "false":
                                                            case "0":
                                                                source.Off = true;
                                                                Value = false;
                                                                break;

                                                        }
                                                    }
                                                    break;
                                            }
                                        }

                                        break;
                                }
                                break;

                            case "group": // группа
                                switch (part[1].ToLower())
                                {
                                    case "count":
                                        Value = Group.items.Count();
                                        break;
                                    case "list":
                                        Value = Group.items.Select(x => x.title).ToArray();
                                        break;

                                    default:
                                        Group group = null;
                                        bool idOK = ushort.TryParse(part[1], out ushort Id);
                                        if (idOK)
                                        {
                                            group = Group.Item(Id); // .items.FirstOrDefault(x => x.Id == Id);
                                        }
                                        else
                                        {
                                            group = Group.Item(part[1]); //.items.FirstOrDefault(x => x.title == part[1]);
                                        }

                                        if (group != null && part.Length >= 3)
                                        {
                                            switch (part[2].ToLower())
                                            {
                                                case "title":
                                                    Value = group.title;
                                                    break;
                                                case "id":
                                                    Value = group.Id;
                                                    break;
                                                case "description":
                                                    Value = group.description;
                                                    break;
                                                case "tagscount":
                                                    Value = group.TagsCount;
                                                    break;
                                                case "tagsgood":
                                                    Value = group.TagsCountGood;
                                                    break;
                                                case "on":
                                                    Value = !group.Off;
                                                    break;
                                                case "off":
                                                    Value = group.Off;
                                                    break;
                                                case "interval":
                                                    Value = group.UpdateRate;
                                                    break;
                                                case "dicvalue":
                                                    var resItems = Tag.items.Where(x => x.groupId == group.Id).ToArray();
                                                    if (resItems != null && resItems.Any())
                                                    {
                                                        Value = resItems.Select(x => $"{x.title}~{Tag.ValuesString(x.LastGoodValue)}~{x.codeMessage.code}").ToArray();
                                                    } else
                                                    {
                                                        Value = new string[] { };
                                                    }
                                                    break;
                                                case "cmd": // Подключить / Отключить
                                                    if (part.Length >= 4)
                                                    {
                                                        switch (part[3].ToLower())
                                                        {
                                                            case "on": // Подключить
                                                            case "true":
                                                            case "1":
                                                                group.Off = false;
                                                                Value = true;
                                                                break;

                                                            case "off": // Отключить
                                                            case "false":
                                                            case "0":
                                                                group.Off = true;
                                                                Value = false;
                                                                break;

                                                        }
                                                    }
                                                    break;
                                            }
                                        }

                                        break;
                                }
                                break;

                            case "tag": // тег
                                switch (part[1].ToLower())
                                {
                                    case "count":
                                        Value = Tag.items.Count();
                                        break;
                                    case "list":
                                        Value = Tag.items.Select(x => x.title).ToArray();
                                        break;
                                    case "good":
                                        Value = (short)Tag.items.Count(x => x.Good);
                                        break;
                                    case "dicvalue":
                                        Value = Tag.items.Select(x => $"{x.title}~{Tag.ValuesString(x.LastGoodValue)}~{x.codeMessage.code}").ToArray();
                                        break;

                                    default:
                                        Tag tag = null;
                                        bool idOK = ushort.TryParse(part[1], out ushort Id);
                                        if (idOK)
                                        {
                                            tag = Tag.Item(Id); // Tag.items.FirstOrDefault(x => x.Id == Id);
                                        }
                                        else
                                        {
                                            tag = Tag.Item(part[1]); // Tag.items.FirstOrDefault(x => x.title == part[1]);
                                        }

                                        if (tag != null && part.Length >= 3)
                                        {
                                            switch (part[2].ToLower())
                                            {
                                                case "title":
                                                    Value = tag.title;
                                                    break;
                                                case "id":
                                                    Value = tag.Id;
                                                    break;
                                                case "infocode2": // del
                                                    Value = (tag.Good) ? 0 : (tag.LastError.code == 0) ? 255 : tag.LastError.code;
                                                    break;
                                                case "infomessage2": // del
                                                    Value = (tag.Good) ? "" : (String.IsNullOrWhiteSpace(tag.LastError.message)) ? "нет данных" : tag.LastError.message;
                                                    break;
                                                case "infocode":
                                                    Value = tag.codeMessage.code;
                                                    break;
                                                case "infomessage":
                                                    Value = tag.codeMessage.message;
                                                    break;
                                                case "code":
                                                    Value = tag.LastError.code;
                                                    break;
                                                case "message":
                                                    Value = tag.LastError.message;
                                                    break;
                                                case "description":
                                                    Value = tag.description;
                                                    break;
                                                case "address":
                                                    Value = tag.Address;
                                                    break;
                                                case "on":
                                                    Value = !tag.Off;
                                                    break;
                                                case "datatype":
                                                    Value = tag.DataType;
                                                    break;
                                                case "good":
                                                    Value = tag.Good;
                                                    break;
                                                case "value":
                                                    Value = tag.LastGoodValue;
                                                    break;
                                                case "sourceid":
                                                    Value = tag.sourceId;
                                                    break;
                                                case "sourcetitle":
                                                    Value = tag.sourceTitle;
                                                    break;
                                                case "groupid":
                                                    Value = tag.groupId;
                                                    break;
                                                case "grouptitle":
                                                    Value = tag.groupTitle;
                                                    break;
                                                case "cmdplay":
                                                    if (part.Length >= 4)
                                                    {
                                                        bool B = Tag.ConvertValue(part[3], eDataType.Bool);
                                                        if (B && tag.Command == eCommand.Wait)
                                                            tag.Command = eCommand.Play;
                                                        Value = B;
                                                    }
                                                    break;
                                                case "cmd": // Подключить / Отключить
                                                    if (part.Length >= 4)
                                                    {
                                                        switch (part[3].ToLower())
                                                        {
                                                            case "on": // Подключить
                                                            case "true":
                                                            case "1":
                                                                tag.Off = false;
                                                                Value = true;
                                                                break;

                                                            case "off": // Отключить
                                                            case "false":
                                                            case "0":
                                                                tag.Off = true;
                                                                Value = false;
                                                                break;

                                                        }
                                                    }
                                                    break;
                                            }
                                        }

                                        break;
                                }
                                break;

                            case "attention": // сообщения
                                string mask = (part.Length >= 3) ? part[2] : "";
                                switch (part[1].ToLower())
                                {
                                    case "count":
                                        //Value = Attention.CountsAll(mask);
                                        break;
                                    case "info":
                                        //Value = Attention.CountsInfo(mask);
                                        break;
                                    case "warning":
                                        //Value = Attention.CountsWarning(mask);
                                        break;
                                    case "alarm":
                                        //Value = Attention.CountsAlarm(mask);
                                        break;

                                    default:
                                        break;
                                }
                                break;

                            case "sound": // звук
                                switch (part[1].ToLower())
                                {
                                    case "count":
                                        //Value = SoundBox.Count();
                                        break;
                                    case "playing":
                                        //Value = SoundBox.playing;
                                        break;
                                    case "activefilename":
                                        //Value = (SoundBox.playing) ? SoundBox.activeFileName : "";
                                        break;

                                    default:
                                        break;
                                }
                                break;

                            case "lad": // функции сравнения переменных
                                switch (part[1].ToLower())
                                {
                                    case "modifed":
                                        {
                                            var wTag = Tag.items.FirstOrDefault(x => x.title == part[2]);
                                            if (wTag != null)
                                            {
                                                if (CashValues.ContainsKey(wTag.title) == false)
                                                    CashValues.Add(wTag.title, wTag.Value);

                                                Value = wTag.Value != CashValues[wTag.title]; // значение меняется
                                                CashValues[wTag.title] = wTag.Value;
                                            }
                                            else
                                            {
                                                Value = false;
                                            }
                                        }
                                        break;

                                    case "frozen":
                                        {
                                            var wTag = Tag.items.FirstOrDefault(x => x.title == part[2]);
                                            if (wTag != null)
                                            {
                                                if (CashValues.ContainsKey(wTag.title) == false)
                                                    CashValues.Add(wTag.title, wTag.Value);

                                                Value = wTag.Value == CashValues[wTag.title]; // значение не меняется
                                                CashValues[wTag.title] = wTag.Value;
                                            }
                                            else
                                            {
                                                Value = false;
                                            }
                                        }
                                        break;

                                    case "posfront":
                                        {
                                            var wTag = Tag.items.FirstOrDefault(x => x.title == part[2]);
                                            if (wTag != null)
                                            {
                                                if (CashValues.ContainsKey(wTag.title) == false)
                                                    CashValues.Add(wTag.title, wTag.Value);

                                                Value = Convert.ToBoolean(wTag.Value) == true && Convert.ToBoolean(CashValues[wTag.title]) == false; // позитивный фронт
                                                CashValues[wTag.title] = wTag.Value;
                                            }
                                            else
                                            {
                                                Value = false;
                                            }
                                        }
                                        break;

                                    case "negfront":
                                        {
                                            var wTag = Tag.items.FirstOrDefault(x => x.title == part[2]);
                                            if (wTag != null)
                                            {
                                                if (CashValues.ContainsKey(wTag.title) == false)
                                                    CashValues.Add(wTag.title, wTag.Value);

                                                Value = Convert.ToBoolean(wTag.Value) == false && Convert.ToBoolean(CashValues[wTag.title]) == true; // позитивный фронт
                                                CashValues[wTag.title] = wTag.Value;
                                            }
                                            else
                                            {
                                                Value = false;
                                            }
                                        }
                                        break;
                                }
                                break;

                            case "timerrun": // работа таймеров
                                {
                                    Value = false;
                                    var wTag = Tag.items.FirstOrDefault(x => x.title == part[1]);
                                    if (wTag != null && part.Length >= 4)
                                    {
                                        string timerTitle = part[2]; // название таймера
                                        int mseconds = Convert.ToInt32(part[3]); // милисекунды

                                        if (Timers.ContainsKey(timerTitle) == false)
                                            Timers.Add(timerTitle, new DMTimer {status = 0, dt = DateTime.Now});

                                        if (Convert.ToBoolean(wTag.Value) == true)
                                        {
                                            if (Timers[timerTitle].status == 0)
                                            {
                                                Timers[timerTitle] = new DMTimer {status = 1, dt = DateTime.Now};
                                            } else
                                            {
                                                var span = DateTime.Now.Subtract(Timers[timerTitle].dt);
                                                Value = (span.TotalMilliseconds >= mseconds);
                                            }
                                        } else
                                        {
                                            Timers[timerTitle] = new DMTimer { status = 0, dt = DateTime.Now };
                                        }

                                    }
                                }
                                break;

                            case "timerinfo": // информация по таймерам
                                {
                                    string timerTitle = part[1]; // название таймера
                                    if (Timers.ContainsKey(timerTitle))
                                    {
                                        if (Timers[timerTitle].status == 1)
                                        {
                                            Value = Math.Floor(DateTime.Now.Subtract(Timers[timerTitle].dt).TotalMilliseconds);
                                        } else
                                        {
                                            Value = 0;
                                        }
                                    } else
                                    {
                                        Value = 0;
                                    }
                                }
                                break;
                        }
                    }
                }

                // Вернуть тег
                return new TagResult(Value);
            }
            catch (Exception ex)
            {
                return new TagResult(Value, ex);
            }
        }

        // Записать значение тега
        public override TagResult SetValue(string address, eDataType DataType, dynamic newValue = null)
        {
            if (newValue == null)
                return new TagResult(newValue, eTagCode.newValueIsNull);

            // добавляем адрес в список
            if (fronts.ContainsKey(address) == false)
                fronts.Add(address, false);

            // newValue
            if (Tag.ConvertValue(newValue, eDataType.Bool) == false)
            {
                fronts[address] = false;
                return new TagResult(newValue);
            }

            // записываем по положительному фронту
            if (newValue is bool)
            {
                if (fronts[address] == false)
                {
                    fronts[address] = true;
                    var result2 = GetValue(address, DataType);
                    return result2;
                }
                else
                {
                    return new TagResult(newValue);
                }
            }

            // записываем постоянно от newValue=true
            var result = GetValue(address, DataType);
            return result;
        }



    }
}
