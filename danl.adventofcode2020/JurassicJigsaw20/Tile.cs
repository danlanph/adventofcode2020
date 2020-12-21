using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace danl.adventofcode2020.JurassicJigsaw20
{
    public class Tile
    {
        public int Id { get; private set; }

        internal bool[,] _bitmap = new bool[10,10];

        public int TopBoundary { get; private set; }

        public int BottomBoundary { get; private set; }

        public int LeftBoundary { get; private set; }

        public int RightBoundary { get; private set; }

        public int[] Boundaries => new[] { TopBoundary, BottomBoundary, LeftBoundary, RightBoundary };

        public Tile(string tileString)
        {
            var tileDescriptionParts = tileString.Split(InputHelper.LineEnding, 2, StringSplitOptions.RemoveEmptyEntries);

            var tileHeaderMatch = Regex.Match(tileDescriptionParts[0], "^Tile\\s(?<tileId>\\d+):$");
            if (!tileHeaderMatch.Success)
                throw new ArgumentException($"'{nameof(tileString)}' does not contain a valid tile header.", nameof(tileString));

            Id = int.Parse(tileHeaderMatch.Groups["tileId"].Value);

            var y = 0;
            foreach (var row in tileDescriptionParts[1].Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries))
            {
                var x = 0;
                foreach (var bit in row)
                {
                    _bitmap[x, y] = bit == '#';
                    x++;
                }

                y++;
            }

            CalculateBoundaries();
        }

        private void CalculateBoundaries()
        {
            var tenRange = Enumerable.Range(0, 10).ToArray();

            TopBoundary = CalculateBoundary(tenRange.Select(x => _bitmap[x, 0]), 10);
            RightBoundary = CalculateBoundary(tenRange.Select(y => _bitmap[9, y]), 10);
            BottomBoundary = CalculateBoundary(tenRange.Select(x => _bitmap[x, 9]), 10);
            LeftBoundary = CalculateBoundary(tenRange.Select(y => _bitmap[0, y]), 10);
        }

        private int CalculateBoundary(IEnumerable<bool> boundaryBits, int length)
        {
            var orientation1 = 1U;
            var orientation2 = orientation1 << (length-1);

            var orientationValue1 = 0U;
            var orientationValue2 = 0U;

            for (var bitEnumerator = boundaryBits.GetEnumerator(); orientation2 > 0 && bitEnumerator.MoveNext(); orientation1 = orientation1 << 1, orientation2 = orientation2 >> 1)
            {
                if (!bitEnumerator.Current)
                    continue;

                orientationValue1 += orientation1;
                orientationValue2 += orientation2;
            }

            return (int)Math.Min(orientationValue1, orientationValue2);
        }

        public void Rotate()
        {
            var newBitmap = new bool[10, 10];

            for (var oldY = 0; oldY < 10; oldY++)
            {
                var newX = 9 - oldY;

                for (var oldXNewY = 0; oldXNewY < 10; oldXNewY++)
                {
                    newBitmap[newX, oldXNewY] = _bitmap[oldXNewY, oldY];
                }
            }

            _bitmap = newBitmap;

            var tmp = TopBoundary;
            TopBoundary = LeftBoundary;
            LeftBoundary = BottomBoundary;
            BottomBoundary = RightBoundary;
            RightBoundary = tmp;
        }

        public void Flip(FlipOrientation orientation)
        {
            Action<bool[,], bool[,], int, int> FlipHorizontal = (dest, src, y, x) => dest[x, 9 - y] = src[x,y];
            Action<bool[,], bool[,], int, int> FlipVertical = (dest, src, x, y) => dest[9 - x, y] = src[x, y];

            Action<bool[,], bool[,], int, int> FlipAction = orientation == FlipOrientation.Vertical ? FlipVertical : FlipHorizontal;

            var newBitmap = new bool[10, 10];
            for (var a = 0; a < 10; a++)
            {
                for (var b = 0; b < 10; b++)
                {
                    FlipAction(newBitmap, _bitmap, a, b);
                }
            }

            _bitmap = newBitmap;

            if (orientation == FlipOrientation.Vertical)
            {
                var tmp = LeftBoundary;
                LeftBoundary = RightBoundary;
                RightBoundary = tmp;
            }
            else
            {
                var tmp = TopBoundary;
                TopBoundary = BottomBoundary;
                BottomBoundary = tmp;
            }            
        }
    }

    public enum FlipOrientation
    {
        Vertical,
        Horizontal
    }
}
