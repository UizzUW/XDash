using System;
using System.Threading.Tasks;
using XDash.Framework.Services.Contracts.Platform;

namespace XDash.Framework.iOS.Services
{
    public class IosTimer : ITimer
    {
        public bool IsEnabled => throw new NotImplementedException();

        public event Func<Task> Elapsed;

        public void Start(int interval)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
