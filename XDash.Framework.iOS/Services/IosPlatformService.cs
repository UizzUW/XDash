using System.Threading;
using XDash.Framework.Models;
using XDash.Framework.Services.Contracts.Platform;

namespace XDash.Framework.iOS.Services
{
    public class IosPlatformService : IPlatformService
    {
        public OperatingSystem OS => OperatingSystem.iOS;

        public string ConfigurationPath => throw new System.NotImplementedException();

        public void ExitApp()
        {
            Thread.CurrentThread.Abort();
        }
    }
}