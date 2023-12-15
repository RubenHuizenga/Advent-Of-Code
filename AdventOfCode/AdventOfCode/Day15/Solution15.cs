using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Solution15
    {
        private class Box
        {
            private readonly List<KeyValuePair<string, int>> lenses = new();
            public int Count => lenses.Count;

            public void Add(string key, int value) => lenses.Add(new KeyValuePair<string, int>(key, value));

            public void Remove(string key) => lenses.Remove(lenses.FirstOrDefault(l => l.Key == key));

            public bool ContainsLens(string key) => lenses.Any(l => l.Key == key);

            public int this[string key]
            {
                get => lenses.FirstOrDefault(l => l.Key == key).Value;
                set => lenses[lenses.IndexOf(lenses.FirstOrDefault(l => l.Key == key))] = new KeyValuePair<string, int>(key, value);
            }

            public int this[int key] => lenses[key].Value;
        }

        public static void Solve()
        {
            var lines = File.ReadLines("Day15/input.txt").ToArray();

            var boxes = new Dictionary<int, Box>();

            var result = 0;

            foreach (var step in lines[0].Split(','))
            {
                var splitStep = step.Split(new char[] { '=', '-' });
                var label = splitStep[0];

                var boxNumber = 0;
                foreach (var c in label)
                    boxNumber = ((boxNumber + c) * 17) % 256;

                if (int.TryParse(splitStep[1], out var focalLength))
                {
                    if (!boxes.ContainsKey(boxNumber))
                        boxes.Add(boxNumber, new Box());

                    if (boxes[boxNumber].ContainsLens(label))
                        boxes[boxNumber][label] = focalLength;
                    else
                        boxes[boxNumber].Add(label, focalLength);
                }
                else if (boxes.ContainsKey(boxNumber))
                {
                    boxes[boxNumber].Remove(label);
                }
            }

            foreach (var boxNumber in boxes.Keys)
            {
                for (int i = 0; i < boxes[boxNumber].Count; i++)
                    result += (1 + boxNumber) * (i + 1) * boxes[boxNumber][i];
            }

            Console.WriteLine(result);
        }

        public static void Solve_Part1()
        {
            var lines = File.ReadLines("Day15/input.txt").ToArray();

            var result = 0;

            foreach (var step in lines[0].Split(','))
            {
                var stepResult = 0;

                foreach (var c in step)
                    stepResult = ((stepResult + c) * 17) % 256;

                result += stepResult;
            }

            Console.WriteLine(result);
        }
    }
}