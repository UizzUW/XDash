using MVPathway.MVVM.Abstractions;
using XDash.Framework.Builder;
using XDash.Framework.Droid.Services;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Droid
{
    public static class XDashDroidPathwayExtension
    {
        public static void AddXDashPlatformDependencies(this IDiContainer container)
        {
            container.Register<IPlatformService, DroidPlatformService>();
            container.Register<ITimer, DroidTimer>(false);
            container.Register<IXDashFilesystem, DroidXDashFilesystem>();
            container.ConfigureXDashClientInfo();
        }
    }
}