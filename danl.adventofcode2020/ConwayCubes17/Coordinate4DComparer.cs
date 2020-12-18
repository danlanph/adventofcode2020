using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace danl.adventofcode2020.ConwayCubes17
{
    public class Coordinate4DComparer : IEqualityComparer<Coordinate4D>
    {
        public bool Equals([AllowNull] Coordinate4D x, [AllowNull] Coordinate4D y)
        {
            return x.W == y.W
                && x.X == y.X
                && x.Y == y.Y
                && x.Z == y.Z;
        }

        public int GetHashCode([DisallowNull] Coordinate4D obj)
        {
            return 104729*obj.W + 887 * obj.Z + 4933 * obj.Y + obj.X;
        }
    }
}