using System;
using System.Collections.Generic;
using System.Linq;

namespace danl.adventofcode2020.AdapterArray10
{
    [Puzzle(puzzleNumber: 10, numberOfParts: 2)]
    public class AdapterArray10
    {
        public const string InputFileResourceName = "danl.adventofcode2020.AdapterArray10.input.txt";

        private readonly int[] _joltageAdapters;

        public AdapterArray10(string inputString)
        {
            _joltageAdapters = inputString
                .Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => int.Parse(i.Trim()))
                .ToArray();
        }

        public static void Run(int part)
        {            
            var input = InputHelper.GetResourceFileAsString(InputFileResourceName);            

            var adapterArray = new AdapterArray10(input);

            switch (part)
            {
                case 1:
                    {                        
                        var joltDifferences = adapterArray.GetJoltDifferences();                
                        Console.WriteLine($"The jolt differences are {string.Join(',', joltDifferences)}");

                        var numberOf1Differences = joltDifferences.Count(j => j == 1);
                        var numberOf3Differences = joltDifferences.Count(j => j == 3);

                        Console.WriteLine($"1 jolt differences: {numberOf1Differences}, 3 jolt differences: {numberOf3Differences}. Product is {numberOf1Differences * numberOf3Differences}");                        
                        break;
                    }
                case 2:
                    {                        
                        var joltDifferences = adapterArray.GetJoltDifferences();
                        var numberOfArrangements = adapterArray.GetNumberOfArrangements(joltDifferences);

                        Console.WriteLine($"The number of arrangements is {numberOfArrangements}");
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }            
        }

        public IList<int> GetJoltDifferences()
        {
            var maxJoltageAdapter = _joltageAdapters.Max();

            var joltDifferences = _joltageAdapters.Concat(new int[] { 0, maxJoltageAdapter + 3 })
                                    .OrderBy(x => x)
                                    .SelectTwo((x, y) => y - x)
                                    .ToList();

            return joltDifferences;
        }

        public long GetNumberOfArrangements(IList<int> joltDifferences)
        {
            return joltDifferences
                .Split(3)
                .Select(a => a.Length)
                .Select(l => GetNumberOfArrangementsForRunOf(l))
                .Aggregate(1L, (product, y) => product * y);
        }

        public int GetNumberOfArrangementsForRunOf(int numberOfOnesInSuccession)
        {
            switch (numberOfOnesInSuccession)
            {
                case 1:
                    return 1;
                case 2:
                    return 2;
                case 3:
                    return 4;
                case 4:
                    return 7;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
