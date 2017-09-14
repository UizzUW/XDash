using Windows.UI.Xaml;
using XDash.Framework.Models;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.UWP.Services
{
    public class UwpPlatformService : IPlatformService
    {
        public OperatingSystem OS => OperatingSystem.Windows;
        public void ExitApp()
        {
            Application.Current.Exit();
        }
    }
}
