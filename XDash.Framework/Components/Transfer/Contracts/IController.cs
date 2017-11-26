using System.Threading.Tasks;
using XDash.Framework.Models;
using XDash.Framework.Models.Abstractions;

namespace XDash.Framework.Components.Transfer.Contracts
{
    public interface IController
    {
        Task<XDashSendResponse> Send(IXDashClient client, Models.XDash dash);
    }
}
