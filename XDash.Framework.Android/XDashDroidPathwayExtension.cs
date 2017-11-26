using MVPathway.MVVM.Abstractions;
using XDash.Framework.Builder;
using XDash.Framework.Droid.Services;
using XDash.Framework.Services.Contracts.Platform;

namespace XDash.Framework.Droid
{
    public static class XDashDroidPathwayExtension
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