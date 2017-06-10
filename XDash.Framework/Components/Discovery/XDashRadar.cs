using System.Threading.Tasks;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;
using XDash.Framework.Services.Contracts;
using XDash.Framework.Components.Discovery.Contracts;

namespace XDash.Framework.Components.Discovery
{
    public class XDashRadar : XDashDiscoveryComponent, IXDashRadar
    {
        private readonly IBinarySerializer _binarySerializer;
        private readonly UdpSocketReceiver _broadcastReceiver = new UdpSocketReceiver();

        public XDashRadar(IBinarySerializer binarySerializer)
        {
            _binarySerializer = binarySerializer;
        }

        public async Task StartScanning()
        {
            _broadcastReceiver.MessageReceived += onReceive;
            await _broadcastReceiver.StartListeningAsync(Port, Interface);
        }

        public async Task StopScanning()
        {
            await _broadcastReceiver.StopListeningAsync();
        }

        private async void onReceive(object sender, UdpSocketMessageReceivedEventArgs message)
        {
            var eargs = _binarySerializer.Deserialize<DasherFoundEventArgs>(message.ByteData);
            DasherFound?.Invoke(eargs);

            await _broadcastReceiver.StartListeningAsync(Port, Interface);
        }

        public event OnDasherFound DasherFound;
    }
}
