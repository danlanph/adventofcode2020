using System;
using System.Linq;

namespace danl.adventofcode2020.EncodingError09
{
    [Puzzle(puzzleNumber: 9, numberOfParts: 2)]
    public class EncodingError09
    {
        public const string InputFileResourceName = "danl.adventofcode2020.EncodingError09.input.txt";

        private readonly long[] _encoding;

        public EncodingError09(string inputString)
        {
            _encoding = inputString
                .Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => long.Parse(i))
                .ToArray();
        }

        public static void Run(int part)
        {
            var input = InputHelper.GetResourceFileAsString(InputFileResourceName);
            var preambleLength = 25;

            var encodingError = new EncodingError09(input);

            switch (part)
            {                
                case 1:
                    {
                        var firstEncodingError = encodingError.GetFirstEncodingError(preambleLength);
                        Console.WriteLine($"The first encoding error is the number {firstEncodingError}.");
                        break;
                    }
                case 2:
                    {
                        var firstEncodingError = encodingError.GetFirstEncodingError(preambleLength);
                        var contiguousBlockWithSpecificSum = encodingError.GetContiguousBlockWithSum(firstEncodingError);
                        Console.WriteLine($"The first contiguous block that sums to {firstEncodingError} are the numbers {string.Join(',', contiguousBlockWithSpecificSum)}.");

                        var SumMaxAndMin = contiguousBlockWithSpecificSum.Min() + contiguousBlockWithSpecificSum.Max();
                        Console.WriteLine($"The max and min element sum to {SumMaxAndMin}");
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }            
        }

        private long[] GetContiguousBlockWithSum(long sumToFind)
        {
            for (var i = 0; i < _encoding.Length - 1; i++)
            {
                var sum = _encoding[i] + _encoding[i + 1];
                if (sum == sumToFind)
                    return new[] { _encoding[i], _encoding[i + 1] };

                for (var y = i + 2; y < _encoding.Length && sum < sumToFind; y++)
                {
                    sum = sum + _encoding[y];

                    if (sum == sumToFind)
                        return _encoding.Skip(i).Take(y + 1 - i).ToArray();
                }
            }

            throw new Exception($"Unable to locate contiguous block that sums to {sumToFind}");
        }

        public long GetFirstEncodingError(int preambleLength)
        {
            var preamble = _encoding.Take(preambleLength).ToArray();
            var sequence = _encoding.Skip(preambleLength).ToArray();

            var circularCache = new CircularCache(preamble);

            foreach (var token in sequence)
            {
                if (!TokenIsValid(circularCache.CacheItems, token))
                    return token;

                circularCache.Push(token);
            }

            throw new Exception("No invalid tokens encountered.");
        }

        public static bool TokenIsValid(long[] previousEntries, long currentEntry)
        {
            for (var x = 0; x < previousEntries.Length - 1; x++)
            {
                for (var y = x + 1; y < previousEntries.Length; y++)
                {
                    if (previousEntries[x] != previousEntries[y] && previousEntries[x] + previousEntries[y] == currentEntry)
                        return true;
                }
            }

            return false;
        }
    }
}
