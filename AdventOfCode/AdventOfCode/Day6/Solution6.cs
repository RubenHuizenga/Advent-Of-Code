using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Solution6
    {
        public static void Solve()
        {
            var lines = File.ReadLines("Day6/input.txt").ToArray();

            // Part 1
            var times = Regex.Matches(lines[0], @"\d+").Select(m => int.Parse(m.Value)).ToList();
            var distances = Regex.Matches(lines[1], @"\d+").Select(m => int.Parse(m.Value)).ToList();

            double multipleRaceResult = 1;

            for (var i = 0; i < times.Count; i++)
            {
                multipleRaceResult *= WaysToWinRace(times[i], distances[i]);
            }

            Console.WriteLine($"Product of the number of ways to beat each race: {multipleRaceResult}");

            // Part 2
            var time = long.Parse(Regex.Match(lines[0], @"\d(\d+| )*\d").Value.Replace(" ", ""));
            var distance = long.Parse(Regex.Match(lines[1], @"\d(\d+| )*\d").Value.Replace(" ", ""));

            var singleRaceResult = WaysToWinRace(time, distance);

            Console.WriteLine($"Product of the number of ways to beat each race: {singleRaceResult}");
        }

        private static double WaysToWinRace(long time, long distance)
        {
            var discriminator = (-time * -time) - (4 * distance);

            var lowerLimit = Math.Floor((time - Math.Sqrt(discriminator)) / 2);
            var upperLimit = Math.Ceiling((time + Math.Sqrt(discriminator)) / 2) - 1;

            return upperLimit - lowerLimit;
        }
    }
}