using System;
using System.Collections.Generic;
using System.Linq;

namespace danl.adventofcode2020.DockingData14
{
    [Puzzle(puzzleNumber: 14, numberOfParts: 2)]
    public class DockingData14
    {
        public const string InputFileResourceName = "danl.adventofcode2020.DockingData14.input.txt";

        private readonly Instruction[] _instructions = new Instruction[0];

        public DockingData14(string inputString)
        {
            _instructions = inputString
                .Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries)
                .Select(Instruction.FromString)
                .ToArray();
        }

        public static void Run(int part)
        {            
            var input = InputHelper.GetResourceFileAsString(InputFileResourceName);            

            var dockingData = new DockingData14(input);

            switch (part)
            {
                case 1:
                case 2:
                    {
                        var memorySum = dockingData.RunProgramAndSumMemoryContents(part == 1 ? new SeaPortDecoderChipV1() : (IWriteDecoder)new SeaPortDecoderChipV2());

                        Console.WriteLine($"The sum of the memory contents is {memorySum}");
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        private ulong RunProgramAndSumMemoryContents(IWriteDecoder writeDecoder)
        {
            var seaPortComputer = new SeaPortComputer(writeDecoder);

            foreach (var instruction in _instructions)
                seaPortComputer.ExecuteInstruction(instruction);

            return seaPortComputer.Memory.Aggregate(0UL, (a,x) => a + x.Item2);
        }
    }
}