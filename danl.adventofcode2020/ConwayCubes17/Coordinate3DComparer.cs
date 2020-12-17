using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace danl.adventofcode2020.ConwayCubes17
{
    public class Coordinate3DComparer : IEqualityComparer<Coordinate3D>
    {
        public bool Equals([AllowNull] Coordinate3D x, [AllowNull] Coordinate3D y)
        {
            return x.X == y.X
                && x.Y == y.Y
                && x.Z == y.Z;
        }

        public int GetHashCode([DisallowNull] Coordinate3D obj)
        {
            return 887 * obj.Z + 4933 * obj.Y + obj.X;
        }
    }
}