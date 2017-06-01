using XDash.Framework.Models;

namespace XDash.Services.Contracts
{
    public interface IPlatformService
    {
        OperatingSystem GetOperatingSystem();
    }
}
