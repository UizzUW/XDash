using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XDash.Framework.Services.Contracts;
using XDash.Framework.Components.Discovery.Contracts;
using XDash.Framework.Models;
using XDash.Framework.Models.Abstractions;
using XDash.Framework.Configuration.Contracts;
using System.Net.Sockets;
using XDash.Framework.Helpers;

namespace XDash.Framework.Components.Discovery
{
    public class Scanner : DiscoveryNode, IScanner
    {
        private readonly IBsonSerializer _binarySerializer;
        private readonly IXDashClient _client;
        private readonly ITimer _timer;

        private UdpClient _scanRequester;
        private UdpClient _scanResponseReceiver;

        private List<IXDashClient> _scanResults;

        public Scanner(IBsonSerializer binarySerializer,
                       ITimer timer,
                       IConfigurator configurator,
                       IDeviceInfoService deviceInfoService,
                       IXDashClient client,
                       ILogger logger)
            : base(deviceInfoService, configurator, client, logger)
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

            _scanResults = new List<IXDashClient>();

            var scanEvent = new DasherScanEventArgs
            {
                Scanner = _client as XDashClient,
                Data = SerialData
            };
            var serializedScanEvent = _binarySerializer.Serialize(scanEvent);
            var ip = GetAdapterBroadcastIp();

            _scanRequester = new UdpClient();
            await _scanRequester.SendAsync(serializedScanEvent, serializedScanEvent.Length, ip, Options.Discovery.ScanPort);
            _scanRequester.Close();

            _timer.Elapsed += onFinishedScanning;
            _timer.Start(timeoutInSeconds);


            while (IsEnabled)
            {
                _scanResponseReceiver = new UdpClient(Options.Discovery.ScanFeedbackPort);
                var result = await _scanResponseReceiver.ReceiveAsyncEx();
                _scanResponseReceiver.Close();
                if (!result.Success)
                {
                    Logger.LogError(result.Exception.ToString());
                    continue;
                }
                var scanResponse = _binarySerializer.Deserialize<DasherScanResponseEventArgs>(result.Message);
                _scanResults.Add(scanResponse.RemoteClient);
                Logger.LogInfo($"Received {result.Message.Length} bytes from {scanResponse.RemoteClient.Name} ({scanResponse.RemoteClient.Ip}) on port {result.RemoteEndPoint.Port}");
            }
            return _scanResults;
        }

        private async Task onFinishedScanning()
        {
            _scanResponseReceiver.Close();
            IsEnabled = false;
            _timer.Elapsed -= onFinishedScanning;
            _timer.Stop();
            await Task.CompletedTask;
        }
    }
}
