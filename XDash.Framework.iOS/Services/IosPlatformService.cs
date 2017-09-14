using System.Threading;
using XDash.Framework.Models;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.iOS.Services
{
    public class IosPlatformService : IPlatformService
    {
        public OperatingSystem OS => OperatingSystem.iOS;
        public void ExitApp()
        {
            Thread.CurrentThread.Abort();
        }
    }
}