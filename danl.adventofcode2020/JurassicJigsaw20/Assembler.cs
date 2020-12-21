using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace danl.adventofcode2020.JurassicJigsaw20
{
    public class Assembler
    {
        private readonly ImageRegion[] _regions;

        public Assembler(Tile[] tiles)
        {
            if (tiles == null || tiles.Length == 0)
                throw new ArgumentNullException(nameof(tiles));

            _regions = tiles
                .Select(tile => new ImageRegion(tile))
                .ToArray();
        }

        public void Assemble()
        {
            var coreAnchor = _regions.First();
            AttachAdjacentRegions(coreAnchor);
        }

        public IEnumerable<ImageRegion> GetCorners()
        {
            foreach (var region in _regions)
            {
                var numberOfUnmatchedSides = region.Tile.Boundaries
                    .Select(x => new { BoundaryCount = GetRegionsWithBoundary(x).Count() })
                    .Where(x => x.BoundaryCount == 1)
                    .Count();

                if (numberOfUnmatchedSides == 2)
                    yield return region;
            }
        }

        private void AttachAdjacentRegions(ImageRegion region)
        {
            region.Used = true;

            if (!region.Top.Set)
            {
                var adjacentRegion = GetRegionsWithBoundary(region.Tile.TopBoundary)
                                        .Where(r => r.Tile.Id != region.Tile.Id)
                                        .FirstOrDefault();

                if (adjacentRegion == null)
                {
                    region.Top.Set = true;
                }
                else
                {
                    JoinAdjacentRegions(region, adjacentRegion, JoinDirection.Top);
                    AttachAdjacentRegions(adjacentRegion);
                }
            }

            if (!region.Right.Set)
            {
                var adjacentRegion = GetRegionsWithBoundary(region.Tile.RightBoundary)
                                        .Where(r => r.Tile.Id != region.Tile.Id)
                                        .FirstOrDefault();

                if (adjacentRegion == null)
                {
                    region.Right.Set = true;
                }
                else
                {
                    JoinAdjacentRegions(region, adjacentRegion, JoinDirection.Right);
                    AttachAdjacentRegions(adjacentRegion);
                }
            }

            if (!region.Bottom.Set)
            {
                var adjacentRegion = GetRegionsWithBoundary(region.Tile.BottomBoundary)
                                        .Where(r => r.Tile.Id != region.Tile.Id)
                                        .FirstOrDefault();

                if (adjacentRegion == null)
                {
                    region.Bottom.Set = true;
                }
                else
                {
                    JoinAdjacentRegions(region, adjacentRegion, JoinDirection.Bottom);
                    AttachAdjacentRegions(adjacentRegion);
                }
            }

            if (!region.Left.Set)
            {
                var adjacentRegion = GetRegionsWithBoundary(region.Tile.LeftBoundary)
                                        .Where(r => r.Tile.Id != region.Tile.Id)
                                        .FirstOrDefault();

                if (adjacentRegion == null)
                {
                    region.Left.Set = true;
                }
                else
                {
                    JoinAdjacentRegions(region, adjacentRegion, JoinDirection.Left);
                    AttachAdjacentRegions(adjacentRegion);
                }
            }
        }

        private void JoinAdjacentRegions(ImageRegion region1, ImageRegion region2, JoinDirection joinDirection)
        {
            switch (joinDirection)
            {
                case JoinDirection.Top:
                    if (region2.Tile.BottomBoundary != region1.Tile.TopBoundary)
                    {
                        if (region2.Tile.LeftBoundary == region1.Tile.TopBoundary)
                            RotateTile(region2.Tile, 3);

                        if (region2.Tile.TopBoundary == region1.Tile.TopBoundary)
                            RotateTile(region2.Tile, 2);

                        if (region2.Tile.RightBoundary == region1.Tile.TopBoundary)
                            RotateTile(region2.Tile, 1);
                    }
                    if (!isCorrectOrientation(region1.Tile, region2.Tile, joinDirection))
                        Flip(region2.Tile, FlipOrientation.Vertical);

                    region1.Top.Set = true;
                    region1.Top.Region = region2;

                    region2.Bottom.Set = true;
                    region2.Bottom.Region = region1;

                    JoinToUsedAdjacentRegions(region2);

                    break;

                case JoinDirection.Bottom:
                    if (region2.Tile.TopBoundary != region1.Tile.BottomBoundary)
                    {
                        if (region2.Tile.LeftBoundary == region1.Tile.BottomBoundary)
                            RotateTile(region2.Tile, 1);

                        if (region2.Tile.BottomBoundary == region1.Tile.BottomBoundary)
                            RotateTile(region2.Tile, 2);

                        if (region2.Tile.RightBoundary == region1.Tile.BottomBoundary)
                            RotateTile(region2.Tile, 3);
                    }
                    if (!isCorrectOrientation(region1.Tile, region2.Tile, joinDirection))
                        Flip(region2.Tile, FlipOrientation.Vertical);

                    region1.Bottom.Set = true;
                    region1.Bottom.Region = region2;

                    region2.Top.Set = true;
                    region2.Top.Region = region1;

                    JoinToUsedAdjacentRegions(region2);

                    break;

                case JoinDirection.Left:
                    if (region2.Tile.RightBoundary != region1.Tile.LeftBoundary)
                    {
                        if (region2.Tile.LeftBoundary == region1.Tile.LeftBoundary)
                            RotateTile(region2.Tile, 2);

                        if (region2.Tile.BottomBoundary == region1.Tile.LeftBoundary)
                            RotateTile(region2.Tile, 3);

                        if (region2.Tile.TopBoundary == region1.Tile.LeftBoundary)
                            RotateTile(region2.Tile, 1);
                    }
                    if (!isCorrectOrientation(region1.Tile, region2.Tile, joinDirection))
                        Flip(region2.Tile, FlipOrientation.Horizontal);

                    region1.Left.Set = true;
                    region1.Left.Region = region2;

                    region2.Right.Set = true;
                    region2.Right.Region = region1;

                    JoinToUsedAdjacentRegions(region2);

                    break;

                case JoinDirection.Right:
                    if (region2.Tile.LeftBoundary != region1.Tile.RightBoundary)
                    {
                        if (region2.Tile.RightBoundary == region1.Tile.RightBoundary)
                            RotateTile(region2.Tile, 2);

                        if (region2.Tile.BottomBoundary == region1.Tile.RightBoundary)
                            RotateTile(region2.Tile, 1);

                        if (region2.Tile.TopBoundary == region1.Tile.RightBoundary)
                            RotateTile(region2.Tile, 3);
                    }
                    if (!isCorrectOrientation(region1.Tile, region2.Tile, joinDirection))
                        Flip(region2.Tile, FlipOrientation.Horizontal);

                    region1.Right.Set = true;
                    region1.Right.Region = region2;

                    region2.Left.Set = true;
                    region2.Left.Region = region1;

                    JoinToUsedAdjacentRegions(region2);

                    break;
            }
        }

        private void JoinToUsedAdjacentRegions(ImageRegion region)
        {
            if (!region.Top.Set)
            {
                var tileAbove = _regions
                    .Where(r => r.Used)
                    .Where(r => r.Tile.BottomBoundary == region.Tile.TopBoundary)
                    .FirstOrDefault();

                if (tileAbove != null)
                {
                    region.Top.Set = true;
                    region.Top.Region = tileAbove;

                    tileAbove.Bottom.Set = true;
                    tileAbove.Bottom.Region = region;
                }
            }

            if (!region.Bottom.Set)
            {
                var tileBelow = _regions
                    .Where(r => r.Used)
                    .Where(r => r.Tile.TopBoundary == region.Tile.BottomBoundary)
                    .FirstOrDefault();

                if (tileBelow != null)
                {
                    region.Bottom.Set = true;
                    region.Bottom.Region = tileBelow;

                    tileBelow.Top.Set = true;
                    tileBelow.Top.Region = region;
                }
            }

            if (!region.Right.Set)
            {
                var tileToRight = _regions
                    .Where(r => r.Used)
                    .Where(r => r.Tile.LeftBoundary == region.Tile.RightBoundary)
                    .FirstOrDefault();

                if (tileToRight != null)
                {
                    region.Right.Set = true;
                    region.Right.Region = tileToRight;

                    tileToRight.Left.Set = true;
                    tileToRight.Left.Region = region;
                }
            }

            if (!region.Left.Set)
            {
                var tileToLeft = _regions
                    .Where(r => r.Used)
                    .Where(r => r.Tile.RightBoundary == region.Tile.LeftBoundary)
                    .FirstOrDefault();

                if (tileToLeft != null)
                {
                    region.Left.Set = true;
                    region.Left.Region = tileToLeft;

                    tileToLeft.Right.Set = true;
                    tileToLeft.Right.Region = region;
                }
            }
        }

        private bool isCorrectOrientation(Tile tile1, Tile tile2, JoinDirection joinDirection)
        {
            switch (joinDirection)
            {
                case JoinDirection.Top:
                    for (var i = 0; i < 10; i++)
                    {
                        if (tile1._bitmap[i, 0] != tile2._bitmap[i, 9])
                            return false;
                    }
                    return true;
                case JoinDirection.Bottom:
                    for (var i = 0; i < 10; i++)
                    {
                        if (tile1._bitmap[i, 9] != tile2._bitmap[i, 0])
                            return false;
                    }
                    return true;
                case JoinDirection.Left:
                    for (var i = 0; i < 10; i++)
                    {
                        if (tile1._bitmap[0, i] != tile2._bitmap[9, i])
                            return false;
                    }
                    return true;
                case JoinDirection.Right:
                    for (var i = 0; i < 10; i++)
                    {
                        if (tile1._bitmap[9, i] != tile2._bitmap[0, i])
                            return false;
                    }
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RotateTile(Tile tile, int timesToRotate)
        {
            while (timesToRotate-- > 0)
                tile.Rotate();
        }

        private void Flip(Tile tile, FlipOrientation flipOrientation)
        {
            tile.Flip(flipOrientation);
        }

        private IEnumerable<ImageRegion> GetRegionsWithBoundary(int boundary)
        {
            return _regions
                .Select(r => r.Tile.Boundaries.Select(b => new { Region = r, Boundary = b }))
                .SelectMany(r => r)
                .Where(r => r.Boundary == boundary)
                .Select(r => r.Region);
        }

        public Image GetImage()
        {
            var top = _regions.First();
            while (top.Left.Region != null)
                top = top.Left.Region;

            while (top.Top.Region != null)
                top = top.Top.Region;

            var imageList = new List<IList<Tile>>();

            while (top != null)
            {
                var left = top;
                var row = new List<Tile>();
                while (left != null)
                {
                    row.Add(left.Tile);
                    left = left.Right.Region;
                }
                imageList.Add(row);
                top = top.Bottom.Region;
            }
            
            return new Image(imageList);
        }
    }

    public class ImageRegion
    {
        public Tile Tile { get; private set; }

        internal bool Used { get; set; }

        public ImageRegion(Tile tile)
        {
            if (tile == null)
                throw new ArgumentNullException(nameof(tile));

            Tile = tile;
            Left = new AdjacentRegion { Set = false, Region = null };
            Top = new AdjacentRegion { Set = false, Region = null };
            Right = new AdjacentRegion { Set = false, Region = null };
            Bottom = new AdjacentRegion { Set = false, Region = null };
            Used = false;
        }

        public AdjacentRegion Left { get; private set; }
        public AdjacentRegion Top { get; private set; }
        public AdjacentRegion Right { get; private set; }
        public AdjacentRegion Bottom { get; private set; }
    }

    public class AdjacentRegion
    {
        public bool Set;
        public ImageRegion Region;
    }

    public enum JoinDirection
    {
        Top,
        Bottom,
        Right,
        Left
    }
}
