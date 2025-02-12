using DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DML
{
    static class Settings
    {
        static public bool tray = false; // Сворачивать ли окно при запуске
        static public bool logTable = false; // Писать лог в таблицу формы
        static public string settingsFileName = "Settings.ini"; // файл настроек

        static public string value = "?";

        static public void Set(INIfile data)
        {
            // APPLICATION
            tray = SimpleConverter.StringToBool(data.IniReadValue("APPLICATION", "Tray"));
            logTable = SimpleConverter.StringToBool(data.IniReadValue("APPLICATION", "TableLog"));

            // test
            value = data.IniReadValue("APPLICATION", "Value");

        }



    }
}
