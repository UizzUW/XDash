using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XDash.Helpers
{
    public static class Extensions
    {
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
