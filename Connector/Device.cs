using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogCodeMessage;

namespace Connector
{
    public enum eDriverType // тип драйвера
    {
        None = 0,
        Formula = 1,
        Application = 2,
        ModbusTCPclient = 11,
        ModbusRTUclient = 12,
        MSSQLclient = 20,
        AppUDP = 30,
        OPCUAclient = 40
    }

    interface IDevice
    {
        CodeMessage CreateClient(string parameters);
        CodeMessage RemoveClient();
        void Request<T>(List<T> tags) where T : ITagClient;
    }

    //public interface IHelp
    //{
    //    Dictionary<string, string> HelpSource { get; }
    //    Dictionary<string, string> HelpTag { get; }
    //}

    interface IControlTrafficLog
    {
        bool EnableTLog { get; set; }
        bool SupportTLog { get; }
    }

    class Device : IDevice, IControlTrafficLog
    {
        const int version = 1000; // Версия

        public bool EnableTLog { get; set; } = false; // разрешить вести лог
        public virtual bool SupportTLog { get; } = false;

        List<ITagClient> tags = new List<ITagClient>(); // for parallel 

        // делегаты
        public delegate void HandlerTrafficLog(string message);
        public HandlerTrafficLog logTraffic;

        public delegate void HandlerLog(CodeMessage cm);
        public HandlerLog log;

        // IHelp Реализация

        //public virtual Dictionary<string, string> HelpSource
        //{
        //    get { return new Dictionary<string, string>(); }
        //}

        //public virtual Dictionary<string, string> HelpTag
        //{
        //    get { return new Dictionary<string, string>(); }
        //}

        // =================================================================================

        public Device()
        {
            log?.Invoke(new CodeMessage(0, "Device created!"));
        }

        public virtual void Dispose()
        {
            RemoveClient();
        }

        // =================================================================================

        // Создание клиента
        public virtual CodeMessage CreateClient(string parameters = "")
        {
            return new CodeMessage();
        }

        // Удаление клиента
        public virtual CodeMessage RemoveClient()
        {
            return new CodeMessage();
        }

        // ------------------------------------------------------------------------

        public void InnerTrafficLog(string message)
        {
            // лог сообщений от драйвера
            if (EnableTLog)
            {
                logTraffic?.Invoke(message);
            }
        }

        // ===================================================================================

        object locker = new object();

        public virtual void Request<T>(List<T> tags) where T : ITagClient
        {
            lock (locker)
            {
                if (tags == null)
                    return;

                for (var i = 0; i < tags.Count(); i++)
                {
                    WorkTag(tags[i]);
                }
            }
        }

        public virtual void RequestParallel<T>(List<T> tags) where T : ITagClient
        {
            if (tags == null)
                return;

            this.tags = tags.Select( x => x as ITagClient).ToList();
            Parallel.For(0, tags.Count, ItemParallel);
        }

        void ItemParallel(int index)
        {
            WorkTag(tags[index]);
        }

        protected virtual void WorkTag(ITagClient tag)
        {
            string raddress = Tag.ExpTagAddress(tag.Address, out bool success, tag);
            logTraffic?.Invoke($"{tag.title}: {raddress}");

            if (success == false)
            {
                tag.SetResult(new TagResult(null, eTagCode.notReliableA));
                return;
            }

            // Читаем
            bool read = IsRead(tag);
            // Пишем
            bool write = IsWrite(tag);

            if (tag.Command != eCommand.None)
                tag.Command = eCommand.Wait;

            if (read) // чтение
            {
                logTraffic?.Invoke("READ");
                tag.SetResult(GetValue(raddress, tag.DataType));
            } else if (write) // запись
            {
                logTraffic?.Invoke("WRITE");
                if (tag.directFull == eDirectFull.WriteTagValue) // запись из другого тега
                {
                    var writeTagId = tag.WriteTagId;
                    var writeTag = (writeTagId > 0) ? Tag.items.FirstOrDefault(x => x.Id == writeTagId) : null;
                    if (writeTag == null)
                    {
                        tag.SetResult(new TagResult(null, eTagCode.noTagForWrite));
                        return;
                    }
                    if (writeTag.codeMessage.code != 0 && writeTag.codeMessage.code != (int)eTagCode.tagOn)
                    {
                        tag.SetResult(new TagResult(null, eTagCode.notReliableTW));
                        return;
                    }
                    
                    tag.SetResult( SetValue(raddress, tag.DataType, tag.WriteTagValue));
                }
                else // eDirectFull.WriteValue // запись значения
                {
                    tag.SetResult( SetValue(raddress, tag.DataType, tag.WriteConstValue));
                }
            }

            //...

        }

