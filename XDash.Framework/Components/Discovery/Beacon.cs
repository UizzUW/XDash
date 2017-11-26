using System.Threading.Tasks;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;
using XDash.Framework.Services.Contracts;
using XDash.Framework.Components.Discovery.Contracts;
using XDash.Framework.Models;
using XDash.Framework.Models.Abstractions;
using XDash.Framework.Configuration.Contracts;

namespace XDash.Framework.Components.Discovery
{
    public class Beacon : DiscoveryNode, IBeacon
    {
        private readonly IBsonSerializer _binarySerializer;
        private UdpSocketReceiver _scanRequestReceiver;

        public Beacon(IBsonSerializer binarySerializer,
                      IDeviceInfoService deviceInfoService,
                      IConfigurator configurator,
                      IXDashClient client)
            : base(deviceInfoService, configurator, client)
        {
            _binarySerializer = binarySerializer;
        }

        public async Task StartListening()
        {
            IsEnabled = true;
            _scanRequestReceiver = new UdpSocketReceiver();
            _scanRequestReceiver.MessageReceived += onReceive;
            await _scanRequestReceiver.StartListeningAsync(Options.Discovery.ScanPort);
        }

        public async Task StopListening()
        {
            _scanRequestReceiver.MessageReceived -= onReceive;
            await _scanRequestReceiver.StopListeningAsync();
            _scanRequestReceiver = null;
            IsEnabled = false;
        }

        private async void onReceive(object sender, UdpSocketMessageReceivedEventArgs message)
        {
            var request = _binarySerializer.Deserialize<DasherScanEventArgs>(message.ByteData);
            var response = new DasherScanResponseEventArgs
            {
                RemoteClient = Client as XDashClient,
                Data = SerialData
            };
            var serializedResponse = _binarySerializer.Serialize(response);
            var responder = new UdpSocketClient();
            await responder.SendToAsync(serializedResponse, request.Scanner.Ip, Options.Discovery.ScanFeedbackPort);
        }
    }
}
