/*
    Credits : http://stackoverflow.com/questions/31107459/system-timer-missing-from-xamarin-pcl
*/

using System;
using System.Threading.Tasks;

namespace XDash.Framework.Helpers
{
    public class Timer
    {

        private int mWaitTime;
        public int WaitTime
        {
            get { return mWaitTime; }
            set { mWaitTime = value; }
        }

        private bool mIsRunning;
        public bool IsRunning
        {
            get { return mIsRunning; }
            set { mIsRunning = value; }
        }

        public event EventHandler Elapsed;
        protected virtual void OnTimerElapsed()
        {
            Elapsed?.Invoke(this, new EventArgs());
        }

        public Timer(int waitTime)
        {
            WaitTime = waitTime;
        }

        public async Task Start()
        {
            int seconds = 0;
            IsRunning = true;
            while (IsRunning)
            {
                if (seconds != 0 && seconds % WaitTime == 0)
                {
                    OnTimerElapsed();
                }
                await Task.Delay(1);
                seconds++;
            }
        }

        public void Stop()
        {
            IsRunning = false;
        }
    }
}
