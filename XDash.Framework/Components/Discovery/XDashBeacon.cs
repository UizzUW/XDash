using System;
using System.Threading.Tasks;
using XDash.Framework.Helpers;
using Sockets.Plugin;
using XDash.Framework.Services.Contracts;
using XDash.Framework.Components.Discovery.Contracts;

namespace XDash.Framework.Components.Discovery
{
    public class XDashBeacon : XDashDiscoveryComponent, IXDashBeacon
    {
        private readonly IBinarySerializer _binarySerializer;

        private Timer _broadcastTimer;
        private UdpSocketClient _broadcastClient = new UdpSocketClient();

        private uint _interval = XDashConst.DEFAULT_BEACON_INTERVAL;
        public uint Interval
        {
            get
            {
                return _interval;
            }
            set
            {
                if (IsBroadcasting)
                {
                    throw new InvalidOperationException("Cannot change timer interval while broadcastin.");
                }
                _interval = value;
            }
        }

        private byte[] _serialData;
        public byte[] SerialData
        {
            get
            {
                return _serialData;
            }
            set
            {
                if (IsBroadcasting)
                {
                    throw new InvalidOperationException("Cannot change timer interval while broadcasting.");
                }
                _serialData = value;
            }
        }

        public bool IsBroadcasting { get; private set; }

        public XDashBeacon(IBinarySerializer binarySerializer)
        {
            _binarySerializer = binarySerializer;
        }

        public async Task StartBroadcasting()
        {
            if (IsBroadcasting)
            {
                return;
            }
            _broadcastTimer = new Timer((int)Interval);
            _broadcastTimer.Elapsed += async (sender, e) => await sendMessage();
            IsBroadcasting = true;
            await _broadcastTimer.Start();
        }

        public void StopBroadcasting()
        {
            if (!IsBroadcasting)
            {
                return;
            }
            _broadcastTimer.Stop();
            IsBroadcasting = false;
        }

        private async Task sendMessage()
        {
            var dataTransferTable = new DasherFoundEventArgs
            {
                RemoteDeviceClientInfo = Client,
                Data = SerialData,
                IsBroadcasting = IsBroadcasting
            };
            var serialDtt = _binarySerializer.Serialize(dataTransferTable);

            await _broadcastClient.SendToAsync(serialDtt, AdapterIp, Port);
        }
    }
}
