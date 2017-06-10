using MVPathway.Builder;
using MVPathway.Builder.Abstractions;
using XDash.Framework.Components.Discovery;
using XDash.Framework.Components.Discovery.Contracts;
using XDash.Framework.Services;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Builder
{
    public static class PathwayBuilderExtensions
    {
        public static IPathwayBuilder UseXDash(this IPathwayBuilder builder)
        {
            var b = builder as PathwayBuilder;

            b.Container.Register<IJsonSerializer, JsonSerializer>();
            b.Container.Register<IBinarySerializer, BinarySerializer>();
            b.Container.Register<ISettingsService, SettingsService>();
            b.Container.Register<IDeviceInfoService, DeviceInfoService>();

            b.Container.Register<IXDashBeacon, XDashBeacon>(false);
            b.Container.Register<IXDashRadar, XDashRadar>(false);

            b.Container.Register<IBeaconService, XDashBeaconService>();
            b.Container.Register<IRadarService, XDashRadarService>();

            return builder;
        }
    }
}
