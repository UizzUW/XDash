using System;
using XDash.Framework.Models;

namespace XDash.Framework.Components.Discovery
{
    public class DasherScanEventArgs : EventArgs
    {
        public XDashClient Scanner { get; set; }
        public byte[] Data { get; set; }
    }
}
