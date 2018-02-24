using MVPathway.MVVM.Abstractions;
using XDash.Framework.Emvy.Builder;
using XDash.Framework.Services.Contracts.Platform;
using XDash.Framework.UWP.Services;

namespace XDash.Framework.Emvy.UWP
{
    public static class XDashUWPEmvyExtension
    {
        public static void AddXDashPlatformDependencies(this IDiContainer container)
        {
            container.Register<IFilesystem, UwpFilesystem>();
            container.Register<IPlatformService, UwpPlatformService>();
            container.ConfigureXDashClientInfo();
        }
    }
}
