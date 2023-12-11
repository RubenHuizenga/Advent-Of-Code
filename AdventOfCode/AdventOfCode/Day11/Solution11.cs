using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Solution11
    {
        public static void Solve()
        {
            var lines = File.ReadLines("Day11/input.txt").ToArray();

            var emptyRows = new List<bool>();
            var emptyColumns = new List<bool>();
            var galaxies = new List<Tuple<int, int>>();

            for (var y = 0; y < lines.Length; y++)
            {
                emptyRows.Add(true);

                for (var x = 0; x < lines[y].Length; x++)
                {
                    if (emptyColumns.Count <= x)
                        emptyColumns.Add(true);

                    if (lines[y][x] == '#')
                    {
                        emptyRows[y] = false;
                        emptyColumns[x] = false;
                        galaxies.Add(new Tuple<int, int>(x, y));
                    }
                }
            }

            long result_Part1 = 0;
            long result_Part2 = 0;

            const int multiplier = 1000000 - 1;

            foreach (var galaxyA in galaxies)
            {
                var expandedA_Part1 = new Tuple<int, int>(galaxyA.Item1 + emptyColumns.ToArray()[0..galaxyA.Item1].Where(e => e == true).Count(),
                    galaxyA.Item2 + emptyRows.ToArray()[0..galaxyA.Item2].Where(e => e == true).Count());

                var expandedA_Part2 = new Tuple<int, int>(galaxyA.Item1 + (emptyColumns.ToArray()[0..galaxyA.Item1].Where(e => e == true).Count() * multiplier),
                    galaxyA.Item2 + (emptyRows.ToArray()[0..galaxyA.Item2].Where(e => e == true).Count() * multiplier));

                foreach (var galaxyB in galaxies)
                {
                    if (galaxyA == galaxyB)
                        continue;

                    var expandedB_Part1 = new Tuple<int, int>(galaxyB.Item1 + emptyColumns.ToArray()[0..galaxyB.Item1].Where(e => e == true).Count(),
                        galaxyB.Item2 + emptyRows.ToArray()[0..galaxyB.Item2].Where(e => e == true).Count());

                    var expandedB_Part2 = new Tuple<int, int>(galaxyB.Item1 + (emptyColumns.ToArray()[0..galaxyB.Item1].Where(e => e == true).Count() * multiplier),
                        galaxyB.Item2 + (emptyRows.ToArray()[0..galaxyB.Item2].Where(e => e == true).Count() * multiplier));

                    result_Part1 += Math.Abs(expandedA_Part1.Item1 - expandedB_Part1.Item1) + Math.Abs(expandedA_Part1.Item2 - expandedB_Part1.Item2);
                    result_Part2 += Math.Abs(expandedA_Part2.Item1 - expandedB_Part2.Item1) + Math.Abs(expandedA_Part2.Item2 - expandedB_Part2.Item2);
                }
            }

            Console.WriteLine(result_Part1 / 2);
            Console.WriteLine(result_Part2 / 2);
        }
    }
}