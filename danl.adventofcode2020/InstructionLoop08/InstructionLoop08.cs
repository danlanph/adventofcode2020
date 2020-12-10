using System;
using System.Collections.Generic;
using System.Linq;

namespace danl.adventofcode2020.InstructionLoop08
{
    [Puzzle(puzzleNumber: 8, numberOfParts: 2)]
    public class InstructionLoop08
    {
        public const string InputFileResourceName = "danl.adventofcode2020.InstructionLoop08.input.txt";

        private readonly Instruction[] _instructions;

        public InstructionLoop08(string inputString)
        {
            _instructions = inputString
                .Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => new Instruction(i))
                .ToArray();
        }

        public static void Run(int part)
        {
            var instructionLoop = new InstructionLoop08(InputHelper.GetResourceFileAsString(InputFileResourceName));            

            switch (part)
            {
                case 1:
                    var accumulatorBeforeLoop = instructionLoop.GetAccumulatorAtFirstLoop();
                    Console.WriteLine($"Accumulator is {accumulatorBeforeLoop}.");
                    break;
                case 2:
                    var accumulatorAfterFixingCode = instructionLoop.GetAccumulatorAfterFixingCode();
                    Console.WriteLine($"Accumulator is {accumulatorAfterFixingCode}.");
                    break;
                default:
                    throw new NotImplementedException();
            }            
        }

        public int GetAccumulatorAfterFixingCode()
        {
            for (var i = 0; i < _instructions.Length; i++)
            {
                var instruction = _instructions[i];
                if (instruction.Operator == Operator.Jmp || instruction.Operator == Operator.Nop)
                {
                    var fixedInstructions = _instructions
                                                .Select(i => new Instruction { Operator = i.Operator, Operand = i.Operand })
                                                .ToArray();
                    
                    fixedInstructions[i].Operator = instruction.Operator == Operator.Jmp ? Operator.Nop : Operator.Jmp;

                    var processor = new InstructionProcessor(fixedInstructions);

                    if (ProgramHalts(processor))
                        return processor.Accumulator;
                }
            }

            throw new Exception("Unable to fix code");
        }

        public static bool ProgramHalts(InstructionProcessor processor)
        {
            var instructionExecutedTrackingArray = new bool[processor.Instructions.Length];
            instructionExecutedTrackingArray[processor.CurrentInstructionPointer] = true;

            while (!processor.Halted)
            {
                processor.Step();

                if (processor.Halted)
                    continue;

                if (instructionExecutedTrackingArray[processor.CurrentInstructionPointer])
                    return false;

                instructionExecutedTrackingArray[processor.CurrentInstructionPointer] = true;
            }

            return true;
        }

        public int GetAccumulatorAtFirstLoop()
        {
            var instructionExecutedTrackingArray = new bool[_instructions.Length];
            var instructionProcessor = new InstructionProcessor(_instructions);

            instructionExecutedTrackingArray[instructionProcessor.CurrentInstructionPointer] = true;

            while (!instructionProcessor.Halted)
            {
                instructionProcessor.Step();

                if (instructionExecutedTrackingArray[instructionProcessor.CurrentInstructionPointer])
                    return instructionProcessor.Accumulator;

                instructionExecutedTrackingArray[instructionProcessor.CurrentInstructionPointer] = true;
            }

            return instructionProcessor.Accumulator;
        }
    }
}
