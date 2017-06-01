using System;
using System.Threading.Tasks;
using XDash.Framework.Helpers;
using System.Collections.Generic;
using XDash.Framework.Services;
using Sockets.Plugin;

namespace XDash.Framework.Components.Discovery
{
  public class XDashBeacon : XDashBaseDiscoveryObject
    {
        #region Instance members
    
        private Timer _broadcastTimer;
        private UdpSocketClient _broadcastClient = new UdpSocketClient();

        #endregion

        #region Properties

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

        #endregion

        #region Public methods

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

        #endregion

        #region Private methods

        private async Task sendMessage()
        {
            var dataTransferTable = new Dictionary<string, object>()
            {
                { XDashConst.Discovery.REMOTE_DEVICE_CLIENT_INFO, Client },
                { XDashConst.Discovery.SERIAL_DATA, SerialData },
                { XDashConst.Discovery.IS_BROADCASTING, IsBroadcasting }
            };
            var serialDtt = DataSerializerService.Serialize(dataTransferTable);

            await _broadcastClient.SendToAsync(serialDtt, AdapterIp, Port);
        }

        #endregion
    }
}
