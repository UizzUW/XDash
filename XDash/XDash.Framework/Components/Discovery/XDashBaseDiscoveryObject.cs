using Sockets.Plugin.Abstractions;
using System;
using XDash.Framework.Helpers;
using XDash.Framework.Models;

namespace XDash.Framework.Components.Discovery
{
    public abstract class XDashBaseDiscoveryObject
    {
        protected string AdapterIp => _interface.IpAddress
                    .Substring(0, _interface.IpAddress.LastIndexOf("."))
                    + XDashConst.BROADCAST_SUBNET_SUFFIX;

        public XDashClient Client { get; set; }

        private ICommsInterface _interface;
        public ICommsInterface Interface
        {
            get { return _interface; }
            set
            {
                _interface = value ??
                    throw new ArgumentNullException("Network interface for XDashDiscoveryObject cannot be null.");
            }
        }

        private int _port = XDashConst.DEFAULT_DISCOVERY_PORT;
        public int Port
        {
            get { return _port; }
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
