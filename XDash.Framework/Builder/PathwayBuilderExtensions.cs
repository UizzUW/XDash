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
using XDash.Framework.Helpers;
using XDash.Framework.Models;
using XDash.Framework.Models.Abstractions;
using XDash.Framework.Services;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Builder
{
    public static class PathwayBuilderExtensions
    {
        public static IPathwayBuilder UseXDash(this IPathwayBuilder builder)
        {
            var b = builder as PathwayBuilder;

            b.UseSettings<ISettingsRepository, SettingsRepository>();

            b.Container.Register<IXDashClient, XDashClient>();

            b.Container.Register<IJsonSerializer, JsonSerializer>();
            b.Container.Register<IBinarySerializer, BinarySerializer>();
            b.Container.Register<IDeviceInfoService, DeviceInfoService>();

            b.Container.Register<IXDashBeacon, XDashBeacon>(false);
            b.Container.Register<IXDashScanner, XDashScanner>(false);
            b.Container.Register<IXDashSender, XDashSender>(false);
            b.Container.Register<IXDashReceiver, XDashReceiver>(false);

            return builder;
        }

        public static IDiContainer ConfigureXDashClientInfo(this IDiContainer container)
        {
            var platformService = container.Resolve<IPlatformService>();
            var settingsRepository = container.Resolve<ISettingsRepository>();
            var deviceInfoService = container.Resolve<IDeviceInfoService>();

            if (settingsRepository.BeaconScanPort == 0)
            {
                settingsRepository.BeaconScanPort = XDashConst.DEFAULT_BEACON_SCAN_PORT;
            }

            if (settingsRepository.ScanResponsePort == 0)
            {
                settingsRepository.ScanResponsePort = XDashConst.DEFAULT_SCAN_RESPONSE_PORT;
            }

            if (settingsRepository.TransferPort == 0)
            {
                settingsRepository.TransferPort = XDashConst.DEFAULT_TRANSFER_PORT;
            }

            if (settingsRepository.TransferFeedbackPort == 0)
            {
                settingsRepository.TransferFeedbackPort = XDashConst.DEFAULT_TRANSFER_FEEDBACK_PORT;
            }

            var clientInfo = container.Resolve<IXDashClient>();

            clientInfo.Guid = getGuid();
            clientInfo.Name = getName();
            clientInfo.Ip = getIp();
            clientInfo.OperatingSystem = platformService.OS;
            clientInfo.FrameworkVersion = new AssemblyName(typeof(XDashClient)
                    .GetTypeInfo()
                    .Assembly.FullName)
                .Version.ToString();

            string getGuid()
            {
                var guid = settingsRepository.Guid;
                if (guid != Guid.Empty)
                {
                    return guid.ToString();
                }

                guid = Guid.NewGuid();
                settingsRepository.Guid = guid;
                return guid.ToString();
            }

            string getName()
            {
                var name = settingsRepository.Name;
                if (name != null)
                {
                    return name;
                }

                name = $"{platformService.OS} Dasher";
                settingsRepository.Name = name;
                return name;
            }

            string getIp()
            {
                var ip = settingsRepository.Ip;
                if (ip != null)
                {
                    return ip;
                }

                ip = Task.Run(async () => (await deviceInfoService.GetInterfaces()).FirstOrDefault()).Result?.IpAddress;
                settingsRepository.Ip = ip;
                return ip;
            }

            return container;
        }
    }
}
