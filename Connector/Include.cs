using DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSimpleIDriver.Connector
{
    public class Include
    {

        // Получение параметров включения
        static public void ParseItemInclude(dynamic item, out string fileName, out string prefix, out Dictionary<string, string> changes)
        {
            fileName = JsonControl.GetString(item, "Filename");
            prefix = JsonControl.GetString(item, "Prefix", "");
            changes = new Dictionary<string, string>();

            // подстановки
            if (JsonControl.IsProp(item, "Changes"))
            {
                IDictionary<string, object> propertyValues = (IDictionary<string, object>)item.Changes;
                changes = propertyValues.ToDictionary(x => x.Key, y => Convert.ToString(y.Value));
            }

        }

        static public bool InProject(dynamic output) => JsonControl.IsProp(output, "Includes") && !JsonControl.IsProp(output, "Contents");

    }

    #region EDITOR

    public class IncludeEditor // Редактирование
    {
        public uint Id { get; set; }
        public string fileName { get; set; } // файл для включения в основной код
        public string prefix { get; set; } // префикс к именам
        //public Dictionary<string, string> changes { get; set; } // подстановки
    }

    public class IncludeChildEditor // Редактирование дочернего элемента
    {
        public uint Id { get; set; }
        public string prefix { get; set; } // префикс к именам
        public string valueFrom { get; set; } // что меняем
        public string valueTo { get; set; } // на что меняем

    }

    #endregion

}
