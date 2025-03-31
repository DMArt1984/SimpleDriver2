using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Connector
{
    public class ReconnectTimer : IDisposable
    {
        private Timer _timer;
        private readonly object _lock = new object();

        public void Start(int delayMs, Action callback)
        {
            Stop();

            _timer = new Timer(_ =>
            {
                callback?.Invoke();
            }, null, delayMs, Timeout.Infinite);
        }

        public void Stop()
        {
            lock (_lock)
            {
                var local = Interlocked.Exchange(ref _timer, null);
                local?.Dispose();
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
