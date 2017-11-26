using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using XDash.Framework.Services.Contracts;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;
using XDash.Framework.Configuration.Contracts;

namespace XDash.Framework.Services
{
    public class DeviceInfoService : IDeviceInfoService
    {
        private readonly IConfigurator _configurator;
        private readonly ICacheService _cacheService;

        private readonly object _lock = new object();

        public async Task<List<ICommsInterface>> GetInterfaces()
        {
            var interfaces = _cacheService.Interfaces ??
                                              (_cacheService.Interfaces =
                                                  CommsInterface.GetAllInterfacesAsync().Result);

            return interfaces
                .Where(x => x.IsUsable && !x.IsLoopback)
                .OrderBy(x => x.IpAddress)
                .ToList<ICommsInterface>();
        }

        public async Task<ICommsInterface> GetSelectedInterface()
        {
            var options = _configurator.GetConfiguration();
            var ip = options.Device.Ip;
            var interfaces = await GetInterfaces();
            return interfaces.FirstOrDefault(x => x.IpAddress == ip);
        }

        public async Task SetSelectedInterface(ICommsInterface commsInterface)
        {
            if (commsInterface == null)
            {
                return;
            }
            if ((await GetInterfaces()).All(x => x.IpAddress != commsInterface.IpAddress))
            {
                throw new ArgumentException("This interface is not available on this device.");
            }
            var options = _configurator.GetConfiguration();
            options.Device.Ip = commsInterface.IpAddress;
            await _configurator.SaveConfiguration(options);
        }

        public DeviceInfoService(IConfigurator configurator,
                                 ICacheService cacheService)
        {
            _configurator = configurator;
            _cacheService = cacheService;
        }
    }
}
