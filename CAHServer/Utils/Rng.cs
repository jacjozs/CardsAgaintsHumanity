using System;
using System.Collections.Generic;
using System.Text;

namespace CAHServer.Utils
{
    public class Rng
    {
        private static readonly Random RNG = new Random();

        public static int Next()
        {
            return RNG.Next();
        }
        public static int Next(int max)
        {
            return RNG.Next(max);
        }
        public static int Next(int min, int max)
        {
            return RNG.Next(min, max);
        }
        public static double NextDouble()
        {
            return RNG.NextDouble();
        }
    }
}
