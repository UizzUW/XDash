using System.Net.NetworkInformation;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Services
{
    public class CacheService : ICacheService
    {
        public NetworkInterface[] Interfaces { get; set; }
    }
}
