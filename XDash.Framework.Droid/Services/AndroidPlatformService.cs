using XDash.Framework.Models;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Droid.Services
{
    public class AndroidPlatformService : IPlatformService
    {
        public OperatingSystem GetOperatingSystem()
        {
            return OperatingSystem.Android;
        }
    }
}