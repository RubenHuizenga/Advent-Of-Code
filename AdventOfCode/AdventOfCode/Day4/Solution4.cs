using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public partial class Solution4
    {
        [GeneratedRegex(@"(?<=:) *((\d+) *)* *(?=\|)")] private static partial Regex WinnerNumbers();
        [GeneratedRegex(@"(?<=\|) *((\d+) *)*(?=$)")] private static partial Regex YourNumbers();

        public static void Solve()
        {
            var lines = File.ReadLines("Day4/input.txt").ToArray();

            var sumTotal = 0;
            var cardCounter = new Dictionary<int, int>();

            for (var index = 0; index < lines.Length; index++)
            {
                var line = lines[index];

                var cardNumber = index + 1;
                cardCounter.TryAdd(cardNumber, 1);

                // Part 1
                var winningNumbers = WinnerNumbers().Match(line).Groups[2].Captures.Select(c => c.Value);
                var yourNumbers = YourNumbers().Match(line).Groups[2].Captures.Select(c => c.Value);

                var matches = yourNumbers.Intersect(winningNumbers).ToList();

                if (matches.Any())
                    sumTotal += (int)Math.Pow(2, matches.Count - 1);

                // Part 2
                for (var copyIndex = cardNumber + 1; copyIndex <= cardNumber + matches.Count; copyIndex++)
                {
                    if (cardCounter.ContainsKey(copyIndex))
                        cardCounter[copyIndex] += cardCounter[cardNumber];
                    else
                        cardCounter.Add(copyIndex, 1 + cardCounter[cardNumber]);
                }
            }

            var totalCards = cardCounter.Sum(kvp => kvp.Value);

            Console.WriteLine($"Total worth of scratchcards: {sumTotal}");
            Console.WriteLine($"Total amount of scratchcards: {totalCards}");
        }
    }
}