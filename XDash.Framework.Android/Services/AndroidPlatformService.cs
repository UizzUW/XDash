using Android.App;
using Xamarin.Forms;
using XDash.Framework.Models;
using XDash.Framework.Services.Contracts.Platform;

namespace XDash.Framework.Droid.Services
{
    public class AndroidPlatformService : IPlatformService
    {
        public OperatingSystem OS => OperatingSystem.Android;

        public string ConfigurationPath => throw new System.NotImplementedException();

        public void ExitApp()
        {
            ((Activity)Forms.Context).FinishAffinity();
        }
    }
}