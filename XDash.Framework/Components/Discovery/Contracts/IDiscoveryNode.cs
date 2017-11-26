using System.Threading.Tasks;

namespace XDash.Framework.Components.Discovery.Contracts
{
    public interface IDiscoveryNode
    {
        Task<string> GetAdapterBroadcastIp();
    }
}
