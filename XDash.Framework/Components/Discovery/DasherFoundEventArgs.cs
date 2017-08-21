using System;
using XDash.Framework.Models.Abstractions;

namespace XDash.Framework.Components.Discovery
{
    public class DasherFoundEventArgs : EventArgs
    {
        public IXDashClient RemoteDeviceClient { get; set; }
        public byte[] Data { get; set; }
        public bool IsBroadcasting { get; set; }
    }
}
