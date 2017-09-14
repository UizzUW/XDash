using System.Collections.Generic;
using Sockets.Plugin;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Services
{
    public class CacheService : ICacheService
    {
        public List<CommsInterface> Interfaces { get; set; }
    }
}
