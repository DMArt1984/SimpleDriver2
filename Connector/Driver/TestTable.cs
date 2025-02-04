using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSimpleIDriver.Connector.Driver
{
    class TestTable : Device
    {
        public const string driverName = "TestTable";

        // Справка
        public static Dictionary<string, string> GetHelpSource()
        {
            return new Dictionary<string, string> {
                        { "Адрес", "Адресом должна быть пустая строка" }
                    };
        } // Описание адреса устройства

        public static Dictionary<string, string> GetHelpTag()
        {
            return new Dictionary<string, string> {
                        { "Заголовок", "Описание" },
                    };
        } // Описание адреса тега для данного устройства

        public TestTable()
        {

        }

        ~TestTable()
        {

        }

        // -------------------------------------------------------------------------

    }
}
