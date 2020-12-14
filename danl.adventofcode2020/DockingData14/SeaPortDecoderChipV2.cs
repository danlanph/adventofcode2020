using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace danl.adventofcode2020.DockingData14
{
    public class SeaPortDecoderChipV2 : IWriteDecoder
    {
        public const ulong SystemMask = 0b0000000000000000000000000000_111111111111111111111111111111111111;
        private ulong _orMask;
        private string _mask;
        private int[] _floatingBits;

        public void SetMask(string mask)
        {
            _mask = mask;
            _orMask = Convert.ToUInt64(new string(mask.Select(c => c == '1' ? '1' : '0').ToArray()), 2) & SystemMask;
            _floatingBits = mask.Reverse()
                .Select((c, i) => new { index = i, digit = c })
                .Where(s => s.digit == 'X')
                .Select(s => s.index)
                .ToArray();
        }

        public void SetMemory(ulong address, ulong value, Hashtable memory)
        {
            address = (address & SystemMask) | _orMask;
            value = value & SystemMask;
            
            foreach (var writeAddress in GetAddresses(address, _floatingBits))
                memory[writeAddress] = value;
        }

        private static IEnumerable<ulong> GetAddresses(ulong baseAddress, int[] floatingBits)
        {
            if (floatingBits.Length == 0)
            {
                yield return baseAddress;
                yield break;
            }

            var allValues = 1 << floatingBits.Length;

            for (uint value = 0; value < allValues; value++)
            {
                var address = baseAddress;
                var index = 0;
                foreach (var bit in GetBits(value, floatingBits.Length))
                    address = SetBit(address, floatingBits[index++], bit);

                yield return address;
            }
        }

        private static IEnumerable<uint> GetBits(uint number, int bitLength)
        {
            var mask = 1U;

            var c = number & mask;
            while (bitLength-- > 0)
            {
                yield return c;
                number = number >> 1;
                c = number & mask;
            }
        }

        private static ulong SetBit(ulong value, int bitNumber, uint bitValue)
        {
            var mask = 1UL << bitNumber;

            if (bitValue == 1)
                return value | mask;

            mask = ulong.MaxValue - mask;
            return value & mask;
        }
    }
}
