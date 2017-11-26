using MVPathway.MVVM.Abstractions;
using XDash.Framework.Emvy.Builder;
using XDash.Framework.iOS.Services;
using XDash.Framework.Services.Contracts.Platform;

namespace XDash.Framework.Emvy.iOS
{
    public static class XDashIosEmvyExtension
    {
        public static void AddXDashPlatformDependencies(this IDiContainer container)
        {
            container.Register<IPlatformService, IosPlatformService>();
            container.Register<ITimer, IosTimer>(false);
            container.Register<IFilesystem, IosFilesystem>();
            container.ConfigureXDashClientInfo();
        }
    }
}