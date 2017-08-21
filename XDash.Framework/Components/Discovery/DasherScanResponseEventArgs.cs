using System;
using XDash.Framework.Models.Abstractions;

namespace XDash.Framework.Components.Discovery
{
    public class DasherScanResponseEventArgs : EventArgs
    {
        public IXDashClient RemoteClient { get; set; }
        public byte[] Data { get; set; }
    }
}
