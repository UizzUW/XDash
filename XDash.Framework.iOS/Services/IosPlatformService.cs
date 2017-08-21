using XDash.Framework.Models;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.iOS.Services
{
    public class IosPlatformService : IPlatformService
    {
        public OperatingSystem OS => OperatingSystem.iOS;
    }
}