using System;
using XDash.Framework.Models;

namespace XDash.Framework.Components.Discovery
{
    public class DasherFoundEventArgs : EventArgs
    {
        public XDashClient RemoteDeviceClientInfo { get; set; }
        public byte[] Data { get; set; }
        public bool IsBroadcasting { get; set; }
    }
}
