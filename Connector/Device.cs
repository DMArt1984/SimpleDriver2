using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSimpleIDriver.Connector.SGT;
using DML.Log;

namespace WinSimpleIDriver.Connector
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

    public interface IRealDevice
    {
        CodeMessage CreateClient(string parameters);
        void RemoveClient();
        CodeMessage Connect(string parameters);
        CodeMessage Disconnect();
        bool Connected { get; }
        void Request<T>(List<T> tags) where T : ITagClient;

    }

    interface ITrafficLog
    {
        bool enableLog { get; set; }
        bool supportLog { get; }
    }

    class Device : IRealDevice, ITrafficLog
    {
        public bool Connected => _connected;
        bool _connected = false;

        public bool enableLog { get; set; } = false; // разрешить вести лог
        public virtual bool supportLog { get; } = false;

        List<ITagClient> tags = new List<ITagClient>(); // for parallel 

        // события
        public delegate void HandlerTrafficLog(string message);
        public event HandlerTrafficLog eventTraffic;

        public Device()
        {
            Console.WriteLine("Device created!");
        }

        ~Device()
        {

        }

        // ==================================================================================

        // Создание клиента
        public virtual CodeMessage CreateClient(string parameters = "")
        {
            return new CodeMessage(0,"");
        }

        // Удаление клиента
        public virtual void RemoveClient()
        {

        }

        public void InnerLog(string message)
        {
            // лог сообщений от драйвера
            if (enableLog)
            {
                eventTraffic?.Invoke(message);
                LogHelper2.LogTraffic(message);
            }
        }

        // ------------------------------------------------------------------------

        public virtual CodeMessage Connect(string parameters)
        {
            _connected = true;
            return new CodeMessage(0, "");
        }

        public virtual CodeMessage Disconnect()
        {
            _connected = false;
            return new CodeMessage(0, "");
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
            Parallel.For(0, tags.Count(), ItemParallel);
        }

        void ItemParallel(int index)
        {
            WorkTag(tags[index]);
        }

        protected virtual void WorkTag(ITagClient tag)
        {
            //Console.WriteLine("========> workTag");
            string raddress = Tag.ExpTagAddress(tag.Address, out bool success, tag);
            if (success == false)
            {
                tag.SetResult(new TagResult(null, eTagCode.notReliableA));
                return;
            }

            // Читаем ЕСЛИ
            bool read = (tag.directFull == eDirectFull.Read && tag.Command != eCommand.Wait) || // (направление чтения И нет ожидания) ИЛИ
                (tag.directFull != eDirectFull.Read && tag.Command == eCommand.Wait); // (направление запись И ожидание)

            // Пишем ЕСЛИ не читаем И ( нет команды обновления ИЛИ изменилось число записи )
            bool write = tag.directFull != eDirectFull.Read && (tag.Command != eCommand.Update || (tag.Value != tag.WriteConstValue && tag.Value != tag.WriteTagValue));

            if (tag.Command != eCommand.None)
                tag.Command = eCommand.Wait;

            if (read) // чтение
            {
                tag.SetResult(GetValue(raddress, tag.DataType));
            } else if (write) // запись
            {
                if (tag.directFull == eDirectFull.WriteTagValue) // запись из другого тега
                {
                    var wtId = tag.WriteTagId;
                    var wt = (wtId > 0) ? Tag.items.FirstOrDefault(x => x.Id == wtId) : null;
                    if (wt == null)
                    {
                        tag.SetResult(new TagResult(null, eTagCode.noTagForWrite));
                        return;
                    }
                    if (wt.codeMessage.code != 0 && wt.codeMessage.code != (int)eTagCode.tagOn)
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

        // ====================================================================================

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
