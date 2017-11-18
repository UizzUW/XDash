using XDash.Framework.Models;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.UWP.Services
{
    public class WindowsPlatformService : IPlatformService
    {
        public OperatingSystem OS => OperatingSystem.Windows;

        public void ExitApp()
        {
            throw new System.NotImplementedException();
        }
    }
}
