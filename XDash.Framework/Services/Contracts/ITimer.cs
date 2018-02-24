using System;
using System.Threading.Tasks;

namespace XDash.Framework.Services.Contracts
{
    public interface ITimer
    {
        bool IsEnabled { get; }

        void Start(int interval);

        void Stop();

        event Func<Task> Elapsed;
    }
}
