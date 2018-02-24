using System;
using System.IO;
using System.Threading.Tasks;
using XDash.Framework.Components.Transfer.Contracts;
using XDash.Framework.Services.Contracts;
using XDash.Framework.Services.Contracts.Platform;
using XDash.Framework.Configuration;
using XDash.Framework.Configuration.Contracts;
using System.Net.Sockets;
using System.Net;

namespace XDash.Framework.Components.Transfer
{
    public class Endpoint : IEndpoint
    {
        private readonly XDashOptions _options;
        private readonly IBsonSerializer _binarySerializer;
        private readonly IFilesystem _filesystem;

        private TcpListener _listener;
        private Func<Models.XDash, Task<bool>> _authHandler;
        private Func<bool, Task> _finishHandler;

        private NetworkStream _controllerStream;

        private byte[] SerializedTrue { get; }
        private byte[] SerializedFalse { get; }

        public bool IsEnabled { get; private set; }
        public bool IsReceiving { get; private set; }

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
            IsEnabled = true;

            _listener = new TcpListener(IPAddress.Any, _options.Transfer.TransferPort);
            _authHandler = authHandler;
            _finishHandler = finishHandler;
            _listener.Start();

            while (IsEnabled)
            {
                var client = await _listener.AcceptTcpClientAsync();
                _controllerStream = client.GetStream();

                var memoryStream = new MemoryStream();

                await _controllerStream.CopyToAsync(memoryStream);
                var currentDash = _binarySerializer.Deserialize<Models.XDash>(memoryStream.ToArray());
                var result = await _authHandler(currentDash);
                if (!result)
                {
                    await sendFeedback(SerializedFalse);
                    currentDash = null;

                    _controllerStream.Close();
                    client.Close();
                    _controllerStream = null;
                    currentDash = null;

                    continue;
                }

                var destination = _options.Device.DownloadsFolderPath;
                var folderExists = await _filesystem.CheckIfFolderExists(destination);
                if (string.IsNullOrEmpty(destination) || !folderExists)
                {
                    destination = await _filesystem.ChooseFolder();
                }

                if (destination == null)
                {
                    await sendFeedback(SerializedFalse);
                    currentDash = null;

                    _controllerStream.Close();
                    client.Close();
                    _controllerStream = null;
                    currentDash = null;

                    continue;
                }

                await sendFeedback(SerializedTrue);
                for (var counter = 0; counter < currentDash.Files.Length; counter++)
                {
                    await _filesystem.StreamToFile(currentDash.Files[counter].Name, destination, _controllerStream);
                    await sendFeedback(SerializedTrue);
                }

                _controllerStream.Close();
                client.Close();
                _controllerStream = null;
                currentDash = null;

                if (_finishHandler != null) await _finishHandler(true);
            }
        }

        public async Task StopReceiving()
        {
            IsEnabled = false;
            await Task.CompletedTask;
        }

        private async Task sendFeedback(byte[] feedback)
        {
            await _controllerStream.WriteAsync(feedback, 0, feedback.Length);
        }
    }
}
