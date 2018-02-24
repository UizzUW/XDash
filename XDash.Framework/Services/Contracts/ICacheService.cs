using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace XDash.Framework.Services.Contracts
{
    public interface ICacheService
    {
        NetworkInterface[] Interfaces { get; set; }
    }
}
