using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public abstract class Solution
    {
        public abstract int Day { get; }

        public abstract void Solve();
        public void PrintDay()
        {
            Console.WriteLine($"Solution for day {Day}");
        }
    }
}
