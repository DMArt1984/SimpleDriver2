using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DML.Log;
using Connector.SGT;
using LogCodeMessage;

namespace Connector.SGT
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

        //List<ITagClient> tags = new List<ITagClient>(); // for parallel 

        // =================================================================================

        //public DeviceReal()
        //{
        //    log?.Invoke(new CodeMessage(0, "Real Device created!"));
        //}

        //~DeviceReal() { } // ничего не делает

        // Лучше:
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

        // ===================================================================================

        //object locker = new object();

    }
}
