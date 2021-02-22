using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;

namespace ImportProfilesService
{
    class MainServiceClass
    {
        private readonly Timer _timer;

        public MainServiceClass()
        {
            _timer = new Timer(1000) { AutoReset = true };
            _timer.Elapsed += TimerElasped;
        }

        private void TimerElasped(object sender,ElapsedEventArgs e)
        {
            string[] lines = new string[] { DateTime.Now.ToString() };
            File.AppendAllLines(@"test.txt", lines);
        }

        public void Start()
        {
            _timer.Start();
        }
        public void Stop()
        {
            _timer.Stop();
        }


    }
}
