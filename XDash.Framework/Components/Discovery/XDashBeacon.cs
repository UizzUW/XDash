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
        private readonly IAsyncTimer _timer;

        private UdpSocketClient _broadcastClient = new UdpSocketClient();

        private byte[] _serializedEvent;

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

        public XDashBeacon(IBinarySerializer binarySerializer,
                           IAsyncTimer timer)
        {
            _binarySerializer = binarySerializer;
            _timer = timer;
        }

        public void StartBroadcasting()
        {
            if (IsBroadcasting)
            {
                return;
            }

            var dataTransferTable = new DasherFoundEventArgs
            {
                RemoteDeviceClientInfo = Client,
                Data = SerialData,
                IsBroadcasting = true
            };
            _serializedEvent = _binarySerializer.Serialize(dataTransferTable);
            _timer.Elapsed += sendMessage;
            _timer.Start(Interval);
            IsBroadcasting = true;
        }

        public async Task StopBroadcasting()
        {
            if (!IsBroadcasting)
            {
                return;
            }

            _timer.Elapsed -= sendMessage;
            _timer.Stop();
            IsBroadcasting = false;

            var dataTransferTable = new DasherFoundEventArgs
            {
                RemoteDeviceClientInfo = Client,
                Data = SerialData,
                IsBroadcasting = false
            };
            _serializedEvent = _binarySerializer.Serialize(dataTransferTable);
            await _broadcastClient.SendToAsync(_serializedEvent, AdapterIp, Port);
        }

        private async Task sendMessage()
        {
            await _broadcastClient.SendToAsync(_serializedEvent, AdapterIp, Port);
        }
    }
}
