using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CAHFrame.Utils
{
    public class ThreadPoolManager
    {
        public static Timer Schedule(int delay, Action action)
        {
            Timer timer = new Timer(new TimerCallback(delegate (object obj)
            {
                action.Invoke();
            }), null, delay, Timeout.Infinite);
            return timer;
        }
    }
}
