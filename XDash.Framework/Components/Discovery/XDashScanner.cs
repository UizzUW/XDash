using System.Threading.Tasks;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;
using XDash.Framework.Services.Contracts;
using XDash.Framework.Components.Discovery.Contracts;

namespace XDash.Framework.Components.Discovery
{
    public class XDashScanner : XDashDiscoveryComponent, IXDashScanner
    {
        private readonly IBinarySerializer _binarySerializer;
        private UdpSocketReceiver _broadcastReceiver;

        public XDashScanner(IBinarySerializer binarySerializer)
        {
            _binarySerializer = binarySerializer;
        }

        public async Task StartScanning()
        {
            _broadcastReceiver = new UdpSocketReceiver();
            _broadcastReceiver.MessageReceived += onReceive;
            await _broadcastReceiver.StartListeningAsync(Port);
        }

        public async Task StopScanning()
        {
            _broadcastReceiver.MessageReceived -= onReceive;
            await _broadcastReceiver.StopListeningAsync();
            _broadcastReceiver = null;
        }

        private void onReceive(object sender, UdpSocketMessageReceivedEventArgs message)
        {
            var eargs = _binarySerializer.Deserialize<DasherFoundEventArgs>(message.ByteData);
            DasherFound?.Invoke(eargs);
        }

        public event OnDasherFound DasherFound;
    }
}
