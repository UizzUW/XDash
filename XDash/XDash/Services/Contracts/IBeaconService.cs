using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDash.Services.Contracts
{
    interface IBeaconService
    {
        Task StartBroadcasting();
        void StopBroadcasting();
    }
}
