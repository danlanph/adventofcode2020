using System;
using System.Linq;

namespace danl.adventofcode2020.SeatFinder05
{
    [Puzzle(puzzleNumber: 5, numberOfParts: 2)]
    public class SeatFinder05
    {
        public const string InputFileResourceName = "danl.adventofcode2020.SeatFinder05.input.txt";

        private readonly Seat[] Seats;

        public SeatFinder05(string inputString)
        {
            Seats = inputString
                .Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => new Seat(s))
                .ToArray();
        }

        public static void Run(int part)
        {
            var seatFinder = new SeatFinder05(InputHelper.GetResourceFileAsString(InputFileResourceName));            

            switch (part)
            {
                case 1:
                    var highestSeatId = seatFinder.Seats.Max(x => x.Id);
                    Console.WriteLine($"The highest seat Id is {highestSeatId}.");
                    break;
                case 2:
                    var missingSeatId = seatFinder.GetMissingSeatId();
                    Console.WriteLine($"The missing seat Id is {missingSeatId}.");
                    break;
                default:
                    throw new NotImplementedException();
            }            
        }

        public int GetMissingSeatId()
        {
            var seatList = Seats.OrderBy(x => x.Id).ToList();
            var lastSeatId = seatList.First().Id;

            foreach (var seat in seatList.Skip(1))
            {
                if (lastSeatId + 1 < seat.Id)
                    return lastSeatId + 1;

                lastSeatId = seat.Id;
            }

            return -1;
        }

    }
}
