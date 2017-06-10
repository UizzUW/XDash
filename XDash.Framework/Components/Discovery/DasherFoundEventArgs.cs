using System;
using XDash.Framework.Models;

namespace XDash.Framework.Components.Discovery
{
    public class DasherFoundEventArgs : EventArgs
    {
        public XDashClient RemoteDeviceClientInfo { get; private set; }
        public byte[] Data { get; private set; }
        public bool IsBroadcasting { get; private set; }
        public DasherFoundEventArgs(XDashClient info, byte[] data, bool isBroadcasting)
        {
            RemoteDeviceClientInfo = info;
            Data = data;
            IsBroadcasting = isBroadcasting;
        }
    }
}
