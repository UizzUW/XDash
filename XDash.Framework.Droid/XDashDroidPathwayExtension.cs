using MVPathway.MVVM.Abstractions;
using XDash.Framework.Droid.Services;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Droid
{
    public static class XDashDroidPathwayExtension
    {
        public static void AddXDashPlatformDependencies(this IDiContainer container)
        {
            container.Register<IPlatformService, AndroidPlatformService>();
        }
    }
}