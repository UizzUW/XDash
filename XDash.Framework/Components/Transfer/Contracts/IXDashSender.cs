using System.Threading.Tasks;
using XDash.Framework.Models;
using XDash.Framework.Models.Abstractions;

namespace XDash.Framework.Components.Transfer.Contracts
{
    public interface IXDashSender
    {
        Task<XDashSendResponse> Send(IXDashClient client, Models.XDash dash);
    }
}
