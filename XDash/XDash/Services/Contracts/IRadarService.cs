using System;
using System.Threading.Tasks;
using XDash.Framework.Components.Discovery;

namespace XDash.Services.Contracts
{
    interface IRadarService
    {
        Task StartScanning(Action<DasherFoundEventArgs> onFind);
        Task StopScanning();
    }
}
