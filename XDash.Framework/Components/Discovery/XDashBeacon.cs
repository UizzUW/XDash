using System.Threading.Tasks;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;
using XDash.Framework.Services.Contracts;
using XDash.Framework.Components.Discovery.Contracts;
using XDash.Framework.Models.Abstractions;

namespace XDash.Framework.Components.Discovery
{
    public class XDashBeacon : XDashDiscoveryComponent, IXDashBeacon
    {
        private readonly IBinarySerializer _binarySerializer;
        private UdpSocketReceiver _scanRequestReceiver;

        public XDashBeacon(IBinarySerializer binarySerializer,
                           ISettingsRepository settingsRepository,
                           IDeviceInfoService deviceInfoService,
                           IXDashClient client)
            : base(settingsRepository, deviceInfoService, client)
        {
            _binarySerializer = binarySerializer;
        }

        public async Task StartListening()
        {
            IsEnabled = true;
            _scanRequestReceiver = new UdpSocketReceiver();
            _scanRequestReceiver.MessageReceived += onReceive;
            await _scanRequestReceiver.StartListeningAsync(SettingsRepository.BeaconScanPort);
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
                RemoteClient = Client,
                Data = SerialData
            };
            var serializedResponse = _binarySerializer.Serialize(response);
            var responder = new UdpSocketClient();
            await responder.SendToAsync(serializedResponse, request.Scanner.Ip, SettingsRepository.ScanResponsePort);
        }
    }
}
