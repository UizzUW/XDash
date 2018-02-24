using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace XDash.Framework.Helpers
{
    public static class ExtensionMethods
    {
        public static string GetValidIPv4(this NetworkInterface nic)
        {
            return nic
                .GetIPProperties()
                .UnicastAddresses
                .FirstOrDefault(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork)
                ?.Address
                .ToString();
        }

        public class UdpReceiveResult
        {
            public bool Success { get; set; }
            public byte[] Message { get; set; }
            public Exception Exception { get; set; }
            public IPEndPoint RemoteEndPoint { get; set; }
        }

        public static async Task<UdpReceiveResult> ReceiveAsyncEx(this UdpClient client)
        {
            var result = new UdpReceiveResult();

            var receiveEndpoint = new IPEndPoint(IPAddress.Any, 0);
            var receiveResult = await Task.Factory.FromAsync(
                client.BeginReceive,
                r =>
                {
                    try
                    {
                        result.Message = client.EndReceive(r, ref receiveEndpoint);
                        result.RemoteEndPoint = receiveEndpoint;
                        result.Success = true;
                    }
                    catch (Exception ex)
                    {
                        result.Exception = ex;
                    }
                    return result.Message;
                },
                TaskCreationOptions.None
            );
            return result;
        }

        public static string AsFormattedBytesValue(this ulong byteCount)
        {
            const float iKb = 1024;
            const float iMb = 1048576;
            const float iGb = 1073741824;
            if (byteCount < iKb)
            {
                return $"{byteCount:0.##} B";
            }
            if (byteCount >= iKb && byteCount < iMb)
            {
                return $"{byteCount / iKb:0.##} KB";
            }
            if (byteCount >= iMb && byteCount < iGb)
            {
                return $"{byteCount / iMb:0.##} MB";
            }
            return $"{byteCount / iGb:0.##} GB";
        }
    }
}
