using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace danl.adventofcode2020.TobogganRoute03
{
    public class TobogganMap
    {
        private const char TreeSymbol = '#';
        private readonly bool[][] _mapLayout;
        private readonly int _rowLength;

        public int X { get; private set; }
        public int Y { get; private set; }

        public TobogganMap(string[] mapRowStrings)
        {
            X = Y = 0;
            _mapLayout = mapRowStrings.Select(RowFromString).ToArray();
            _rowLength = _mapLayout.First().Length;

            if (!_mapLayout.All(row => row.Length == _rowLength))
                throw new ArgumentException("Not all rows are the same length", nameof(mapRowStrings));
        }

        public static bool[] RowFromString(string rowString)
        {
            return rowString.Select(c => c == TreeSymbol).ToArray();
        }

        public void Reset(int x, int y)
        {
            if (x < 0 || x >= _rowLength)
                throw new ArgumentOutOfRangeException(nameof(x));

            if (y < 0 || y >= _mapLayout.Length)
                throw new ArgumentOutOfRangeException(nameof(y));

            X = x;
            Y = y;
        }

        public bool HasMoreRows()
        {
            return Y < _mapLayout.Length - 1;
        }

        public void Move(Direction direction)
        {
            X = (X + direction.x) % _rowLength;
            Y += direction.y;
        }

        public bool IsTree
        {
            get
            {
                return Y < _mapLayout.Length && _mapLayout[Y][X];
            }
        }
    }
}
