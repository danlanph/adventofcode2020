using System;
using System.Collections.Generic;
using System.Text;

namespace danl.adventofcode2020.InstructionLoop08
{
    public class Instruction
    {
        public static readonly Dictionary<string, Operator> InstructionLookup = new Dictionary<string, Operator>() {
            {"acc", Operator.Acc },
            {"jmp", Operator.Jmp },
            {"nop", Operator.Nop }
        };

        public Instruction() { }

        public Instruction(string instruction)
        {
            var parts = instruction.Split(' ');

            Operator = InstructionLookup[parts[0].ToLowerInvariant()];
            Operand = int.Parse(parts[1]);
        }

        public Operator Operator { get; set; }
        public int Operand { get; set; }
    }

    public enum Operator
    {
        Acc,
        Jmp,
        Nop
    }
}
