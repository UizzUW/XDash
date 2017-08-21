using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;
using XDash.Framework.Services.Contracts;
using XDash.Framework.Components.Discovery.Contracts;
using XDash.Framework.Models.Abstractions;

namespace XDash.Framework.Components.Discovery
{
    public class XDashScanner : XDashDiscoveryComponent, IXDashScanner
    {
        private readonly IBinarySerializer _binarySerializer;
        private readonly IXDashClient _client;
        private readonly ITimer _timer;

        private UdpSocketClient _scanRequester;
        private UdpSocketReceiver _scanResponseReceiver;

        private TaskCompletionSource<List<IXDashClient>> _tcs;
        private List<IXDashClient> _scanResults;

        public XDashScanner(IBinarySerializer binarySerializer,
                            ITimer timer,
                            ISettingsRepository settingsRepository,
                            IDeviceInfoService deviceInfoService,
                            IXDashClient client)
            : base(settingsRepository, deviceInfoService, client)
        {
            _binarySerializer = binarySerializer;
            _client = client;
            _timer = timer;
        }

        public async Task<List<IXDashClient>> Scan(int timeoutInSeconds = 3)
        {
            if (IsEnabled)
            {
                throw new InvalidOperationException("Another scan already in progress.");
            }

            IsEnabled = true;

            _tcs = new TaskCompletionSource<List<IXDashClient>>();
            _scanResults = new List<IXDashClient>();

            _scanResponseReceiver = new UdpSocketReceiver();
            _scanResponseReceiver.MessageReceived += onDasherFound;
            await _scanResponseReceiver.StartListeningAsync(SettingsRepository.ScanResponsePort);

            var scanEvent = new DasherScanEventArgs
            {
                Scanner = _client,
                Data = SerialData
            };
            var serializedScanEvent = _binarySerializer.Serialize(scanEvent);
            _scanRequester = new UdpSocketClient();
            var ip = await GetAdapterBroadcastIp();
            await _scanRequester.SendToAsync(serializedScanEvent, ip, SettingsRepository.BeaconScanPort);

            _timer.Elapsed += onFinishedScanning;
            _timer.Start(timeoutInSeconds);

            return await _tcs.Task;
        }

        private void onDasherFound(object sender, UdpSocketMessageReceivedEventArgs message)
        {
            var scanResponse = _binarySerializer.Deserialize<DasherScanResponseEventArgs>(message.ByteData);
            _scanResults.Add(scanResponse.RemoteClient);
        }

        private async Task onFinishedScanning()
        {
            _timer.Elapsed -= onFinishedScanning;
            _timer.Stop();
            await _scanResponseReceiver.StopListeningAsync();
            _tcs.SetResult(_scanResults);
            _scanRequester = null;
            _scanResponseReceiver = null;
            IsEnabled = false;
        }
    }
}
