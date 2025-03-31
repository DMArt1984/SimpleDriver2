
using DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connector
{
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

    public class SourceEditor // Редактирование
    {
        public uint Id; // Уникальный идентификатор (0 - нет Id)
        public eDriverType driver; // Тип драйвера
        public string title; // Название драйвера
        public string address; // Строка подключения
        public bool off; // Отключение
        public string description; // Описание
        public bool auto; // Запуск опроса после открытия файла
        public bool reconnect; // Автоматическое переподключение
    }
    
    static public class SourceLib
    {
        static public bool InProject(dynamic output) => JsonControl.IsProp(output, "Sources");
        static public void ParseItemSource(dynamic item, uint forId, out string title, out eDriverType driver, out string connection, out bool off, out string description, out dynamic tags, out bool auto, out bool reopen)
        {
            title = JsonControl.GetString(item, "Title", $"Source #{forId}");
            driver = JsonControl.GetTypeEnum<eDriverType>(item, "Driver", eDriverType.None);
            connection = JsonControl.GetString(item, "Address");
            off = JsonControl.GetBool(item, "Off");
            description = JsonControl.GetString(item, "Desc");
            tags = JsonControl.IsProp(item, "Tags") ? item.Tags : null;
            auto = JsonControl.GetBool(item, "Auto");
            reopen = JsonControl.GetBool(item, "Reconnect");
        }
    }

}
