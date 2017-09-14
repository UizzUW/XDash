using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.UWP.Services
{
    public class UwpTimer : ITimer
    {
        private DispatcherTimer _timer;

        public bool IsEnabled => _timer.IsEnabled;

        public void Start(int interval)
        {
            if (_timer != null && _timer.IsEnabled)
            {
                return;
            }
            _timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, interval)
            };
            _timer.Tick += onTick;
            _timer.Start();
        }

        public void Stop()
        {
            if (_timer == null)
            {
                return;
            }
            _timer.Tick -= onTick;
            _timer.Stop();
            _timer = null;
        }

        private async void onTick(object sender, object e)
        {
            await Elapsed?.Invoke();
        }

        public event Func<Task> Elapsed;
    }
}
