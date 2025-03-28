using Connector.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connector.SGT
{
    interface IDeviceFactory
    {
        IDevice CreateDevice(eDriverType driverType, string address);
    }

    class DeviceFactory : IDeviceFactory
    {
        public IDevice CreateDevice(eDriverType driverType, string address)
        {
            switch (driverType)
            {
                case eDriverType.Formula:
                    return new FormulaAdapter();
                case eDriverType.Application:
                    return new AppDevice();
                case eDriverType.ModbusTCPclient:
                    return new ModbusTCPAdapter();
                case eDriverType.ModbusRTUclient:
                    return new ModbusRTUAdapter();
                case eDriverType.AppUDP:
                    return new AppUDP(address);
                case eDriverType.MSSQLclient:
                    return new MSSQLAdapter();
                case eDriverType.OPCUAclient:
                    return new HylasoftOPCUAAdapter();
                default:
                    return new Device();
            }
        }
    }
}
