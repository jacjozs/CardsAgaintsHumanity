using System;
using System.Collections.Generic;
using System.Text;

namespace CAHLib.Utils
{
    public class Utils
    {
        public static long DateTimeToTimestamp(DateTime time)
        {
            TimeSpan t = (time - new DateTime(1970, 1, 1).ToUniversalTime());
            return (long)t.TotalSeconds;
        }
    }
}
