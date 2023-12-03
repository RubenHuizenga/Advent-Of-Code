using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public partial class Solution3
    {
        [GeneratedRegex(@"[^\d|.]")] private static partial Regex AnyButDigitOrPeriod();
        [GeneratedRegex(@"\d+")] private static partial Regex AnyNumber();
        [GeneratedRegex(@"\*")] private static partial Regex Gear();

        public static void Solve()
        {
            var lines = File.ReadLines("Day3/input.txt").ToArray();

            // Part 1
            var sumTotal = lines.Select((currentLine, lineIndex) =>
                (from number in AnyNumber().Matches(currentLine)
                 where AnyButDigitOrPeriod()
                     .IsMatch(lines[Math.Max(lineIndex - 1, 0)..Math.Min(lineIndex + 2, lines.Length)]
                         .Aggregate("", (current, subListLine) =>
                             current + subListLine[Math.Max(number.Index - 1, 0)..Math.Min(number.Index + number.Value.Length + 1, subListLine.Length)]))
                 select int.Parse(number.Value)).Sum()).Sum();

            Console.WriteLine($"Sum of all the engine parts: {sumTotal}");

            // Part 2
            var ratioSum = 0;

            for (var lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                foreach (var gear in Gear().Matches(lines[lineIndex]).ToList())
                {
                    var numbers = new List<Match>();
                    foreach (var line in lines[Math.Max(lineIndex - 1, 0)..Math.Min(lineIndex + 2, lines.Length)])
                    {
                        numbers.AddRange(AnyNumber().Matches(line)
                            .Where(m => m.Index - 1 <= gear.Index && gear.Index <= m.Index + m.Value.Length));
                    }

                    if (numbers.Count == 2)
                        ratioSum += int.Parse(numbers[0].Value) * int.Parse(numbers[1].Value);
                }
            }

            Console.WriteLine($"Sum of all the gear ratios: {ratioSum}");
        }

        /// <summary>
        /// Solving todays puzzle but very verbose
        /// </summary>
        public static void SolveVerbose()
        {
            const string filePath = "Day3/input.txt";

            var linesEnumerable = File.ReadLines(filePath);
            var lines = linesEnumerable.ToArray();
            var totalLines = lines.Length;

            // Part 1
            var sumTotal = 0;

            for (var lineIndex = 0; lineIndex < totalLines; lineIndex++)
            {
                var currentLine = lines[lineIndex];
                var currentLineLength = currentLine.Length;

                var numberRegex = AnyNumber();
                var numbers = numberRegex.Matches(currentLine);
                var numbersCount = numbers.Count;

                for (var currentNumberIndex = 0; currentNumberIndex < numbersCount; currentNumberIndex++)
                {
                    var number = numbers[currentNumberIndex];
                    var numberIndex = number.Index;
                    var numberValue = number.Value;
                    var numberValueLength = numberValue.Length;

                    var specialCharacterRegex = AnyButDigitOrPeriod();

                    var linesSubSetMin = Math.Max(lineIndex - 1, 0);
                    var linesSubSetMax = Math.Min(lineIndex + 2, totalLines);
                    var linesSubSetRange = linesSubSetMin..linesSubSetMax;
                    var linesSubSet = lines[linesSubSetRange];
                    var linesSubSetLength = linesSubSet.Length;

                    var stringSubSetMinComparator = numberIndex - 1;
                    const int stringSubSetMinConstant = 0;
                    var stringSubSetMin = Math.Max(stringSubSetMinComparator, stringSubSetMinConstant);

                    var stringSubSetMaxComparator = numberIndex + numberValueLength + 1;
                    var stringSubSetMax = Math.Min(stringSubSetMaxComparator, currentLineLength);

                    var stringSubSetRange = stringSubSetMin..stringSubSetMax;

                    var surroundingCharacters = "";
                    for (var linesSubSetIndex = 0; linesSubSetIndex < linesSubSetLength; linesSubSetIndex++)
                    {
                        var subListLine = linesSubSet[linesSubSetIndex];
                        var subListSubString = subListLine[stringSubSetRange];
                        surroundingCharacters += subListSubString;
                    }

                    var isSpecialCharacterMatch = specialCharacterRegex.IsMatch(surroundingCharacters);

                    if (!isSpecialCharacterMatch)
                        continue;

                    var intValue = int.Parse(numberValue);
                    sumTotal += intValue;
                }
            }

            var printValueEngineParts = $"Sum of all the engine parts: {sumTotal}";
            Console.WriteLine(printValueEngineParts);

            // Part 2
            var ratioSum = 0;

            for (var lineIndex = 0; lineIndex < totalLines; lineIndex++)
            {
                var currentLine = lines[lineIndex];

                var gearRegex = Gear();
                var gearMatches = gearRegex.Matches(currentLine);

                for (var currentGearIndex = 0; currentGearIndex < gearMatches.Count; currentGearIndex++)
                {
                    var gear = gearMatches[currentGearIndex];
                    var gearIndex = gear.Index;

                    var numbers = new List<Match>();

                    var linesSubSetMin = Math.Max(lineIndex - 1, 0);
                    var linesSubSetMax = Math.Min(lineIndex + 2, totalLines);
                    var linesSubSetRange = linesSubSetMin..linesSubSetMax;
                    var linesSubSet = lines[linesSubSetRange];
                    var linesSubSetLength = linesSubSet.Length;

                    for (var linesSubSetIndex = 0; linesSubSetIndex < linesSubSetLength; linesSubSetIndex++)
                    {
                        var line = linesSubSet[linesSubSetIndex];

                        var numberRegex = AnyNumber();
                        var numberMatches = numberRegex.Matches(line);
                        var numberMatchesList = numberMatches.ToList();
                        var numberMatchesListCount = numberMatchesList.Count;

                        for (var currentNumberIndex = 0; currentNumberIndex < numberMatchesListCount; currentNumberIndex++)
                        {
                            var number = numberMatchesList[currentNumberIndex];
                            var numberIndex = number.Index;
                            var numberValue = number.Value;
                            var numberValueLength = numberValue.Length;

                            var gearAboveLowerBound = numberIndex - 1 <= gearIndex;
                            var gearUnderUpperBound = gearIndex <= numberIndex + numberValueLength;

                            if (gearAboveLowerBound && gearUnderUpperBound)
                                numbers.Add(number);
                        }
                    }

                    var numbersCount = numbers.Count;
                    if (numbersCount != 2)
                        continue;

                    var numbersFirst = numbers[0];
                    var numbersFirstValue = numbersFirst.Value;
                    var numbersFirstValueInt = int.Parse(numbersFirstValue);

                    var numbersSecond = numbers[1];
                    var numbersSecondValue = numbersSecond.Value;
                    var numbesrSecondValueInt = int.Parse(numbersSecondValue);

                    var gearRatio = numbersFirstValueInt * numbesrSecondValueInt;
                    ratioSum += gearRatio;
                }
            }

            var printValueGearRatios = $"Sum of all the gear ratios: {ratioSum}";
            Console.WriteLine(printValueGearRatios);
        }
    }
}