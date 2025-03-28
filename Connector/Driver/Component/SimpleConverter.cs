using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connector.Driver.Component
{
    static class SimpleConverter
    {
        const int version = 1000;
        // Конвертировать строку в строку BOOL (для INSERT)
        static public string StringToBoolString(string value) => StringToBool(value) ? "1" : "0";

        // Конвертировать строку в bool
        static public bool StringToBool(string value)
        {
            return value.Equals("1", StringComparison.OrdinalIgnoreCase) ||
                   value.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                   value.Equals("on", StringComparison.OrdinalIgnoreCase);
        }

        // Конвертировать строку в int
        static public int StringToInt(string value) => int.TryParse(value, out int result) ? result : 0;

    }
}
