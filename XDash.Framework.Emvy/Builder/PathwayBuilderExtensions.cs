using System;
using System.Linq;
using System.Reflection;
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
using System.Net;

namespace XDash.Framework.Emvy.Builder
{
    public static class PathwayBuilderExtensions
    {
        public static IPathwayBuilder UseXDash(this IPathwayBuilder builder)
        {
            var b = builder as PathwayBuilder;

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
            var clientInfo = container.Resolve<IXDashClient>();

            configurator.Init(platformService.ConfigurationPath);

            var options = configurator.GetConfiguration();

            clientInfo.Guid = getGuid();
            clientInfo.Name = getName();
            clientInfo.Ip = getIp();
            clientInfo.OperatingSystem = platformService.OS;
            clientInfo.FrameworkVersion = new AssemblyName(typeof(XDashClient)
                    .GetTypeInfo()
                    .Assembly.FullName)
                .Version.ToString();

            Task.Run(async () => await configurator.SaveConfiguration(options));

            return container;

            #region Helpers

            string getGuid()
            {
                var guid = options.Device.Guid;
                if (guid != Guid.Empty)
                {
                    return guid.ToString();
                }

                guid = Guid.NewGuid();
                options.Device.Guid = guid;
                return guid.ToString();
            }

            string getName()
            {
                var name = options.Device.Name;
                if (!string.IsNullOrEmpty(name))
                {
                    return name;
                }

                name = $"{platformService.OS} Dasher";
                options.Device.Name = name;
                return name;
            }

            string getIp()
            {
                var ip = options.Device.Ip;
                if (ip != IPAddress.Any.ToString())
                {
                    return ip;
                }

                ip = Task.Run(async () => (await deviceInfoService.GetInterfaces()).FirstOrDefault()).Result?.IpAddress;
                options.Device.Ip = ip;
                return ip;
            }

            #endregion
        }
    }
}
