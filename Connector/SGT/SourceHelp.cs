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
                    return FormulaAdapter.GetHelpSource();

                case eDriverType.Application:
                    return AppDevice.GetHelpSource();

                case eDriverType.ModbusTCPclient:
                    return ModbusTCPAdapter.GetHelpSource();

                case eDriverType.ModbusRTUclient:
                    return ModbusRTUAdapter.GetHelpSource();

                case eDriverType.AppUDP:
                    return AppUDP.GetHelpSource();

                case eDriverType.MSSQLclient:
                    return MSSQLAdapter.GetHelpSource();

                case eDriverType.OPCUAclient:
                    return HylasoftOPCUAAdapter.GetHelpSource();
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
                    return FormulaAdapter.GetHelpTag();

                case eDriverType.Application:
                    return AppDevice.GetHelpTag();

                case eDriverType.ModbusTCPclient:
                    return ModbusTCPAdapter.GetHelpTag();

                case eDriverType.ModbusRTUclient:
                    return ModbusRTUAdapter.GetHelpTag();

                case eDriverType.AppUDP:
                    return AppUDP.GetHelpTag();

                case eDriverType.MSSQLclient:
                    return MSSQLAdapter.GetHelpTag();

                case eDriverType.OPCUAclient:
                    return HylasoftOPCUAAdapter.GetHelpTag();
            }
            return new Dictionary<string, string>();
        }
    }
}
