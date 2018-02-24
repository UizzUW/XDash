using System.Threading.Tasks;
using MVPathway.Builder;
using MVPathway.Builder.Abstractions;
using MVPathway.MVVM.Abstractions;
using XDash.Framework.Components.Discovery;
using XDash.Framework.Components.Discovery.Contracts;
using XDash.Framework.Components.Transfer;
using XDash.Framework.Components.Transfer.Contracts;
using XDash.Framework.Models;
using XDash.Framework.Models.Abstractions;
using XDash.Framework.Services;
using XDash.Framework.Services.Contracts;
using XDash.Framework.Services.Contracts.Platform;
using XDash.Framework.Configuration.Contracts;
using XDash.Framework.Configuration;
using XDash.Framework.Emvy.Services;

namespace XDash.Framework.Emvy.Builder
{
    public static class PathwayBuilderExtensions
    {
        public static IPathwayBuilder UseXDash(this IPathwayBuilder builder)
        {
            var b = builder as PathwayBuilder;

            b.Container.Register<ILogger, BridgedLogger>();

            b.Container.Register<ITimer, Timer>(false);
            b.Container.Register<IJsonSerializer, JsonSerializer>();
            b.Container.Register<IBsonSerializer, BsonSerializer>();

            b.Container.Register<ICacheService, CacheService>();

            b.Container.Register<IConfigurator, Configurator>();
            b.Container.Register<IXDashClient, XDashClient>();

            b.Container.Register<IDeviceInfoService, DeviceInfoService>();

            b.Container.Register<IBeacon, Beacon>(false);
            b.Container.Register<IScanner, Scanner>(false);
            b.Container.Register<IController, Controller>(false);
            b.Container.Register<IEndpoint, Endpoint>(false);

            return builder;
        }

        public static IDiContainer ConfigureXDashClientInfo(this IDiContainer container)
        {
            var platformService = container.Resolve<IPlatformService>();
            var configurator = container.Resolve<IConfigurator>();
            var deviceInfoService = container.Resolve<IDeviceInfoService>();
            var client = container.Resolve<IXDashClient>();

            Task.Run(async () => await ConfigHelper.ConfigureOptions(client, configurator, platformService, deviceInfoService));

            return container;
        }
    }
}
