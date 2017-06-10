using System.Threading.Tasks;

namespace XDash.Framework.Components.Discovery.Contracts
{
    public interface IXDashBeacon : IXDashDiscoveryComponent
    {
        uint Interval { get; set; }
        byte[] SerialData { get; set; }
        bool IsBroadcasting { get; }

        Task StartBroadcasting();
        void StopBroadcasting();
    }
}
