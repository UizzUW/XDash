using XDash.Framework.Models;

namespace XDash.Framework.Services.Contracts
{
    public interface IPlatformService
    {
        OperatingSystem OS { get; }
    }
}
