using MVPathway.MVVM.Abstractions;
using XDash.Framework.Builder;
using XDash.Framework.Services.Contracts;
using XDash.Framework.UWP.Services;

namespace XDash.Framework.UWP
{
    public static class XDashUwpPathwayExtensio
    {
        public static void AddXDashPlatformDependencies(this IDiContainer container)
        {
            container.Register<IPlatformService, UwpPlatformService>();
            container.Register<ITimer, UwpTimer>(false);
            container.ConfigureXDashClientInfo();
        }
    }
}
