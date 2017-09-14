using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using XDash.Framework.Services.Contracts;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;

namespace XDash.Framework.Services
{
    public class DeviceInfoService : IDeviceInfoService
    {
        private readonly ISettingsRepository _settingsRepository;
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
            var ip = _settingsRepository.Ip;
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
            _settingsRepository.Ip = commsInterface.IpAddress;
        }

        public DeviceInfoService(ISettingsRepository settingsRepository, ICacheService cacheService)
        {
            _settingsRepository = settingsRepository;
            _cacheService = cacheService;
        }
    }
}
