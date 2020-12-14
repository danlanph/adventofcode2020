using System.Collections;

namespace danl.adventofcode2020.DockingData14
{
    public interface IWriteDecoder
    {
        void SetMask(string mask);

        void SetMemory(ulong address, ulong value, Hashtable memory);
    }
}
