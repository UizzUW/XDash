using System.Threading.Tasks;
using XDash.Framework.Helpers;
using XDash.Framework.Services;
using XDash.Framework.Models;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;

namespace XDash.Framework.Components.Discovery
{
    public class XDashRadar : XDashBaseDiscoveryObject
    {
        #region Instance members

        private readonly UdpSocketReceiver _broadcastReceiver = new UdpSocketReceiver();

        #endregion

        #region Public methods

        public async Task StartScanning()
        {
            _broadcastReceiver.MessageReceived += onReceive;
            await _broadcastReceiver.StartListeningAsync(Port, Interface);
        }

        public async Task StopScanning()
        {
            await _broadcastReceiver.StopListeningAsync();
        }

        #endregion

        #region Private methods

        private async void onReceive(object sender, UdpSocketMessageReceivedEventArgs message)
        {
            var data = DataSerializerService.Deserialize(message.ByteData);

            var eargs = new DasherFoundEventArgs(
                (XDashClient)data[XDashConst.Discovery.REMOTE_DEVICE_CLIENT_INFO],
                (byte[])data[XDashConst.Discovery.SERIAL_DATA],
                (bool)data[XDashConst.Discovery.IS_BROADCASTING]);
            DasherFound?.Invoke(eargs);

            await _broadcastReceiver.StartListeningAsync(Port, Interface);
        }

        #endregion

        #region Events

        public delegate void OnDasherFound(DasherFoundEventArgs e);
        public event OnDasherFound DasherFound;

        #endregion
    }
}
