using System.Collections.Generic;
using System.Threading.Tasks;
using XDash.Framework.Models.Abstractions;

namespace XDash.Framework.Components.Discovery.Contracts
{
    public delegate void OnDasherScanRequested(DasherScanEventArgs e);

    public interface IScanner : IDiscoveryNode
    {
        Task<List<IXDashClient>> Scan(int timeoutInSeconds = 3);
    }
}
