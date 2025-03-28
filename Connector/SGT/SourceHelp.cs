using Connector.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connector.SGT
{

    static class SourceHelp
    {
        // Справки
        static public Dictionary<string, string> HelpDicSource(eDriverType driverType)
        {
            switch (driverType)
            {
                case eDriverType.None:
                    break;

                case eDriverType.Formula:
                    return FormulaAdapter.HelpSource;

                case eDriverType.Application:
                    return AppDevice.HelpSource;

                case eDriverType.ModbusTCPclient:
                    return ModbusTCPAdapter.HelpSource;

                case eDriverType.ModbusRTUclient:
                    return ModbusRTUAdapter.HelpSource;

                case eDriverType.AppUDP:
                    return AppUDP.HelpSource;

                case eDriverType.MSSQLclient:
                    return MSSQLAdapter.HelpSource;

                case eDriverType.OPCUAclient:
                    return HylasoftOPCUAAdapter.HelpSource;
            }
            return new Dictionary<string, string>();
        }
        static public Dictionary<string, string> HelpDicTag(eDriverType driverType)
        {
            switch (driverType)
            {
                case eDriverType.None:
                    break;

                case eDriverType.Formula:
                    return FormulaAdapter.HelpTag;

                case eDriverType.Application:
                    return AppDevice.HelpTag;

                case eDriverType.ModbusTCPclient:
                    return ModbusTCPAdapter.HelpTag;

                case eDriverType.ModbusRTUclient:
                    return ModbusRTUAdapter.HelpTag;

                case eDriverType.AppUDP:
                    return AppUDP.HelpTag;

                case eDriverType.MSSQLclient:
                    return MSSQLAdapter.HelpTag;

                case eDriverType.OPCUAclient:
                    return HylasoftOPCUAAdapter.HelpTag;
            }
            return new Dictionary<string, string>();
        }
    }
}
