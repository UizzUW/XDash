using System.Threading.Tasks;

namespace XDash.Framework.Components.Discovery.Contracts
{
    public interface IXDashDiscoveryComponent
    {
        Task<string> GetAdapterBroadcastIp();
    }
}
