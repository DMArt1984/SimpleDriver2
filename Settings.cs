using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSimpleIDriver
{
    static class Settings
    {
        static public bool tray = false; // Сворачивать ли окно при запуске
        static public bool logTable = false; // Писать лог в таблицу формы
        static public string settingsFileName = "Settings.ini"; // файл настроек
        static public string x = "?";

        static public void Set(INIfile data)
        {
            // APPLICATION
            tray = SimpleConverter.StringToBool(data.IniReadValue("APPLICATION", "Tray"));
            logTable = SimpleConverter.StringToBool(data.IniReadValue("APPLICATION", "TableLog"));

            x = data.IniReadValue("APPLICATION", "Value");

        }



    }
}
