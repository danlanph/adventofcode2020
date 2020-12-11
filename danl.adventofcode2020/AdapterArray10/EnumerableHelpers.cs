using System;
using System.Collections.Generic;
using System.Linq;

namespace danl.adventofcode2020.AdapterArray10
{
    public static class EnumerableHelpers
    {
        public static IEnumerable<TResult> SelectTwo<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TResult> selector)
        {
            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                    yield break;

                var item1 = sourceIterator.Current;

                while (sourceIterator.MoveNext())
                {
                    var item2 = sourceIterator.Current;
                    yield return selector(item1, item2);
                    item1 = item2;
                }
            }
        }

        public static IEnumerable<TSource[]> Split<TSource>(this IEnumerable<TSource> source, TSource splitValue)
        {
            using (var sourceIterator = source.GetEnumerator())
            {
                while (sourceIterator.MoveNext())
                {
                    var subArray = SplitEnumerate(sourceIterator, splitValue).ToArray();
                    if (subArray.Length > 0)
                        yield return subArray;
                }                
            }
        }

        private static IEnumerable<TSource> SplitEnumerate<TSource>(IEnumerator<TSource> enumerator, TSource splitValue)
        {
            if (EqualityComparer<TSource>.Default.Equals(enumerator.Current, splitValue))
                yield break;

            yield return enumerator.Current;

            while (enumerator.MoveNext() && !EqualityComparer<TSource>.Default.Equals(enumerator.Current, splitValue))
            {
                yield return enumerator.Current;
            }
        }
    }
}
