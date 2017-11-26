using System.Threading.Tasks;

namespace XDash.Framework.Components.Discovery.Contracts
{
    public interface IBeacon : IDiscoveryNode
    {
        Task StartListening();
        Task StopListening();
    }
}
