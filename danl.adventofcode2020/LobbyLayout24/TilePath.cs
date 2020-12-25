using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace danl.adventofcode2020.LobbyLayout24
{
    public class TilePath
    {
        public Dictionary<PathDirection, Action<TileCoordinate>> Translations = new Dictionary<PathDirection, Action<TileCoordinate>>()
        {
            { PathDirection.NorthEast, (tc) => { tc.h = tc.h + 1; tc.z = tc.z + 1; } },
            { PathDirection.East, (tc) => { tc.h = tc.h + 2; } },
            { PathDirection.SouthEast, (tc) => { tc.h = tc.h + 1; tc.z = tc.z - 1; } },
            { PathDirection.NorthWest, (tc) => { tc.h = tc.h - 1; tc.z = tc.z + 1; } },
            { PathDirection.West, (tc) => { tc.h = tc.h - 2; } },
            { PathDirection.SouthWest, (tc) => { tc.h = tc.h - 1; tc.z = tc.z - 1; } }
        };

        private static readonly IEqualityComparer<TileCoordinate> _tileCordinateComparer = new TileCoordinateComparer();

        public IList<TileCoordinate> FlippedTiles { get; private set; }

        public TilePath()
        {
            FlippedTiles = new List<TileCoordinate>();
        }

        public void Flip(PathDirection[] vectors)
        {
            var tileCoordinate = new TileCoordinate { h = 0, z = 0 };

            foreach (var vector in vectors)
                Translations[vector](tileCoordinate);

            var flippedTile = FlippedTiles.FirstOrDefault(t => t.h == tileCoordinate.h && t.z == tileCoordinate.z);
            if (flippedTile == null)
            {
                FlippedTiles.Add(tileCoordinate);
            }
            else 
            {
                FlippedTiles.Remove(flippedTile);
            }
        }

        public void ApplyDailyTransformation()
        {
            var firstPassTiles = FlippedTiles.Select(t => new { Tile = t, AdjacentTiles = AdjacentTiles(t) }).ToArray();

            var neighboursOfBlackTiles = firstPassTiles
                                    .Select(x => x.AdjacentTiles.Select(y => new { Tile = x.Tile, AdjacentTile = y }))
                                    .SelectMany(f => f)
                                    .GroupBy(f => f.AdjacentTile, _tileCordinateComparer)
                                    .Select(t => new { Tile = t.Key, BlackAdjacentTileCount = t.Select(f => f.Tile).Distinct(_tileCordinateComparer).Count() });

            var whiteTilesWithBlackNeighbours = neighboursOfBlackTiles.Where(x => !FlippedTiles.Contains(x.Tile, _tileCordinateComparer)).ToArray();
            var blackTilesWithBlackNeighbours = firstPassTiles.Select(t => new
            {
                Tile = t.Tile,
                BlackAdjacentTileCount = t.AdjacentTiles.Where(n => FlippedTiles.Contains(n, _tileCordinateComparer)).Count()
            }).ToArray();

            var tilesToRemove = blackTilesWithBlackNeighbours.Where(t => t.BlackAdjacentTileCount == 0 || t.BlackAdjacentTileCount > 2).Select(t => t.Tile).ToArray();
            foreach (var tile in tilesToRemove)
                FlippedTiles.Remove(tile);

            var tilesToAdd = whiteTilesWithBlackNeighbours.Where(t => t.BlackAdjacentTileCount == 2).Select(t => t.Tile).ToArray();
            foreach (var tile in tilesToAdd)
                FlippedTiles.Add(tile);
        }

        public IEnumerable<TileCoordinate> AdjacentTiles(TileCoordinate tile)
        {
            return new[] { PathDirection.East, PathDirection.NorthEast , PathDirection.SouthEast,
            PathDirection.West, PathDirection.NorthWest, PathDirection.SouthWest }.Select(d =>
            {
                var adjacentTile = new TileCoordinate { h = tile.h, z = tile.z };
                Translations[d](adjacentTile);
                return adjacentTile;
            });
        }
    }

    public class TileCoordinateComparer : IEqualityComparer<TileCoordinate>
    {
        public bool Equals([AllowNull] TileCoordinate x, [AllowNull] TileCoordinate y)
        {
            if (x == null || y == null)
                return false;

            return x.h == y.h && x.z == y.z;                
        }

        public int GetHashCode([DisallowNull] TileCoordinate obj)
        {
            return (int)(10007 * obj.h + obj.z);
        }
    }

    public class TileCoordinate
    {
        public long h;
        public long z;
    }

    public enum PathDirection
    {
        East,
        SouthEast,
        SouthWest,
        West,
        NorthWest,
        NorthEast
    }
}
