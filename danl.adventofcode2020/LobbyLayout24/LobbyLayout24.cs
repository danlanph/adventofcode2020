using System;
using System.Collections.Generic;
using System.Linq;

namespace danl.adventofcode2020.LobbyLayout24
{
    [Puzzle(puzzleNumber: 24, numberOfParts: 2)]
    public class LobbyLayout24
    {
        public const string InputFileResourceName = "danl.adventofcode2020.LobbyLayout24.input.txt";

        private PathDirection[][] _paths;

        public LobbyLayout24(string inputString)
        {
            _paths = inputString.Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => PathVectors(s).ToArray())
                .ToArray();
        }

        public static void Run(int part)
        {
            var input = InputHelper.GetResourceFileAsString(InputFileResourceName);            

            var lobbyLayout = new LobbyLayout24(input);

            switch (part)
            {
                case 1:
                    {
                        var blackTiles = lobbyLayout.GetNumberOfBlackTilesAfterDay(0);
                        Console.WriteLine($"Number of black tiles is : {blackTiles}.");

                        break;
                    }
                case 2:
                    {
                        var blackTiles = lobbyLayout.GetNumberOfBlackTilesAfterDay(100);
                        Console.WriteLine($"Number of black tiles is : {blackTiles}.");

                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        private static IEnumerable<PathDirection> PathVectors(string path)
        {
            path = path.ToUpperInvariant();
            var i = 0;
            while (i < path.Length)
            {
                var peek = path[i++];

                switch (peek)
                {
                    case 'E':
                        yield return PathDirection.East;
                        break;
                    case 'W':
                        yield return PathDirection.West;
                        break;
                    case 'N':
                        peek = path[i++];
                        switch (peek)
                        {
                            case 'E':
                                yield return PathDirection.NorthEast;
                                break;
                            case 'W':
                                yield return PathDirection.NorthWest;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    case 'S':
                        peek = path[i++];
                        switch (peek)
                        {
                            case 'E':
                                yield return PathDirection.SouthEast;
                                break;
                            case 'W':
                                yield return PathDirection.SouthWest;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        private int GetNumberOfBlackTilesAfterDay(int days)
        {
            var tilePath = new TilePath();

            foreach (var path in _paths)
                tilePath.Flip(path);

            while (days-- > 0)
                tilePath.ApplyDailyTransformation();

            return tilePath.FlippedTiles.Count;
        }
    }
}