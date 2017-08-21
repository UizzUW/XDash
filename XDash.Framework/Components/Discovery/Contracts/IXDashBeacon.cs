using System.Threading.Tasks;

namespace XDash.Framework.Components.Discovery.Contracts
{
    public interface IXDashBeacon : IXDashDiscoveryComponent
    {
        Task StartListening();
        Task StopListening();
    }
}
