using Sockets.Plugin.Abstractions;
using XDash.Framework.Models;

namespace XDash.Framework.Components.Discovery.Contracts
{
    public interface IXDashDiscoveryComponent
    {
        XDashClient Client { get; set; }
        ICommsInterface Interface { get; set; }
        int Port { get; set; }
    }
}
