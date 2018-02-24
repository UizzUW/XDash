using System;
using System.Linq;
using System.Threading.Tasks;
using XDash.Framework.Services.Contracts;
using XDash.Framework.Configuration.Contracts;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using XDash.Framework.Helpers;

namespace XDash.Framework.Services
{
    public class DeviceInfoService : IDeviceInfoService
    {
        private readonly IConfigurator _configurator;
        private readonly ICacheService _cacheService;

        private readonly object _lock = new object();

        public NetworkInterface[] GetInterfaces()
        {
            var interfaces = _cacheService.Interfaces ??
                                              (_cacheService.Interfaces =
                                                  NetworkInterface.GetAllNetworkInterfaces());

            return interfaces
                .Where(n => (n.NetworkInterfaceType == NetworkInterfaceType.Ethernet || n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                && n.GetIPProperties().UnicastAddresses.FirstOrDefault(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork) != null)
                .ToArray();
        }

        public NetworkInterface GetSelectedInterface()
        {
            var options = _configurator.GetConfiguration();
            var ip = options.Device.Ip;
            var interfaces = GetInterfaces();
            return interfaces.FirstOrDefault(x => x.GetValidIPv4() == ip);
        }

        public async Task SetSelectedInterface(NetworkInterface commsInterface)
        {
            if (commsInterface == null)
            {
                return;
            }
            var commsInterfaceIp = commsInterface.GetValidIPv4();
            if (GetInterfaces().All(x => x.GetValidIPv4() != commsInterfaceIp))
            {
                throw new ArgumentException("This interface is not available on this device.");
            }
            var options = _configurator.GetConfiguration();
            options.Device.Ip = commsInterfaceIp;
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
