using System;

namespace XDash.Framework.Models.Abstractions
{
    public interface IXDashClient : IEquatable<IXDashClient>
    {
        string Guid { get; set; }
        string Name { get; set; }
        string Ip { get; set; }
        OperatingSystem OperatingSystem { get; set; }
        string FrameworkVersion { get; set; }
    }
}
