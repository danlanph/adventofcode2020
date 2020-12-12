using System;
using System.Collections.Generic;
using System.Linq;

namespace danl.adventofcode2020.SeatingSystem11
{
    [Puzzle(puzzleNumber: 11, numberOfParts: 2)]
    public class SeatingSystem11
    {
        public const string InputFileResourceName = "danl.adventofcode2020.SeatingSystem11.input.txt";

        private readonly Grid _grid;

        public SeatingSystem11(string inputString)
        {
            var layout = inputString
                            .Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries)
                            .Select(
                                r => r.Select<char, Position>(s =>
                                {
                                    switch (s)
                                    {
                                        case 'L':
                                            return new Seat();
                                        case '.':
                                            return new Floor();
                                        default:
                                            throw new ArgumentOutOfRangeException();
                                    }
                                }).ToArray()
                            )
                            .ToArray();

            _grid = new Grid(layout);
        }

        public static void Run(int part)
        {
            var input = InputHelper.GetResourceFileAsString(InputFileResourceName);

            var seatingSystem = new SeatingSystem11(input);

            switch (part)
            {
                case 1:
                    {
                        var equilibrium = seatingSystem.GetEquilibrium(1, 4);
                        var numberOfOccupiedSeats = equilibrium.SelectMany(x => x).Count(s => s.Occupied);

                        Console.WriteLine($"The equilibrium point has {numberOfOccupiedSeats} occupied seats.");
                        break;
                    }
                case 2:
                    {
                        var equilibrium = seatingSystem.GetEquilibrium(null, 5);
                        var numberOfOccupiedSeats = equilibrium.SelectMany(x => x).Count(s => s.Occupied);

                        Console.WriteLine($"The equilibrium point has {numberOfOccupiedSeats} occupied seats.");
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        public Position[][] GetEquilibrium(int? sightDistance, int toggleThreshold)
        {
            var changes = GetChanges(_grid, sightDistance, toggleThreshold);

            while (changes.Length > 0)
            {
                ApplyChanges(_grid, changes);
                changes = GetChanges(_grid, sightDistance, toggleThreshold);
            }

            return _grid.Layout;
        }

        public Coordinate[] GetChanges(Grid grid, int? sightDistance, int toggleThreshold)
        {
            var changes = new List<Coordinate>();

            for (var x = 0; x < grid.NumberOfColumns; x++)
            {
                for (var y = 0; y < grid.NumberOfRows; y++)
                {
                    var position = grid.GetPosition(x, y);
                    if (position.CanOccupy)
                    {
                        var seatsInEachDirection = grid.AllPositionsInEachDirection(x, y)
                                                    .Select(d =>
                                                    {
                                                        var seatsInDirection = d.AsEnumerable();
                                                        if (sightDistance.HasValue)
                                                            seatsInDirection = seatsInDirection.Take(sightDistance.Value);

                                                        return seatsInDirection.Select(s => grid.GetPosition(s.x, s.y));
                                                    })
                                                    .Select(d => d.FirstOrDefault(s => s.CanOccupy))
                                                    .Where(p => p != null);

                        if (ShouldToggle(position, seatsInEachDirection, toggleThreshold))
                            changes.Add(new Coordinate { x = x, y = y });
                    }
                }
            }

            return changes.ToArray();
        }

        public bool ShouldToggle(Position currentPosition, IEnumerable<Position> adjacentPositions, int toggleThreshold)
        {
            var numberOfOccupiedAdjacentSeats = adjacentPositions.Count(p => p.Occupied);

            if (!currentPosition.Occupied && numberOfOccupiedAdjacentSeats == 0)
                return true;

            if (currentPosition.Occupied && numberOfOccupiedAdjacentSeats >= toggleThreshold)
                return true;

            return false;
        }

        public void ApplyChanges(Grid grid, Coordinate[] changes)
        {
            foreach (var change in changes)
            {
                var seat = grid.GetPosition(change.x, change.y) as Seat;
                if (seat != null)
                    seat.Toggle();
            }
        }
    }
}