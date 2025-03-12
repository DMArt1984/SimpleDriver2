using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DML.Log
{
    static public class LoggerConsole
    {
        //
        static public void Log(string message, bool enable = false)
        {
            if (enable)
            {
                Console.WriteLine(message);
            }
        }
    }
}
