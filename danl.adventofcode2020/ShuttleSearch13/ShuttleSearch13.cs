using System;
using System.Collections.Generic;
using System.Linq;

namespace danl.adventofcode2020.ShuttleSearch13
{
    [Puzzle(puzzleNumber: 13, numberOfParts: 2)]
    public class ShuttleSearch13
    {
        public const string InputFileResourceName = "danl.adventofcode2020.ShuttleSearch13.input.txt";

        private long _timeOfArrival;

        private long[] _busIds;

        public ShuttleSearch13(string inputString)
        {
            var lines = inputString
                .Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries);

            _timeOfArrival = long.Parse(lines.First());
            _busIds = lines.Last().Split(',', StringSplitOptions.RemoveEmptyEntries)                           
                           .Select(id => id == "x" ? -1L : long.Parse(id))
                           .ToArray();
        }

        public static void Run(int part)
        {            
            var input = InputHelper.GetResourceFileAsString(InputFileResourceName);            

            var shuttleSearch = new ShuttleSearch13(input);

            switch (part)
            {
                case 1:
                    {
                        var busIdMultipliedByWaitingTime = shuttleSearch.GetBusIdMultipliedByWaitingTime();

                        Console.WriteLine($"Bus ID times Waiting Time is {busIdMultipliedByWaitingTime}");
                        break;
                    }
                case 2:
                    {
                        var timestamp = shuttleSearch.GetEarlistTimestampOfConsecutiveDepartures();

                        Console.WriteLine($"The earliest timestamp where all buses leave in consecutive minutes is {timestamp}");
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        private long GetBusIdMultipliedByWaitingTime()
        {
            var busIds = _busIds.Where(id => id != -1L).ToArray();
            var timeOfArrival = _timeOfArrival;
            var lowestCommonMultiple = Math.LowestCommonMultiple(busIds);
            timeOfArrival = timeOfArrival % lowestCommonMultiple;

            var earliestBus = busIds.Select(period =>
            {
                var remainder = timeOfArrival % period;
                var wait = (period - remainder) % period;                

                return new { BusId = period, Wait = wait };
            })
                .OrderBy(x => x.Wait)
                .First();

            return earliestBus.BusId * earliestBus.Wait;
        }

        private long GetEarlistTimestampOfConsecutiveDepartures()
        {
            var busIds = _busIds.Where(id => id != -1L).ToArray();

            if (!Math.AreCoprime(busIds))
                throw new NotImplementedException("Only co-prime bus ids are supported");

            var chineseRemainderTheoremEquations = _busIds
                .Select((x, minuteOffset) => x != -1 ? new { a = (x - (minuteOffset % x)) % x, modulus = x } : null)
                .Where(x => x != null)
                .ToArray();

            var values = chineseRemainderTheoremEquations.Select(x => x.a).ToArray();
            var moduli = chineseRemainderTheoremEquations.Select(x => x.modulus).ToArray();

            var solution = Math.GetChineseRemainderTheoremSolution(values, moduli);

            return solution;
        }
    }
}