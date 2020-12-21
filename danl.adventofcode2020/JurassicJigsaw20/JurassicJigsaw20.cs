using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace danl.adventofcode2020.JurassicJigsaw20
{
    [Puzzle(puzzleNumber: 20, numberOfParts: 2)]
    public class JurassicJigsaw20
    {
        public const string InputFileResourceName = "danl.adventofcode2020.JurassicJigsaw20.input.txt";

        private readonly Dictionary<int, Tile> _tiles;

        public JurassicJigsaw20(string inputString)
        {
            _tiles = inputString
                .Split(InputHelper.LineEnding + InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries)
                .Select(tile => new Tile(tile))
                .ToDictionary(tile => tile.Id, tile => tile);
        }

        public static void Run(int part)
        {
            var input = InputHelper.GetResourceFileAsString(InputFileResourceName);            

            var jurassicJigsaw = new JurassicJigsaw20(input);

            switch (part)
            {
                case 1:
                    {
                        var image = jurassicJigsaw.ReassembleTiles();
                        Console.WriteLine(image);
                        var cornerProduct = image.CornerTiles.Aggregate(1L, (a, t) => a * t.Id);

                        Console.WriteLine($"Product of four corner tiles is: {cornerProduct}");
                        break;
                    }
                case 2:
                    {
                        var image = jurassicJigsaw.ReassembleTiles();
                        var satelliteImage = image.Render();
                        var imageSearcher = new ImageSearcher();

                        var numberOfSeaMonsters = 0;
                        var index = 0;
                        while (index < 16 && numberOfSeaMonsters == 0)
                        {
                            if (index % 4 == 0 && index % 4 != 0)
                            {
                                satelliteImage.Flip(FlipOrientation.Horizontal);
                            }
                            if (index % 8 == 0)
                            {
                                satelliteImage.Flip(FlipOrientation.Vertical);
                            }
                            satelliteImage.Rotate();
                            
                            numberOfSeaMonsters = imageSearcher.GetNumberOfSeaMonsters(satelliteImage);
                            index++;
                        }
                        Console.WriteLine(satelliteImage);

                        Console.WriteLine($"Number of sea monsters is {numberOfSeaMonsters}");
                        var choppyWaters = Enumerable.Range(0, satelliteImage.height).Select(y => Enumerable.Range(0, satelliteImage.width).Count(x => satelliteImage._image[x, y])).Sum();
                        var choppiness = choppyWaters - (numberOfSeaMonsters*15);

                        Console.WriteLine($"The water choppiness is {choppiness}.");
                        
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        private Image ReassembleTiles()
        {
            var assembler = new Assembler(_tiles.Select(kvp => kvp.Value).ToArray());
            assembler.Assemble();
            return assembler.GetImage();
        }
    }
}