using System;
using System.Collections.Generic;
using System.Text;

namespace danl.adventofcode2020.JurassicJigsaw20
{
    public class ImageSearcher
    {
        public int GetNumberOfSeaMonsters(SatelliteImage image)
        {
            var count = 0;

            for (int y = 0; y < image.height - 3; y++)
            {
                for (int x = 0; x < image.width - 20; x++)
                {
                    if (SeaMonsterAt(image, x, y))
                        count++;
                }
            }

            return count;
        }

        private bool SeaMonsterAt(SatelliteImage image, int x, int y)
        {
            /*      01  4567  0123  6789
             *0                       # 
             *1     #    ##    ##    ###  
             *2      #  #  #  #  #  #    

             */

            if (image._image[x, y + 1] && image._image[x + 1, y + 2] && image._image[x + 4, y + 2] && image._image[x + 5, y + 1] && image._image[x + 6, y + 1] && image._image[x + 7, y + 2]
                && image._image[x + 10, y + 2] && image._image[x + 11, y + 1] && image._image[x + 12, y + 1] && image._image[x + 13, y + 2]
                && image._image[x + 16, y + 2] && image._image[x + 17, y + 1] && image._image[x + 18, y + 1] && image._image[x + 19, y + 1] && image._image[x + 18, y])
                return true;

            return false;
        }
    }
}
