using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace danl.adventofcode2020.DockingData14
{
    public class SeaPortComputer
    {
        private readonly Hashtable _memory = new Hashtable();        

        private readonly IWriteDecoder _writeDecoder;

        public SeaPortComputer(IWriteDecoder writeDecoder)
        {
            if (writeDecoder == null)
                throw new ArgumentNullException(nameof(IWriteDecoder));

            _writeDecoder = writeDecoder;
        }

        public void ExecuteInstruction(Instruction instruction)
        {
            switch (instruction.Operator)
            {
                case Operator.SetMask:
                    _writeDecoder.SetMask(instruction.Operand1 as string);                    
                    break;
                case Operator.SetMemory:
                    _writeDecoder.SetMemory((ulong)instruction.Operand1, (ulong)instruction.Operand2, _memory);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public IEnumerable<Tuple<ulong, ulong>> Memory
        {
            get
            {
                foreach (ulong address in _memory.Keys)
                    yield return new Tuple<ulong, ulong>(address, (ulong)_memory[address]);
            }
        }
    }

    public class Instruction
    {
        public Operator Operator { get; set; }

        public object Operand1 { get; set; }

        public object Operand2 { get; set; }

        public static Instruction FromString(string instructionString)
        {
            var instructionParts = instructionString.Split('=', 2, StringSplitOptions.RemoveEmptyEntries);
            var instruction = instructionParts[0].Trim();
            var value = instructionParts[1].Trim();

            if (instruction.Equals("mask", StringComparison.InvariantCultureIgnoreCase))
            {
                return new Instruction
                {
                    Operator = Operator.SetMask,
                    Operand1 = value
                };
            }

            if (instruction.StartsWith("mem["))
            {
                var endBracketIndex = instruction.IndexOf(']');
                var address = ulong.Parse(instruction.Substring(4, endBracketIndex - 4));
                var numericValue = ulong.Parse(value);

                return new Instruction
                {
                    Operator = Operator.SetMemory,
                    Operand1 = address,
                    Operand2 = numericValue
                };
            }

            throw new ArgumentException("Invalid instruction", nameof(instructionString));
        }
    }

    public enum Operator
    {
        SetMask,
        SetMemory
    }
}
