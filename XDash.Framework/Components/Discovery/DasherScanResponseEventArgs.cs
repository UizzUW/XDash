using System;
using XDash.Framework.Models;

namespace XDash.Framework.Components.Discovery
{
    public class DasherScanResponseEventArgs : EventArgs
    {
        public XDashClient RemoteClient { get; set; }
        public byte[] Data { get; set; }
    }
}
