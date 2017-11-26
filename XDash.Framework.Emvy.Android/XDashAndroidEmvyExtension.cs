using MVPathway.MVVM.Abstractions;
using XDash.Framework.Droid.Services;
using XDash.Framework.Emvy.Builder;
using XDash.Framework.Services.Contracts.Platform;

namespace XDash.Framework.Emvy.Droid
{
    public static class XDashAndroidEmvyExtension
    {
        public static void AddXDashPlatformDependencies(this IDiContainer container)
        {
            container.Register<IPlatformService, AndroidPlatformService>();
            container.Register<ITimer, AndroidTimer>(false);
            container.Register<IFilesystem, AndroidFilesystem>();
            container.ConfigureXDashClientInfo();
        }
    }
}