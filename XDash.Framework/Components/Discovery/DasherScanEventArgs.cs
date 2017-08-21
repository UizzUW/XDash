using System;
using XDash.Framework.Models.Abstractions;

namespace XDash.Framework.Components.Discovery
{
    public class DasherScanEventArgs : EventArgs
    {
        public IXDashClient Scanner { get; set; }
        public byte[] Data { get; set; }
    }
}
