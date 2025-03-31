using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogCodeMessage;

namespace Connector
{
    public interface IRealDevice
    {
        CodeMessage Connect(string parameters);
        CodeMessage Disconnect();
        bool Connected { get; }
    }

    class DeviceReal : Device, IRealDevice
    {
        const int version = 1000; // Версия

        public bool Connected => _connected;
        bool _connected = false;

        public override void Dispose()
        {
            Disconnect();
            RemoveClient();
        }

        // ==================================================================================

        public virtual CodeMessage Connect(string parameters)
        {
            _connected = true;
            return new CodeMessage();
        }

        public virtual CodeMessage Disconnect()
        {
            _connected = false;
            return new CodeMessage();
        }

    }
}
