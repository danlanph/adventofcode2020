using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace danl.adventofcode2020.JurassicJigsaw20
{
    public class Image
    {
        private Tile[][] _tiles;

        public Image(IList<IList<Tile>> imageList)
        {
            _tiles = imageList
                .Select(row => row.ToArray())
                .ToArray();
        }

        public IEnumerable<Tile> CornerTiles
        {
            get
            {
                var width = _tiles[0].Length;
                var height = _tiles.Length;

                yield return _tiles[0][0];
                yield return _tiles[0][width - 1];
                yield return _tiles[height - 1][width - 1];
                yield return _tiles[height - 1][0];
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
       
            foreach (var row in _tiles)
            {
                sb.AppendLine(string.Join(' ', row.Select(t => t.Id)));
            }

            return sb.ToString();
        }

        public SatelliteImage Render()
        {
            return new SatelliteImage(GetRows().Select(row => row.ToArray()).ToArray());
        }

        private IEnumerable<IEnumerable<bool>> GetRows()
        {
            foreach (var tileRow in _tiles)
            {
                for (var i = 1; i < 9; i++)
                {
                    yield return GetRow(tileRow, i);
                }
            }
        }

        private IEnumerable<bool> GetRow(Tile[] tileRow, int i)
        {
            foreach (var tile in tileRow)
            {
                for (var j = 1; j < 9; j++)
                {
                    yield return tile._bitmap[j, i];
                }
            }
        }
    }
}
