using System;
using System.Collections.Generic;
using System.Linq;

namespace danl.adventofcode2020.ConwayCubes17
{
    [Puzzle(puzzleNumber: 17, numberOfParts: 2)]
    public class ConwayCubes17
    {
        public const string InputFileResourceName = "danl.adventofcode2020.ConwayCubes17.input.txt";

        private bool[][] _initialState;

        public ConwayCubes17(string inputString)
        {
            _initialState = inputString
                .Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Select(c => c == '#').ToArray())
                .ToArray();
        }

        public static void Run(int part)
        {            
            var input = InputHelper.GetResourceFileAsString(InputFileResourceName);            

            var conwayCubes = new ConwayCubes17(input);

            switch (part)
            {
                case 1:
                    {
                        var numberOfActiveCubes = conwayCubes.GetNumberOfActiveCubesAfterCyclesIn3Dimensions(6);

                        Console.WriteLine($"Number of active cubes after 6 cycles is {numberOfActiveCubes}");
                        break;
                    }
                case 2:
                    {
                        var numberOfActiveCubes = conwayCubes.GetNumberOfActiveCubesAfterCyclesIn4Dimensions(6);
                        
                        Console.WriteLine($"Number of active cubes after 6 cycles is {numberOfActiveCubes}");
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        private int GetNumberOfActiveCubesAfterCyclesIn3Dimensions(int numberOfCycles)
        {
            var simulator = new ThreeDimensionalPocketSimulator(_initialState);

            while (numberOfCycles-- > 0)
                simulator.Cycle();

            var cubes = simulator.ActiveCubes.ToList();

            foreach (var cube in cubes)
                Console.WriteLine($"({cube.X},{cube.Y},{cube.Z})");

            return cubes.Count;
        }

        private int GetNumberOfActiveCubesAfterCyclesIn4Dimensions(int numberOfCycles)
        {
            var simulator = new FourDimensionalPocketSimulator(_initialState);

            while (numberOfCycles-- > 0)
                simulator.Cycle();

            var cubes = simulator.ActiveCubes.ToList();

            foreach (var cube in cubes)
                Console.WriteLine($"({cube.W},{cube.X},{cube.Y},{cube.Z})");

            return cubes.Count;
        }
    }
}