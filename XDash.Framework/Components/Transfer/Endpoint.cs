using System;
using System.IO;
using System.Threading.Tasks;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;
using XDash.Framework.Components.Transfer.Contracts;
using XDash.Framework.Services.Contracts;
using static XDash.Framework.Helpers.ExtensionMethods;
using XDash.Framework.Services.Contracts.Platform;
using XDash.Framework.Configuration;
using XDash.Framework.Configuration.Contracts;

namespace XDash.Framework.Components.Transfer
{
    public class Endpoint : IEndpoint
    {
        private readonly XDashOptions _options;
        private readonly IBsonSerializer _binarySerializer;
        private readonly IFilesystem _filesystem;

        private TcpSocketListener _listener;
        private Func<Models.XDash, Task<bool>> _authHandler;
        private Func<bool, Task> _finishHandler;

        private Models.XDash _currentDash;
        private ITcpSocketClient _remoteClient;

        private byte[] SerializedTrue { get; }
        private byte[] SerializedFalse { get; }

        public Endpoint(IConfigurator configurator,
                        IBsonSerializer binarySerializer,
                        IFilesystem filesystem)
        {
            _options = configurator.GetConfiguration();
            _binarySerializer = binarySerializer;
            _filesystem = filesystem;

            SerializedTrue = _binarySerializer.Serialize(true);
            SerializedFalse = _binarySerializer.Serialize(false);
        }

        public async Task StartReceiving(Func<Models.XDash, Task<bool>> authHandler,
            Func<bool, Task> finishHandler)
        {
            _listener = new TcpSocketListener();
            _listener.ConnectionReceived += onDashReceived;
            _authHandler = authHandler;
            _finishHandler = finishHandler;
            await _listener.StartListeningAsync(_options.Transfer.TransferPort);
        }

        public async Task StopReceiving()
        {
            _listener.ConnectionReceived -= onDashReceived;
            await _listener.StopListeningAsync();
            _listener = null;
            _authHandler = null;
        }

        private async Task sendFeedback(byte[] feedback)
        {
            var feedbackSender = new TcpSocketClient();
            try
            {
                await feedbackSender.ConnectAsync(_remoteClient.RemoteAddress,
                    _options.Transfer.TransferFeedbackPort);

                await feedbackSender.WriteStream.WriteAsync(feedback, 0, feedback.Length);
                await feedbackSender.DisconnectAsync();
            }
            catch { }
        }

        private string _destination;
        private int _counter;

        private async void onDashReceived(object sender, TcpSocketListenerConnectEventArgs e)
        {
            _remoteClient = e.SocketClient;
            if (_currentDash == null)
            {
                var memoryStream = new MemoryStream();

                await _remoteClient.ReadStream.CopyToAsync(memoryStream);
                _currentDash = _binarySerializer.Deserialize<Models.XDash>(memoryStream.ToArray());
                var result = await _authHandler(_currentDash);
                if (!result)
                {
                    await sendFeedback(SerializedFalse);
                    _currentDash = null;
                    return;
                }

                _destination = _options.Device.DownloadsFolderPath;
                var folderExists = await _filesystem.CheckIfFolderExists(_destination);
                if (string.IsNullOrEmpty(_destination) || !folderExists)
                {
                    await TaskOnUiThread(async () =>
                        _destination = await _filesystem.ChooseFolder());
                }

                if (_destination == null)
                {
                    await sendFeedback(SerializedFalse);
                    _currentDash = null;
                    return;
                }

                await sendFeedback(SerializedTrue);
                _counter = 0;

                return;
            }

            await _filesystem.StreamToFile(_currentDash.Files[_counter].Name, _destination, _remoteClient.ReadStream);
            await sendFeedback(SerializedTrue);

            _counter++;
            if (_counter < _currentDash.Files.Length)
            {
                return;
            }

            await _remoteClient.DisconnectAsync();
            _remoteClient = null;
            _currentDash = null;

            if (_finishHandler != null) await _finishHandler(true);
        }
    }
}
