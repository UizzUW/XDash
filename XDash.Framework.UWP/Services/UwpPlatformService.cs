using XDash.Framework.Models;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.UWP.Services
{
    public class UwpPlatformService : IPlatformService
    {
        public OperatingSystem OS => OperatingSystem.Windows;
    }
}
