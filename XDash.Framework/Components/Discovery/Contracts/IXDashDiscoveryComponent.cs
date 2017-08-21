using Sockets.Plugin.Abstractions;
using XDash.Framework.Models.Abstractions;

namespace XDash.Framework.Components.Discovery.Contracts
{
    public interface IXDashDiscoveryComponent
    {
        IXDashClient Client { get; set; }
        ICommsInterface Interface { get; set; }
        int Port { get; set; }
    }
}
