using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;
using XDash.Framework.Components.Transfer.Contracts;
using XDash.Framework.Models;
using XDash.Framework.Services.Contracts;
using static XDash.Framework.Helpers.ExtensionMethods;

namespace XDash.Framework.Components.Transfer
{
    public class XDashReceiver : IXDashReceiver
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly IBinarySerializer _binarySerializer;
        private readonly IXDashFilesystem _filesystem;

        private TcpSocketListener _listener;
        private Func<Models.XDash, Task<bool>> _authHandler;

        private Models.XDash _currentDash;
        private ITcpSocketClient _remoteClient;

        private byte[] SerializedTrue { get; }
        private byte[] SerializedFalse { get; }


        public XDashReceiver(ISettingsRepository settingsRepository,
                             IBinarySerializer binarySerializer,
                             IXDashFilesystem filesystem)
        {
            _settingsRepository = settingsRepository;
            _binarySerializer = binarySerializer;
            _filesystem = filesystem;

            SerializedTrue = _binarySerializer.Serialize(true);
            SerializedFalse = _binarySerializer.Serialize(false);
        }

        public async Task StartReceiving(Func<Models.XDash, Task<bool>> authHandler)
        {
            _listener = new TcpSocketListener();
            _listener.ConnectionReceived += onDashReceived;
            _authHandler = authHandler;
            await _listener.StartListeningAsync(_settingsRepository.TransferPort);
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
                    _settingsRepository.TransferFeedbackPort);

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

                _destination = _settingsRepository.DownloadsFolderPath;
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

            await _filesystem.StreamToFile(_currentDash.Files[_counter].Name, _destination, e.SocketClient.ReadStream);
            await sendFeedback(SerializedTrue);

            _counter++;
            if (_counter < _currentDash.Files.Length)
            {
                return;
            }

            await _remoteClient.DisconnectAsync();
            _remoteClient = null;
            _currentDash = null;
        }
    }
}
