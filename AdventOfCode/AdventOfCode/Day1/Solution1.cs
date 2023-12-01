namespace AdventOfCode
{
    public class Solution1 : Solution
    {
        public override int Day => 1;

        private readonly Dictionary<string, int> validDigits = new()
        {
            {"one", 1 }, {"two", 2 }, {"three", 3 }, {"four", 4 }, {"five", 5 }, {"six", 6 }, {"seven", 7 }, {"eight", 8 }, {"nine", 9 }, 
            {"1", 1 }, {"2", 2 }, {"3", 3 }, {"4", 4 }, {"5", 5 }, {"6", 6 }, {"7", 7 }, {"8", 8 }, {"9", 9 }        
        };

        public override void Solve()
        {
            var lines = File.ReadLines("Day1/input.txt");
            var calibrationTotal = 0;

            foreach (var line in lines)
            {
                int? firstDigit = null;
                int? lastDigit = null;
                for (int i = 0; i < line.Length; i++)
                {
                    foreach (var digit in validDigits)
                    {
                        if (line[i..].StartsWith(digit.Key))
                        {
                            firstDigit ??= digit.Value;
                            lastDigit = digit.Value;
                        }
                    }
                }

                calibrationTotal += (firstDigit ?? 0) * 10 + (lastDigit ?? 0);
            }

            Console.WriteLine($"The sum of the calibration values is {calibrationTotal}");
        }
    }
}
