using Android.App;
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
            // TODO : capture context without forms
            //((Activity)Forms.Context).FinishAffinity();
        }
    }
}