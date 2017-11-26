using System;
using System.Threading.Tasks;

namespace XDash.Framework.Components.Transfer.Contracts
{
    public interface IEndpoint
    {
        Task StartReceiving(Func<Models.XDash, Task<bool>> authHandler, Func<bool, Task> finishHandler);
        Task StopReceiving();
    }
}
