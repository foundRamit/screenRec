using System;
using System.Diagnostics;

namespace ScreenRecorderApp.Utils
{
    public class TimerHelper
    {
        private Stopwatch stopwatch;
        private TimeSpan pausedTime;
        private bool isPaused;

        public TimerHelper()
        {
            stopwatch = new Stopwatch();
            pausedTime = TimeSpan.Zero;
            isPaused = false;
        }

        public void Start()
        {
            stopwatch.Start();
        }

        public void Pause()
        {
            if (!isPaused)
            {
                stopwatch.Stop();
                pausedTime = stopwatch.Elapsed;
                isPaused = true;
            }
        }

        public void Resume()
        {
            if (isPaused)
            {
                stopwatch.Start();
                isPaused = false;
            }
        }

        public void Stop()
        {
            stopwatch.Stop();
        }

        public TimeSpan Elapsed => stopwatch.Elapsed;
    }
}
