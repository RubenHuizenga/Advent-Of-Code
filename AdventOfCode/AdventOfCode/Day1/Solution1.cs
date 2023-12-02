using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public partial class Solution1
    {
        private static readonly Dictionary<string, string> LetterDigits = new()
        {
            { "one", "one1one" }, { "two", "two2two" }, { "three", "three3three" }, { "four", "four4four" },
            { "five", "five5five" }, { "six", "six6six" }, { "seven", "seven7seven" }, { "eight", "eight8eight" },
            { "nine", "nine9nine" }
        };

        [GeneratedRegex("\\d")]
        private static partial Regex Digit();

        public static void Solve(bool includeLetterDigits = false)
        {
            var lines = File.ReadLines("Day1/input.txt");
            var calibrationTotal = 0;

            foreach (var line in lines)
            {
                var parsedLine = includeLetterDigits
                    ? LetterDigits.Aggregate(line, (current, kvp) => current.Replace(kvp.Key, kvp.Value))
                    : line;

                calibrationTotal += int.Parse(Digit().Match(parsedLine).Value) * 10;
                calibrationTotal += int.Parse(Digit().Match(new string(parsedLine.Reverse().ToArray())).Value);
            }

            Console.WriteLine($"The sum of the calibration values is {calibrationTotal}");
        }
    }
}