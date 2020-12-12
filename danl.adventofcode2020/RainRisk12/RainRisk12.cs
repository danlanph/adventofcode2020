using System;
using System.Collections.Generic;
using System.Linq;

namespace danl.adventofcode2020.RainRisk12
{
    [Puzzle(puzzleNumber: 12, numberOfParts: 2)]
    public class RainRisk12
    {
        public const string InputFileResourceName = "danl.adventofcode2020.RainRisk12.input.txt";

        private Movement[] _movements;

        public RainRisk12(string inputString)
        {
            _movements = inputString
                .Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries)
                .Select(l => new Movement(l))
                .ToArray();
        }

        public static void Run(int part)
        {
            var input = InputHelper.GetResourceFileAsString(InputFileResourceName);            

            var rainRisk = new RainRisk12(input);

            switch (part)
            {
                case 1:
                    {
                        var distanceFromStart = rainRisk.GetManhattanDistanceFromStartingPosition(new Ship());

                        Console.WriteLine($"The manhattan distamce from the start is {distanceFromStart}.");
                        break;
                    }
                case 2:
                    {
                        var distanceFromStart = rainRisk.GetManhattanDistanceFromStartingPosition(new WaypointShip());

                        Console.WriteLine($"The manhattan distamce from the start is {distanceFromStart}.");
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        public int GetManhattanDistanceFromStartingPosition(IShip ship)
        {
            foreach (var movement in _movements)
                ship.Move(movement);

            return Math.Abs(ship.X) + Math.Abs(ship.Y);
        }
    }
}