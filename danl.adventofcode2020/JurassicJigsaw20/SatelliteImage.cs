using System;
using System.Linq;
using System.Text;

namespace danl.adventofcode2020.JurassicJigsaw20
{
    public class SatelliteImage
    {
        internal bool[,] _image;
        internal int width;
        internal int height;

        public SatelliteImage(bool[][] completeImage)
        {
            if (completeImage == null)
                throw new ArgumentNullException(nameof(completeImage));

            width = completeImage[0].Length;
            height = completeImage.Length;

            _image = new bool[width, height];

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    _image[x, y] = completeImage[y][x];
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var y = 0; y < height; y++)
            {
                sb.AppendLine(string.Concat(Enumerable.Range(0, width).Select(x => _image[x, y]).Select(b => b ? '#' : '.')));
            }

            return sb.ToString();
        }

        public void Flip(FlipOrientation flipOrientation)
        {
            if (flipOrientation == FlipOrientation.Horizontal)
            {
                FlipHorizontal();
                return;
            }

            FlipVertical();
        }

        private void FlipHorizontal()
        {
            var newBitmap = new bool[width, height];

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    newBitmap[x, height - y - 1] = _image[x, y];
                }
            }

            _image = newBitmap;
        }

        private void FlipVertical()
        {
            var newBitmap = new bool[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    newBitmap[width-x-1,y] = _image[x, y];
                }
            }

            _image = newBitmap;
        }

        public void Rotate()
        {
            var newBitmap = new bool[height, width];

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    newBitmap[height - y - 1, x] = _image[x, y];
                }
            }

            _image = newBitmap;

            var tmp = width;
            width = height;
            height = tmp;
        }
    }
}