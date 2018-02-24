using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using XDash.Framework.Components.Transfer.Contracts;
using XDash.Framework.Models;
using XDash.Framework.Models.Abstractions;
using XDash.Framework.Services.Contracts;
using XDash.Framework.Services.Contracts.Platform;
using XDash.Framework.Configuration.Contracts;
using XDash.Framework.Configuration;
using System.Net.Sockets;

namespace XDash.Framework.Components.Transfer
{
    public class Controller : IController
    {
        private readonly IBsonSerializer _binarySerializer;
        private readonly XDashOptions _options;
        private readonly IFilesystem _filesystem;
        private readonly ITimer _timer;
        private NetworkStream _endpointStream;

        public Controller(IBsonSerializer binarySerializer,
                           IConfigurator configurator,
                           IFilesystem filesystem,
                           ITimer timer)
        {
            _binarySerializer = binarySerializer;
            _options = configurator.GetConfiguration();
            _filesystem = filesystem;
            _timer = timer;
        }

        public async Task<XDashSendResponse> Send(IXDashClient client, Models.XDash dash)
        {
            var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(client.Ip, _options.Transfer.TransferPort);
            _endpointStream = tcpClient.GetStream();

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
            await _endpointStream.WriteAsync(serializedData, 0, serializedData.Length, new CancellationTokenSource(3000).Token);

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
                _endpointStream.Close();
                tcpClient.Close();
                return new XDashSendResponse
                {
                    Status = XDashSendResponseStatus.HandshakeRefused
                };
            }

            try
            {
                //Stream crtStream = null;
                //_timer.Elapsed += async () =>
                //{
                //    Debug.WriteLine($"~~~~~ {crtStream?.Position} PROGRESS ~~~~~ ");
                //};
                //_timer.Start(1);
                foreach (var file in dash.Files)
                {
                    using (var fileStream = await _filesystem.StreamFile(file.FullPath))
                    {
                        //crtStream = fileStream;
                        await fileStream.CopyToAsync(_endpointStream);
                        //crtStream = null;
                    }

                    await feedback();
                }
                _timer.Stop();
            }
            catch (Exception ex)
            {
                _endpointStream.Close();
                tcpClient.Close();
                return new XDashSendResponse
                {
                    Status = XDashSendResponseStatus.ErrorDuringTransfer,
                    Message = ex.ToString()
                };
            }

            _endpointStream.Close();
            tcpClient.Close();
            return new XDashSendResponse
            {
                Status = XDashSendResponseStatus.Success
            };
        }


        private async Task<bool> feedback()
        {
            var memoryStream = new MemoryStream();
            await _endpointStream.CopyToAsync(memoryStream);
            var handshakeResult = _binarySerializer.Deserialize<bool>(memoryStream.ToArray());
            return handshakeResult;
        }
    }
}
