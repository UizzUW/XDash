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

        public async Task<List<ICommsInterface>> GetInterfaces()
        {
            return (await CommsInterface.GetAllInterfacesAsync())
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

        public DeviceInfoService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }
    }
}
