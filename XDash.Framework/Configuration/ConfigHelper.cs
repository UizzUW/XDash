using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using XDash.Framework.Configuration.Contracts;
using XDash.Framework.Helpers;
using XDash.Framework.Models;
using XDash.Framework.Models.Abstractions;
using XDash.Framework.Services.Contracts;
using XDash.Framework.Services.Contracts.Platform;

namespace XDash.Framework.Configuration
{
    public static class ConfigHelper
    {
        public static async Task ConfigureOptions(IXDashClient client,
                                                  IConfigurator configurator,
                                                  IPlatformService platformService,
                                                  IDeviceInfoService deviceInfoService)
        {

            configurator.Init(platformService.ConfigurationPath);

            var options = configurator.GetConfiguration();

            client.Guid = getGuid();
            client.Name = getName();
            client.Ip = await getIp();
            client.OperatingSystem = platformService.OS;
            client.FrameworkVersion = new AssemblyName(typeof(XDashClient)
                    .GetTypeInfo()
                    .Assembly.FullName)
                .Version.ToString();

            await configurator.SaveConfiguration(options);

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

            async Task<string> getIp()
            {
                var ip = options.Device.Ip;
                if (!string.IsNullOrWhiteSpace(ip) && ip != IPAddress.Any.ToString())
                {
                    return ip;
                }

                ip = deviceInfoService.GetInterfaces().FirstOrDefault()?.GetValidIPv4();
                options.Device.Ip = ip;
                return ip;
            }

            #endregion
        }
    }
}
