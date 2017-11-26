using System;
using System.Threading.Tasks;
using XDash.Framework.Components.Discovery.Contracts;
using XDash.Framework.Configuration;
using XDash.Framework.Configuration.Contracts;
using XDash.Framework.Helpers;
using XDash.Framework.Models.Abstractions;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Components.Discovery
{
    public abstract class DiscoveryNode : IDiscoveryNode
    {
        public async Task<string> GetAdapterBroadcastIp()
        {
            var selectedInterface = await DeviceInfoService.GetSelectedInterface();
            var ip = selectedInterface.IpAddress;
            return ip.Substring(0, selectedInterface.IpAddress.LastIndexOf(".", StringComparison.Ordinal)) +
                   XDashConst.BROADCAST_SUBNET_SUFFIX;
        }

        private byte[] _serialData;
        public byte[] SerialData
        {
            get => _serialData;
            set
            {
                if (IsEnabled)
                {
                    throw new InvalidOperationException("Cannot change timer interval while broadcasting.");
                }
                _serialData = value;
            }
        }

        public bool IsEnabled { get; protected set; }

        protected XDashOptions Options { get; }
        protected IXDashClient Client { get; }
        protected IDeviceInfoService DeviceInfoService { get; }

        protected DiscoveryNode(IDeviceInfoService deviceInfoService,
                                IConfigurator configurator,
                                IXDashClient client)
        {
            DeviceInfoService = deviceInfoService;
            Options = configurator.GetConfiguration();
            Client = client;
        }
    }
}
