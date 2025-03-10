using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSimpleIDriver.Connector
{
    public class Include
    {


        

    }

    #region EDITOR

    public class IncludeEditor // Редактирование
    {
        public ushort Id { get; set; }
        public string fileName { get; set; } // файл для включения в основной код
        public string prefix { get; set; } // префикс к именам
        //public Dictionary<string, string> changes { get; set; } // подстановки
    }

    public class IncludeChildEditor // Редактирование дочернего элемента
    {
        public ushort Id { get; set; }
        public string prefix { get; set; } // префикс к именам
        public string valueFrom { get; set; } // что меняем
        public string valueTo { get; set; } // на что меняем

    }

    #endregion

}
