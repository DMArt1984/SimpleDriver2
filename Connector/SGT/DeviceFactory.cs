using Connector.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connector.SGT
{
    public interface IDeviceFactory
    {
        IRealDevice CreateDevice(eDriverType driverType, string address);
    }

    public class DeviceFactory : IDeviceFactory
    {
        public IRealDevice CreateDevice(eDriverType driverType, string address)
        {
            switch (driverType)
            {
                case eDriverType.Formula:
                    return new Formula();
                case eDriverType.Application:
                    return new AppDevice();
                case eDriverType.ModbusTCPclient:
                    return new ModbusTCPClient();
                case eDriverType.ModbusRTUclient:
                    return new ModbusRTUClient();
                case eDriverType.AppUDP:
                    return new AppUDP(address);
                case eDriverType.MSSQLclient:
                    return new MSSQLclient();
                case eDriverType.OPCUAclient:
                    return new HylasoftOPCUA();
                default:
                    return new Device();
            }
        }
    }
}
