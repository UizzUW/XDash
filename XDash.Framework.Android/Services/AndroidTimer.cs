﻿using System;
using System.Threading.Tasks;
using System.Timers;
using XDash.Framework.Services.Contracts.Platform;

namespace XDash.Framework.Droid.Services
{
    public class AndroidTimer : ITimer
    {
        private Timer _timer;

        public bool IsEnabled => _timer.Enabled;

        public void Start(int interval)
        {
            if (_timer != null && _timer.Enabled)
            {
                return;
            }
            _timer = new Timer(interval * 1000);
            _timer.Elapsed += onElapsed;
            _timer.Start();
        }

        public void Stop()
        {
            if (_timer == null)
            {
                return;
            }
            _timer.Elapsed -= onElapsed;
            _timer.Stop();
            _timer = null;
        }

        private async void onElapsed(object sender, EventArgs e)
        {
            if (_timer == null)
            {
                return;
            }
            var invoke = Elapsed?.Invoke();
            if (invoke != null)
            {
                await invoke;
            }
            Stop();
        }

        public event Func<Task> Elapsed;
    }
}