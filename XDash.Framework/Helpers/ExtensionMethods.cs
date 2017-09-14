using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XDash.Framework.Helpers
{
    public static class ExtensionMethods
    {
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

        public static async Task TaskOnUiThread<T>(Func<Task<T>> task)
        {
            var tcs = new TaskCompletionSource<T>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    
                    tcs.SetResult(await task());
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            await tcs.Task;
        }

        public static async Task TaskOnUiThread(Func<Task> task)
        {
            var tcs = new TaskCompletionSource<object>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await task();
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            await tcs.Task;
        }
    }
}
