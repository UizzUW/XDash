using System;
using System.Threading.Tasks;
using System.Timers;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Droid.Services
{
    public class DroidTimer : ITimer
    {
        private Timer _timer;

        public bool IsEnabled => _timer.Enabled;

        public void Start(uint interval)
        {
            if (_timer != null && _timer.Enabled)
            {
                return;
            }
            _timer = new Timer(interval);
            _timer.Elapsed += onElapsed;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Elapsed -= onElapsed;
            _timer.Stop();
            _timer = null;
        }

        private async void onElapsed(object sender, EventArgs e) => await Elapsed?.Invoke();

        public event Func<Task> Elapsed;
    }
}