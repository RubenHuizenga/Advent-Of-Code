using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public partial class Solution19
    {
        [GeneratedRegex("(\\D+){(((\\D[<|>]\\d+:\\D+),)*(\\D+))}")] private static partial Regex Workflow();
        [GeneratedRegex("{(\\D=(\\d+),*)*}")] private static partial Regex MachinePart();
        [GeneratedRegex("(<|>)|:")] private static partial Regex Operator();

        private class Part<T>
        {
            private readonly Dictionary<char, T> _values;

            public Part(T x, T m, T a, T s) => _values = new Dictionary<char, T> { { 'x', x }, { 'm', m }, { 'a', a }, { 's', s } };

            public T this[char key]
            {
                get
                {
                    if (_values.TryGetValue(key, out var value))
                        return value;
                    throw new ArgumentException($"Invalid argument for Part: '{key}' (should be [x, m, a, s])");
                }
                set
                {
                    if (!_values.ContainsKey(key))
                        throw new ArgumentException($"Invalid argument for Part: '{key}' (should be [x, m, a, s])");

                    _values[key] = value;
                }
            }

            public Part(Part<T> original)
                => _values = new Dictionary<char, T> { { 'x', original['x'] }, { 'm', original['m'] }, { 'a', original['a'] }, { 's', original['s'] } };

            public override string ToString() => $"x={_values['x']}, m={_values['m']}, a={_values['a']}, s={_values['s']}";
        }

        private partial class Instruction
        {
            private readonly struct Constraint
            {
                public readonly char Key;
                public readonly char Operation;
                public readonly int Value;

                public Constraint(char key, char operation, int value)
                {
                    Key = key;
                    Operation = operation;
                    Value = value;
                }
            }

            private readonly Constraint? _constraint;
            private readonly string _result;

            public Instruction(string instruction)
            {
                var parts = Operator().Split(instruction);

                if (parts.Length == 1)
                    _result = parts[0];
                else
                {
                    _constraint = new Constraint(parts[0][0], parts[1][0], int.Parse(parts[2]));
                    _result = parts[3];
                }
            }

            public bool Evaluate(Part<int> input, out string? nextInstruction) => Evaluate(_constraint.HasValue ? input[_constraint.Value.Key] : null, out nextInstruction);

            private bool Evaluate(int? input, out string? nextInstruction)
            {
                var evaluation = true;
                if (_constraint.HasValue)
                    evaluation = _constraint.Value.Operation switch
                    {
                        '>' => input > _constraint.Value.Value,
                        '<' => input < _constraint.Value.Value,
                        _ => throw new Exception($"Unexpected operator for instruction: '{_constraint.Value.Operation}' (should be [<, >])")
                    };

                nextInstruction = evaluation ? _result : null;
                return evaluation;
            }

            public bool Evaluate(Part<Range> input, out string? nextInstruction, out (Part<Range>?, Part<Range>) newRanges)
            {
                if (!_constraint.HasValue)
                {
                    newRanges = (null, input);
                    nextInstruction = _result;
                    return true;
                }

                if (_constraint.Value.Value <= input[_constraint.Value.Key].Start.Value || _constraint.Value.Value >= input[_constraint.Value.Key].End.Value)
                {
                    newRanges = (null, input);
                    return Evaluate(input[_constraint.Value.Key].Start.Value, out nextInstruction);
                }

                nextInstruction = _result;

                newRanges = (new Part<Range>(input), new Part<Range>(input));

                switch (_constraint.Value.Operation)
                {
                    case '>':
                        newRanges.Item1[_constraint.Value.Key] = new Range(input[_constraint.Value.Key].Start, _constraint.Value.Value);
                        newRanges.Item2[_constraint.Value.Key] = new Range(_constraint.Value.Value + 1, input[_constraint.Value.Key].End);
                        break;
                    case '<':
                        newRanges.Item1[_constraint.Value.Key] = new Range(_constraint.Value.Value, input[_constraint.Value.Key].End);
                        newRanges.Item2[_constraint.Value.Key] = new Range(input[_constraint.Value.Key].Start, _constraint.Value.Value - 1);
                        break;
                    default:
                        throw new Exception($"Unexpected operator for instruction: '{_constraint.Value.Operation}' (should be [<, >])");
                }

                return true;
            }

            public override string ToString() => !_constraint.HasValue ? $"{_result}" : $"{_constraint.Value.Key} {_constraint.Value.Operation} {_constraint.Value.Value}";
        }

        public static void Solve()
        {
            var lines = File.ReadLines("Day19/input.txt").ToArray();

            var workFlows = new Dictionary<string, List<Instruction>>();
            var acceptedParts = new List<Part<int>>();

            var lineIndex = -1;
            while (!string.IsNullOrEmpty(lines[++lineIndex]))
            {
                var match = Workflow().Match(lines[lineIndex]);

                var instructions = match.Groups[4].Captures.Select(i => new Instruction(i.Value))
                    .Append(new Instruction(match.Groups[5].Value)).ToList();

                workFlows.Add(match.Groups[1].Value, instructions);
            }

            // Part 1
            for (++lineIndex; lineIndex < lines.Length; lineIndex++)
            {
                var values = MachinePart().Match(lines[lineIndex]).Groups[2].Captures
                    .Select(c => int.Parse(c.Value)).ToArray();
                var part = new Part<int>(values[0], values[1], values[2], values[3]);

                var workFlowKey = "in";
                var instructionIndex = 0;
                while (workFlowKey != "R" && workFlowKey != "A")
                {
                    if (!workFlows[workFlowKey][instructionIndex].Evaluate(part, out var nextKey))
                        instructionIndex++;
                    else
                        instructionIndex = 0;

                    workFlowKey = nextKey ?? workFlowKey;
                }

                if (workFlowKey == "A")
                    acceptedParts.Add(part);
            }

            var result = acceptedParts.Sum(p => p['x'] + p['m'] + p['a'] + p['s']);

            Console.WriteLine(result);

            // Part 2
            var acceptedRanges = new List<Part<Range>>();

            var startRanges = new Part<Range>(new Range(1, 4000), new Range(1, 4000), new Range(1, 4000), new Range(1, 4000));

            var workflowQueue = new Queue<Tuple<KeyValuePair<string, List<Instruction>>, Part<Range>>>();
            workflowQueue.Enqueue(new Tuple<KeyValuePair<string, List<Instruction>>, Part<Range>>(
                    new KeyValuePair<string, List<Instruction>>("in", workFlows["in"]), startRanges));

            while (workflowQueue.Any())
            {
                var workflow = workflowQueue.Dequeue();
                var currentPartRanges = workflow.Item2;

                foreach (var instruction in workflow.Item1.Value)
                {
                    var instructionTrueForSubRange = instruction.Evaluate(currentPartRanges, out var nextKey, out var newRanges);

                    if (instructionTrueForSubRange)
                    {
                        if (nextKey == "A")
                            acceptedRanges.Add(newRanges.Item2);
                        else if (nextKey == null)
                            throw new Exception($"nextKey should not be null if instruction is true. Instrucion {instruction} was true for {currentPartRanges}");
                        else if (nextKey != "R")
                            workflowQueue.Enqueue(new Tuple<KeyValuePair<string, List<Instruction>>, Part<Range>>(new KeyValuePair<string, List<Instruction>>(nextKey, workFlows[nextKey]), newRanges.Item2));
                    }

                    currentPartRanges = newRanges.Item1;

                    if (currentPartRanges == null)
                        break;
                }
            }

            var resultPart2 = acceptedRanges.Sum(r =>
                ((long)(r['x'].End.Value - r['x'].Start.Value) + 1) *
                ((long)(r['m'].End.Value - r['m'].Start.Value) + 1) *
                ((long)(r['a'].End.Value - r['a'].Start.Value) + 1) *
                ((long)(r['s'].End.Value - r['s'].Start.Value) + 1));

            Console.WriteLine(resultPart2);
        }
    }
}