using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace MediaStat.Services
{
    public sealed class BlazorTimer
    {
        private Timer _timer;

        public void SetTimer(double intervalMilliseconds, bool repeat)
        {
            _timer = new Timer(intervalMilliseconds);
            _timer.Elapsed += NotifyTimerElapsed;
            _timer.AutoReset = repeat;
            _timer.Enabled = repeat;
        }

        public event Action OnElapsed;

        private void NotifyTimerElapsed(object source, ElapsedEventArgs e)
        {
            OnElapsed?.Invoke();
            if (!_timer.AutoReset)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
        }

        public void StopTimer()
        {
            _timer.Stop();
            _timer.Dispose();
            _timer = null;
        }

    }
}
