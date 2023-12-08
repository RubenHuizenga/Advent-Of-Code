using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public partial class Solution8
    {
        [GeneratedRegex(@"\w{3}")] private static partial Regex ThreeLetterWord();

        public static void Solve()
        {
            var lines = File.ReadLines("Day8/input.txt").ToArray();

            var instructions = lines[0];
            var maps = new Dictionary<string, string[]>();

            var currentPositions = new List<string>();

            for (var i = 2; i < lines.Length; i++)
            {
                var keys = ThreeLetterWord().Matches(lines[i]);

                maps.Add(keys[0].Value, new[] { keys[1].Value, keys[2].Value });

                if (keys[0].Value[2] == 'A')
                    currentPositions.Add(keys[0].Value);
            }

            // Part 1
            var currentPosition = "AAA";
            var steps = 0;

            while (currentPosition != "ZZZ")
                currentPosition = maps[currentPosition][instructions[steps++ % instructions.Length] == 'R' ? 1 : 0];

            Console.WriteLine($"Number of steps required to reach ZZZ: {steps}");

            // Part 2
            long? result = null;

            for (var index = 0; index < currentPositions.Count; index++)
            {
                var cycleLength = 0;

                while (currentPositions[index][2] != 'Z')
                    currentPositions[index] = maps[currentPositions[index]][instructions[cycleLength++ % instructions.Length] == 'R' ? 1 : 0];

                result = result == null ? cycleLength : Lcm(result.Value, cycleLength);
            }

            Console.WriteLine($"Number of steps required to reach all nodes ending in Z: {result}");
        }

        private static long Lcm(long a, long b) => a * b / Gcd(a, b);

        private static long Gcd(long a, long b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }
    }
}