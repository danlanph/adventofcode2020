using System;
using System.Collections.Generic;
using System.Text;

namespace danl.adventofcode2020.InstructionLoop08
{
    public class InstructionProcessor
    {
        public int CurrentInstructionPointer { get; private set; }

        public int Accumulator { get; private set; }

        public readonly Instruction[] Instructions;

        public bool Halted { get; private set; }

        public InstructionProcessor(Instruction[] instructions)
        {
            CurrentInstructionPointer = 0;
            Accumulator = 0;
            Instructions = instructions;
            Halted = false;
        }

        public void Step()
        {
            if (Halted)
                return;

            var instruction = Instructions[CurrentInstructionPointer];

            switch (instruction.Operator)
            {
                case Operator.Acc:
                    Accumulator += instruction.Operand;
                    CurrentInstructionPointer++;
                    break;
                case Operator.Jmp:
                    CurrentInstructionPointer += instruction.Operand;
                    break;
                case Operator.Nop:
                    CurrentInstructionPointer++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (CurrentInstructionPointer == Instructions.Length)
            {
                Halted = true;
            }
        }
    }
}
