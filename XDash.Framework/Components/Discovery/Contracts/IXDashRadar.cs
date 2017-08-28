using System.Threading.Tasks;

namespace XDash.Framework.Components.Discovery.Contracts
{
    public delegate void OnDasherFound(DasherFoundEventArgs e);

    public interface IXDashRadar : IXDashDiscoveryComponent
    {
        Task StartScanning();

        Task StopScanning();

        event OnDasherFound DasherFound;
    }
}
