using XDash.Framework.Models;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.UWP.Services
{
    class WindowsPlatformService : IPlatformService
    {
        public OperatingSystem GetOperatingSystem()
        {
            return OperatingSystem.Windows;
        }
    }
}
