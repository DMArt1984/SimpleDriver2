using Connector.SGT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connector.Driver
{
    class TestTable : DeviceReal
    {
        public const string driverName = "TestTable";

        // Справка
        // Описание адреса устройства
        public override Dictionary<string, string> HelpSource
            => new Dictionary<string, string> {
                        { "Адрес", "Адресом должна быть пустая строка" }
                    };
        // Описание адреса тега для данного устройства
        public override Dictionary<string, string> HelpTag
            => new Dictionary<string, string> {
                         { "Заголовок", "Описание" },
                    };

        public TestTable()
        {

        }

        ~TestTable()
        {

        }

        // -------------------------------------------------------------------------

    }
}
