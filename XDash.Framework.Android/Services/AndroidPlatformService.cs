using XDash.Framework.Models;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Droid.Services
{
    public class AndroidPlatformService : IPlatformService
    {
        public OperatingSystem OS => OperatingSystem.Android;

        public void ExitApp()
        {
            throw new System.NotImplementedException();
        }
    }
}