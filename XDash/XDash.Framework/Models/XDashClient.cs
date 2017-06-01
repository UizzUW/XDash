using System;

namespace XDash.Framework.Models
{
    public struct XDashClient : IEquatable<XDashClient>
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public OperatingSystem OperatingSystem { get; set; }
        public string FrameworkVersion { get; set; }
        public bool Equals(XDashClient client)
        {
            return Guid == client.Guid;
        }
    }
}
