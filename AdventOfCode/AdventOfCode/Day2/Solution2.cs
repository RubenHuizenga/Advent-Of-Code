using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Solution2
    {
        public static void Solve()
        {
            var lines = File.ReadLines("Day2/input.txt");

            // Part 1
            var sumTotal = (from line in lines
                            where GetMaxForColor(line, "red") <= 12 && GetMaxForColor(line, "green") <= 13 && GetMaxForColor(line, "blue") <= 14
                            select int.Parse(Regex.Match(line, @"Game (\d+)").Groups[1].Value)).Sum();
            Console.WriteLine($"Sum of the ID's of possible games: {sumTotal}");

            // Part 2
            var powerSum = lines.Sum(line => GetMaxForColor(line, "red") * GetMaxForColor(line, "green") * GetMaxForColor(line, "blue"));
            Console.WriteLine($"Sum of the powers of the minimum needed set for each game: {powerSum}");
        }

        private static int GetMaxForColor(string line, string color) =>
            Regex.Matches(line, @$"(\d+) {color}").Max(m => int.Parse(m.Groups[1].Value));
    }
}