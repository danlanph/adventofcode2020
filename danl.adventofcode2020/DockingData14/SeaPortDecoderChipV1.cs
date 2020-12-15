using System;
using System.Collections;
using System.Linq;

namespace danl.adventofcode2020.DockingData14
{
    public class SeaPortDecoderChipV1 : IWriteDecoder
    {
        public const ulong SystemMask = 0b0000000000000000000000000000_111111111111111111111111111111111111;
        private ulong _orMask = 0b0000000000000000000000000000_000000000000000000000000000000000000;
        private ulong _andMask = 0b1111111111111111111111111111_111111111111111111111111111111111111;

        public void SetMask(string mask)
        {
            var setZeroMask = Convert.ToUInt64(new string(mask.Select(c => c == '0' ? '0' : '1').ToArray()), 2);
            var setOneMask = Convert.ToUInt64(new string(mask.Select(c => c == '1' ? '1' : '0').ToArray()), 2);

            _orMask = setOneMask & SystemMask;
            _andMask = setZeroMask & SystemMask;
        }

        public void SetMemory(ulong address, ulong value, Hashtable memory)
        {
            address = address & SystemMask;
            value = value & SystemMask;

            var maskedValue = (value & _andMask) | _orMask;
            memory[address] = maskedValue;
        }
    }
}
