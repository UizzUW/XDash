using System;
using XDash.Framework.Models;

namespace XDash.Framework.Configuration
{
    public class XDashOptions
    {
        public XDashDeviceOptions Device { get; set; }

        public XDashDiscoveryOptions Discovery { get; set; }

        public XDashTransferOptions Transfer { get; set; }

        public class XDashDiscoveryOptions
        {
            public int ScanPort { get; set; }

            public int ScanFeedbackPort { get; set; }
        }

        public class XDashTransferOptions
        {
            public int TransferPort { get; set; }

            public int TransferFeedbackPort { get; set; }
        }

        public class XDashDeviceOptions
        {
            public Guid Guid { get; set; }

            public string Name { get; set; }

            public string Ip { get; set; }

            public string DownloadsFolderPath { get; set; }

            public Language Language { get; set; }
        }
    }
}
