using Sockets.Plugin.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XDash.Framework.Services.Contracts
{
    public interface IDeviceInfoService
    {
        Task<List<ICommsInterface>> GetInterfaces();
        Task<ICommsInterface> GetSelectedInterface();
        Task SetSelectedInterface(ICommsInterface commsInterface);
    }
}
