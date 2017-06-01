using XDash.Framework.Models;
using XDash.Services.Contracts;

namespace XDash.Droid.Services
{
    public class AndroidPlatformService : IPlatformService
    {
        public OperatingSystem GetOperatingSystem()
        {
            return OperatingSystem.Android;
        }
    }
}