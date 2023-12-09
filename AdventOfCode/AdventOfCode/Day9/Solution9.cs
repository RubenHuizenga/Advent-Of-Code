using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public partial class Solution9
    {
        [GeneratedRegex(@"[-]?\d+")] private static partial Regex AnyNumberWithSign();

        public static void Solve()
        {
            var lines = File.ReadLines("Day9/input.txt").ToArray();

            var extrapolatedValueSumFuture = 0;
            var extrapolatedValueSumPast = 0;

            foreach (var line in lines)
            {
                var sequences = new List<List<int>> { AnyNumberWithSign().Matches(line).Select(m => int.Parse(m.Value)).ToList() };

                var index = 0;

                while (sequences[index].Any(i => i != 0))
                {
                    sequences.Add(new List<int>());

                    for (var i = 1; i < sequences[index].Count; i++)
                        sequences[index + 1].Add(sequences[index][i] - sequences[index][i - 1]);

                    index++;
                }

                for (var i = sequences.Count - 1; i > 0; i--)
                {
                    // Part 1
                    sequences[i - 1].Add(sequences[i - 1].Last() + sequences[i].Last());

                    // Part 2
                    sequences[i - 1].Insert(0, sequences[i - 1].First() - sequences[i].First());
                }

                extrapolatedValueSumFuture += sequences[0].Last();
                extrapolatedValueSumPast += sequences[0].First();
            }

            Console.WriteLine($"Sum of the next number in each sequence: {extrapolatedValueSumFuture}");
            Console.WriteLine($"Sum of the previous number in each sequence: {extrapolatedValueSumPast}");
        }
    }
}