        // =====================================================================================

        private bool IsRead(ITagClient tag)
        {
            return (tag.directFull == eDirectFull.Read && tag.Command != eCommand.Wait) || // (направление чтения И нет ожидания) ИЛИ
                (tag.directFull != eDirectFull.Read && tag.Command == eCommand.Wait); // (направление запись И ожидание)
        }

        private bool IsWrite(ITagClient tag)
        {
            return tag.directFull != eDirectFull.Read && (tag.Command != eCommand.Update || (tag.Value != tag.WriteConstValue && tag.Value != tag.WriteTagValue));
        }

        // =====================================================================================

        public virtual TagResult GetValue(string address, eDataType dataType)
        {
            return new TagResult(0, eTagCode.noData);
        }
        public virtual TagResult SetValue(string address, eDataType dataType, dynamic newValue)
        {
            return new TagResult(newValue, eTagCode.noData);
        }

        // ======================================================================================

        #region Lib: convertor
        // Раскидать строку параметров в словарь
        static public Dictionary<string, string> ParamsToDic(string parameters)
        {
            if (!String.IsNullOrWhiteSpace(parameters))
            {
                string[] pairs = parameters.Split(';').Where(x => x.Contains('=')).ToArray();
                if (pairs.Length > 0)
                {
                    var nameAndValue = pairs.Select(x => x.Split('=').ToArray()).Where(y => !String.IsNullOrWhiteSpace(y[0]));
                    return nameAndValue?.ToDictionary(x => x[0].Trim(), y => y[1]);
                }
            }
            return new Dictionary<string, string>();
        }

        // Раскидать словарь в строку параметров
        static public string DicToParams(Dictionary<string, string> dic)
        {
            return string.Join(";", dic.Select(x => $"{x.Key}={x.Value}"));
        }

        // --- добавлено 19.08.2023
        // Если массив содержит одно значение, то это уже не массив
        static public void OneArrayToValue(ref dynamic value)
        {
            var enumerable = value as System.Collections.IEnumerable;
            if (enumerable == null)
                return;

            ushort iter = 0;
            foreach (var item in value)
                iter++;
            if (iter == 1)
                value = value[0];

            return;
        }

        // dynamic to string[]
        static public string[] DynamicToStringArray(dynamic value)
        {
            string[] values = new string[] { };
            if (value is string)
            {
                values = new string[] { Convert.ToString(value) };
            }
            else
            {
                List<string> ls = new List<string>();

                // Проверка на перечисление
                var enumerable = value as System.Collections.IEnumerable;
                if (enumerable != null)
                {
                    foreach (var item in value)
                    {
                        ls.Add(Convert.ToString(item));
                    }
                }
                else
                {
                    ls.Add(Convert.ToString(value));
                }
                if (ls.Any())
                    values = ls.ToArray();
            }
            return values;
        }
        // string to bool
        static public bool StringToBool(string value)
        {
            value = value.ToLower();
            if (bool.TryParse(value, out bool result1))
                return result1;
            if (int.TryParse(value, out int result2))
                return result2 > 0;
            if (decimal.TryParse(value, out decimal result3))
                return result3 > 0;
            return false;
        }
        #endregion

    }
}
