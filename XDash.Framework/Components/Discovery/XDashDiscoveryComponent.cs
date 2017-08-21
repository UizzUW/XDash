using Sockets.Plugin.Abstractions;
using System;
using XDash.Framework.Components.Discovery.Contracts;
using XDash.Framework.Helpers;
using XDash.Framework.Models.Abstractions;

namespace XDash.Framework.Components.Discovery
{
    public abstract class XDashDiscoveryComponent : IXDashDiscoveryComponent
    {
        protected string AdapterIp => _interface.IpAddress.Replace(
            _interface.IpAddress.Remove(0, _interface.IpAddress.LastIndexOf(".", StringComparison.Ordinal)),
            XDashConst.BROADCAST_SUBNET_SUFFIX);

        public IXDashClient Client { get; set; }

        private ICommsInterface _interface;
        public ICommsInterface Interface
        {
            get => _interface;
            set => _interface = value ??
                                throw new ArgumentNullException("Network interface for XDashDiscoveryObject cannot be null.");
        }

        private int _port = XDashConst.DEFAULT_DISCOVERY_PORT;
        public int Port
        {
            get => _port;
            set
            {
                if (value <= XDashConst.MINIMUM_PORT_VALUE)
                {
                    throw new ArgumentException($"Invalid port, please use a port greater than {XDashConst.MINIMUM_PORT_VALUE}.");
                }
                _port = value;
            }
        }
    }
}
