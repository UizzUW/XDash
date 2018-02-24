using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace XDash.Framework.Services.Contracts
{
    public interface IDeviceInfoService
    {
        NetworkInterface[] GetInterfaces();
        NetworkInterface GetSelectedInterface();
        Task SetSelectedInterface(NetworkInterface commsInterface);
    }
}
