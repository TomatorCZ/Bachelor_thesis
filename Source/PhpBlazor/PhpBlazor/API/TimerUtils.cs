using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;

[assembly: PhpExtension]

namespace PhpBlazor
{
    [PhpType]
    public class Timer
    {
        private System.Timers.Timer timer;

        public Timer(double interval)
        {
            timer = new System.Timers.Timer(interval);
        }

        public void addEventElapsed(Context ctx, IPhpCallable handler)
        {
            void HandlerDelegate(object sender, ElapsedEventArgs args)
            {
                handler.Invoke(ctx, PhpValue.FromClr(sender), PhpValue.FromClass(args));
            }

            timer.Elapsed += new System.Timers.ElapsedEventHandler(HandlerDelegate);
        }

        public void Start() => timer.Start();

        public void Stop() => timer.Stop();
    }
}
