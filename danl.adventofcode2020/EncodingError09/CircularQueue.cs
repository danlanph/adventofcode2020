using System;

namespace danl.adventofcode2020.EncodingError09
{
    public class CircularCache
    {
        public long[] CacheItems { get; private set; }

        private int _startIndex;

        public CircularCache(long[] startingState)
        {
            if (startingState == null || startingState.Length == 0)
                throw new ArgumentNullException(nameof(startingState));

            CacheItems = new long[startingState.Length];
            startingState.CopyTo(CacheItems, 0);
            _startIndex = 0;
        }

        public void Push(long newEntry)
        {
            CacheItems[_startIndex] = newEntry;
            _startIndex = (_startIndex + 1) % CacheItems.Length;
        }
    }
}
