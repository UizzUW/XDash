using Android.App;
using Xamarin.Forms;
using XDash.Framework.Models;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Droid.Services
{
    public class DroidPlatformService : IPlatformService
    {
        public OperatingSystem OS => OperatingSystem.Android;
        public void ExitApp()
        {
            ((Activity)Forms.Context).FinishAffinity();
        }
    }
}