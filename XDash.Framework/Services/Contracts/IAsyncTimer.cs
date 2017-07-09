using System;
using System.Threading.Tasks;

namespace XDash.Framework.Services.Contracts
{
    public interface IAsyncTimer
    {
        bool IsEnabled { get; }

        void Start(uint interval);

        void Stop();

        event Func<Task> Elapsed;
    }
}
