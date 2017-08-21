using XDash.Framework.Models.Abstractions;

namespace XDash.Framework.Models
{
    public class XDashClient : IXDashClient
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public OperatingSystem OperatingSystem { get; set; }
        public string FrameworkVersion { get; set; }
        public bool Equals(IXDashClient client)
        {
            return Guid == client.Guid;
        }
    }
}
