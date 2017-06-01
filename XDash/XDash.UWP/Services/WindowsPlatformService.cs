using XDash.Framework.Models;
using XDash.Services.Contracts;

namespace XDash.UWP.Services
{
    class WindowsPlatformService : IPlatformService
    {
        public OperatingSystem GetOperatingSystem()
        {
            return OperatingSystem.Windows;
        }
    }
}
