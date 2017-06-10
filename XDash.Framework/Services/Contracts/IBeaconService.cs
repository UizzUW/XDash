using System.Threading.Tasks;

namespace XDash.Framework.Services.Contracts
{
    interface IBeaconService
    {
        Task StartBroadcasting();
        void StopBroadcasting();
    }
}
