using System;
using System.Threading.Tasks;

namespace XDash.Framework.Components.Transfer.Contracts
{
    public interface IXDashReceiver
    {
        Task StartReceiving(Func<Models.XDash, Task<bool>> authHandler);
        Task StopReceiving();
    }
}
