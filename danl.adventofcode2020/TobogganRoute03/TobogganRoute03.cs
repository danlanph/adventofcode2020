using System;
using System.Linq;

namespace danl.adventofcode2020.TobogganRoute03
{
    [Puzzle(puzzleNumber: 3, numberOfParts: 2)]
    public class TobogganRoute03
    {
        public const string InputFileResourceName = "danl.adventofcode2020.TobogganRoute03.input.txt";

        private readonly TobogganMap _tobogganMap;

        public TobogganRoute03(string inputString)
        {
            var mapRows = inputString
                            .Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Trim())
                            .ToArray();

            _tobogganMap = new TobogganMap(mapRows);
        }

        public static void Run(int part)
        {
            var tobogganRoute = new TobogganRoute03(InputHelper.GetResourceFileAsString(InputFileResourceName));            

            switch (part)
            {
                case 1:
                    {
                        var numberOfTrees = tobogganRoute.GetNumberOfTreesEncountered(new Direction { x = 3, y = 1 });
                        Console.WriteLine($"{numberOfTrees} trees encountered.");
                        break;
                    }
                case 2:
                    {
                        var slopes = new[] {
                            new Direction { x = 1, y = 1 },
                            new Direction { x = 3, y = 1 },
                            new Direction { x = 5, y = 1 },
                            new Direction { x = 7, y = 1 },
                            new Direction { x = 1, y = 2 },
                        };

                        Console.WriteLine($"Slope {slopes[0].x},{slopes[0].y} encounters {tobogganRoute.GetNumberOfTreesEncountered(slopes[0])} trees.");
                        Console.WriteLine($"Slope {slopes[1].x},{slopes[1].y} encounters {tobogganRoute.GetNumberOfTreesEncountered(slopes[1])} trees.");
                        Console.WriteLine($"Slope {slopes[2].x},{slopes[2].y} encounters {tobogganRoute.GetNumberOfTreesEncountered(slopes[2])} trees.");
                        Console.WriteLine($"Slope {slopes[3].x},{slopes[3].y} encounters {tobogganRoute.GetNumberOfTreesEncountered(slopes[3])} trees.");
                        Console.WriteLine($"Slope {slopes[4].x},{slopes[4].y} encounters {tobogganRoute.GetNumberOfTreesEncountered(slopes[4])} trees.");

                        var numberOfTreesForDifferentSlopesMultiplied = slopes.Aggregate(1l, 
                            (p, slope) =>
                            {
                                return p * tobogganRoute.GetNumberOfTreesEncountered(slope);
                            });
                        Console.WriteLine($"Product of trees encountered for each slope is {numberOfTreesForDifferentSlopesMultiplied}.");
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }            
        }

        public long GetNumberOfTreesEncountered(Direction direction)
        {
            long treesEncountered = 0;
            _tobogganMap.Reset(0, 0);

            while (_tobogganMap.HasMoreRows())
            {
                _tobogganMap.Move(direction);

                if (_tobogganMap.IsTree)
                    treesEncountered++;
            }

            return treesEncountered;
        }
    }
}
