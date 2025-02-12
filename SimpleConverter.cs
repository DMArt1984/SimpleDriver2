using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSimpleIDriver
{
    static class SimpleConverter
    {

        // Конвертировать строку в строку BOOL (для INSERT)
        static public string StringToBoolString(string value)
        {
            return StringToBool(value) ? "1" : "0";
        }
        static public bool StringToBool(string value)
        {
            string val = value.ToLower();
            return (val == "1" || val == "true" || val == "on") ? true : false;
        }
        static public int StringToInt(string value)
        {
            bool check = int.TryParse(value, out int result);
            return (check) ? result : 0;
        }
        
    }
}
