using System.Threading.Tasks;

namespace XDash.Framework.Services.Contracts
{
    public interface IBeaconService
    {
        Task StartBroadcasting();
        void StopBroadcasting();
    }
}
