using System.Collections.Generic;
using Sockets.Plugin;

namespace XDash.Framework.Services.Contracts
{
    public interface ICacheService
    {
        List<CommsInterface> Interfaces { get; set; }
    }
}
