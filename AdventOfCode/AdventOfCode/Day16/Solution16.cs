using System;
using System.Drawing;

namespace AdventOfCode
{
    public struct Vector2
    {
        public readonly int X;
        public readonly int Y;

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static readonly Vector2 Up = new(0, -1);
        public static readonly Vector2 Down = new(0, 1);
        public static readonly Vector2 Left = new(-1, 0);
        public static readonly Vector2 Right = new(1, 0);

        public readonly bool InBounds(string[] array) => Y < array.Length && Y >= 0 && X < array[Y].Length && X >= 0;

        public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
        public static bool operator ==(Vector2 a, Vector2 b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Vector2 a, Vector2 b) => !(a == b);
    }


    public class Solution16
    {
        public static void Solve()
        {
            var lines = File.ReadLines("Day16/input.txt").ToArray();

            // Part 1
            var part1 = SolveConfiguration(lines, new Vector2(0, 0), Vector2.Right);
            Console.WriteLine(part1);

            // Part 2
            var maxEnergized = 0;
            for (var x = 0; x < lines[0].Length; x++)
            {
                maxEnergized = Math.Max(SolveConfiguration(lines, new Vector2(x, 0), Vector2.Down), maxEnergized);
                maxEnergized = Math.Max(SolveConfiguration(lines, new Vector2(x, lines.Length - 1), Vector2.Up), maxEnergized);
            }

            for (var y = 0; y < lines.Length; y++)
            {
                maxEnergized = Math.Max(SolveConfiguration(lines, new Vector2(0, y), Vector2.Right), maxEnergized);
                maxEnergized = Math.Max(SolveConfiguration(lines, new Vector2(lines[0].Length - 1, y), Vector2.Left), maxEnergized);
            }

            Console.WriteLine(maxEnergized);
        }

        public static int SolveConfiguration(string[] lines, Vector2 currentTile, Vector2 currentDirection)
        {
            var energizedTiles = new List<Vector2>();
            var usedSplitters = new List<Vector2>();

            var paths = new Queue<Tuple<Vector2, Vector2>>();

            paths.Enqueue(new Tuple<Vector2, Vector2>(currentTile, currentDirection));

            while (paths.Count > 0)
            {
                (currentTile, currentDirection) = paths.Dequeue();

                var isSplit = false;

                while (currentTile.InBounds(lines))
                {
                    if (!energizedTiles.Contains(currentTile))
                        energizedTiles.Add(currentTile);

                    switch (lines[currentTile.Y][currentTile.X])
                    {
                        case '|' when (currentDirection == Vector2.Right || currentDirection == Vector2.Left):
                            if (!usedSplitters.Contains(currentTile))
                            {
                                usedSplitters.Add(currentTile);
                                paths.Enqueue(new Tuple<Vector2, Vector2>(currentTile, Vector2.Up));
                                paths.Enqueue(new Tuple<Vector2, Vector2>(currentTile, Vector2.Down));
                            }
                            isSplit = true;
                            break;
                        case '-' when (currentDirection == Vector2.Up || currentDirection == Vector2.Down):
                            if (!usedSplitters.Contains(currentTile))
                            {
                                usedSplitters.Add(currentTile);
                                paths.Enqueue(new Tuple<Vector2, Vector2>(currentTile, Vector2.Left));
                                paths.Enqueue(new Tuple<Vector2, Vector2>(currentTile, Vector2.Right));
                            }
                            isSplit = true;
                            break;
                        case '/':
                            if (currentDirection == Vector2.Left || currentDirection == Vector2.Right)
                                currentDirection = new Vector2(currentDirection.Y, -currentDirection.X);
                            else if (currentDirection == Vector2.Up || currentDirection == Vector2.Down)
                                currentDirection = new Vector2(-currentDirection.Y, currentDirection.X);
                            break;
                        case '\\':
                            if (currentDirection == Vector2.Left || currentDirection == Vector2.Right)
                                currentDirection = new Vector2(-currentDirection.Y, currentDirection.X);
                            else if (currentDirection == Vector2.Up || currentDirection == Vector2.Down)
                                currentDirection = new Vector2(currentDirection.Y, -currentDirection.X);
                            break;
                    }

                    if (!isSplit)
                        currentTile += currentDirection;
                    else
                        break;
                }
            }

            return energizedTiles.Count;
        }

}