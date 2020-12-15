using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace danl.adventofcode2020.RambunctiousRecitation15
{
    public class Sequence
    {
        private int currentIndex = 0;

        private int lastTerm = 0;

        private readonly Hashtable _lastIndexes = new Hashtable();

        public int CurrentIndex => currentIndex;

        public int CurrentTerm => lastTerm;

        public Sequence(int[] startingTerms)
        {
            if (startingTerms == null || startingTerms.Length == 0)
                throw new ArgumentNullException(nameof(startingTerms));

            foreach (var term in startingTerms)
                pushTerm(term);
        }

        private int pushTerm(int term)
        {
            currentIndex++;
            lastTerm = term;

            if (!_lastIndexes.ContainsKey(term))
            {
                _lastIndexes[term] = new Pair { lastIndex = currentIndex };
                return currentIndex;
            }

            var pair = (Pair)_lastIndexes[term];
            pair.penultimateIndex = pair.lastIndex;
            pair.lastIndex = currentIndex;

            return currentIndex;
        }

        public int GenerateNextTerm()
        {
            var pair = (Pair)_lastIndexes[lastTerm];

            if (pair.penultimateIndex == 0)
                return pushTerm(0);

            return pushTerm(pair.lastIndex - pair.penultimateIndex);
        }

        public class Pair
        {
            public int penultimateIndex;
            public int lastIndex;
        }
    }
}
