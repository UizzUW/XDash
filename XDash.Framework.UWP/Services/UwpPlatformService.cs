using System.Net;
using Windows.ApplicationModel;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using XDash.Framework.Services.Contracts.Platform;

namespace XDash.Framework.UWP.Services
{
    public class UwpPlatformService : IPlatformService
    {
        public Models.OperatingSystem OS => Models.OperatingSystem.Windows;

        public string ConfigurationPath
        {
            get
            {
                var root = Package.Current.InstalledLocation;
                StorageApplicationPermissions.FutureAccessList.AddOrReplace(WebUtility.UrlEncode(root.Path), root);
                return root.Path;
            }
        }

        public void ExitApp()
        {
            Application.Current.Exit();
        }
    }
}
