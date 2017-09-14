using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MVPathway.Logging.Abstractions;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;
using XDash.Framework.Components.Transfer.Contracts;
using XDash.Framework.Models;
using XDash.Framework.Models.Abstractions;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Components.Transfer
{
    public class XDashSender : IXDashSender
    {
        private readonly IBinarySerializer _binarySerializer;
        private readonly ISettingsRepository _settingsRepository;
        private readonly IXDashFilesystem _filesystem;
        private readonly ITimer _timer;
        private readonly ILogger _logger;

        private TaskCompletionSource<bool> _handshakeTcs;

        public XDashSender(IBinarySerializer binarySerializer,
                           ISettingsRepository settingsRepository,
                           IXDashFilesystem filesystem,
                           ITimer timer,
                           ILogger logger)
        {
            _binarySerializer = binarySerializer;
            _settingsRepository = settingsRepository;
            _filesystem = filesystem;
            _timer = timer;
            _logger = logger;
        }

        public async Task<XDashSendResponse> Send(IXDashClient client, Models.XDash dash)
        {
            var sender = new TcpSocketClient();
            await sender.ConnectAsync(client.Ip, _settingsRepository.TransferPort);
            //try
            //{
            //    await connectTask;
            //}
            //catch (TaskCanceledException)
            //{
            //    return new XDashSendResponse
            //    {
            //        Status = XDashSendResponseStatus.TimeoutDuringConnect
            //    };
            //}
            //catch (Exception ex) when (!(ex is TaskCanceledException))
            //{
            //    return new XDashSendResponse
            //    {
            //        Status = XDashSendResponseStatus.ErrorDuringConnect,
            //        Message = ex.ToString()
            //    };
            //}

            var serializedData = _binarySerializer.Serialize(dash);
            await sender.WriteStream.WriteAsync(serializedData, 0, serializedData.Length, new CancellationTokenSource(3000).Token);
            await sender.DisconnectAsync();
            //try
            //{
            //    await handshakeTask;
            //}
            //catch (Exception ex) when (!(ex is TaskCanceledException))
            //{
            //    return new XDashSendResponse
            //    {
            //        Status = XDashSendResponseStatus.ErrorDuringHandshake,
            //        Message = ex.ToString()
            //    };
            //}
            //if (handshakeTask.IsCanceled)
            //{
            //    return new XDashSendResponse
            //    {
            //        Status = XDashSendResponseStatus.TimeoutDuringHandshake
            //    };
            //}


            var result = await feedback();
            if (!result)
            {
                return new XDashSendResponse
                {
                    Status = XDashSendResponseStatus.HandshakeRefused
                };
            }

            try
            {
                Stream crtStream = null;
                _timer.Elapsed += async () =>
                {
                    Debug.WriteLine($"~~~~~ {crtStream?.Position} PROGRESS ~~~~~ ");
                };
                _timer.Start(1);
                foreach (var file in dash.Files)
                {
                    var fileStream = await _filesystem.StreamFile(file.FullPath);
                    await sender.ConnectAsync(client.Ip, _settingsRepository.TransferPort);
                    crtStream = fileStream;
                    await fileStream.CopyToAsync(sender.WriteStream);
                    crtStream = null;
                    fileStream.Dispose();

                    await sender.DisconnectAsync();

                    await feedback();
                }
                _timer.Stop();
            }
            catch (Exception ex)
            {
                return new XDashSendResponse
                {
                    Status = XDashSendResponseStatus.ErrorDuringTransfer,
                    Message = ex.ToString()
                };
            }

            return new XDashSendResponse
            {
                Status = XDashSendResponseStatus.Success
            };
        }

        private async Task<bool> feedback()
        {
            var feedbackListenenr = new TcpSocketListener();
            feedbackListenenr.ConnectionReceived += onFeedbackReceived;
            await feedbackListenenr.StartListeningAsync(_settingsRepository.TransferFeedbackPort);

            _handshakeTcs = new TaskCompletionSource<bool>();
            var result = await _handshakeTcs.Task;

            feedbackListenenr.ConnectionReceived -= onFeedbackReceived;
            await feedbackListenenr.StopListeningAsync();

            return result;
        }

        private async void onFeedbackReceived(object sender, TcpSocketListenerConnectEventArgs e)
        {
            var memoryStream = new MemoryStream();
            await e.SocketClient.ReadStream.CopyToAsync(memoryStream);
            var handshakeResult = _binarySerializer.Deserialize<bool>(memoryStream.ToArray());
            _handshakeTcs.SetResult(handshakeResult);
        }
    }
}
