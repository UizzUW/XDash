using MVPathway.MVVM.Abstractions;
using XDash.Framework.Services.Contracts;
using XDash.Framework.UWP.Services;

namespace XDash.Framework.UWP
{
    public static class XDashUWPPathwayExtensio
    {
        public static void AddXDashPlatformDependencies(this IDiContainer container)
        {
            container.Register<IPlatformService, WindowsPlatformService>();
        }
    }
}
