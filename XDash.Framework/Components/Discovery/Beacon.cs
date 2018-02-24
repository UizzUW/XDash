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
    public class Beacon : DiscoveryNode, IBeacon
    {
        private readonly IBsonSerializer _binarySerializer;
        private UdpClient _scanRequestReceiver;

        public Beacon(IBsonSerializer binarySerializer,
                      IDeviceInfoService deviceInfoService,
                      IConfigurator configurator,
                      IXDashClient client,
                      ILogger logger)
            : base(deviceInfoService, configurator, client, logger)
        {
            _binarySerializer = binarySerializer;
        }

        public async Task StartListening()
        {
            IsEnabled = true;

            while (IsEnabled)
            {
                _scanRequestReceiver = new UdpClient(Options.Discovery.ScanPort);
                var result = await _scanRequestReceiver.ReceiveAsyncEx();
                _scanRequestReceiver.Close();

                if (!result.Success)
                {
                    Logger.LogError(result.Exception.ToString());
                    return;
                }

                await respondToScanners(result);
            }

        }

        public async Task StopListening()
        {
            IsEnabled = false;
            await Task.CompletedTask;
        }

        private async Task respondToScanners(ExtensionMethods.UdpReceiveResult result)
        {
            var request = _binarySerializer.Deserialize<DasherScanEventArgs>(result.Message);
            var response = new DasherScanResponseEventArgs
            {
                RemoteClient = Client as XDashClient,
                Data = SerialData
            };
            var serializedResponse = _binarySerializer.Serialize(response);
            var responder = new UdpClient();
            var bytesSent = await responder.SendAsync(serializedResponse, serializedResponse.Length, request.Scanner.Ip, Options.Discovery.ScanFeedbackPort);
            responder.Close();
            Logger.LogInfo($"Sent {bytesSent} bytes to {request.Scanner.Name} ({request.Scanner.Ip}) on port {Options.Discovery.ScanFeedbackPort}");
        }
    }
}
