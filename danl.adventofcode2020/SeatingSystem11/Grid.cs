using System;
using System.Collections.Generic;
using System.Linq;

namespace danl.adventofcode2020.SeatingSystem11
{
    public class Grid
    {
        private readonly Position[][] _layout;

        private int _noOfColumns;

        private int _noOfRows;

        public Grid(Position[][] layout)
        {
            if (layout == null)
                throw new ArgumentNullException(nameof(layout));

            _layout = layout;
            _noOfColumns = layout[0].Length;
            _noOfRows = layout.Length;
        }

        public Position[][] Layout => _layout;

        public int NumberOfColumns => _noOfColumns;

        public int NumberOfRows => _noOfRows;

        public Position GetPosition(int x, int y)
        {
            return _layout[y][x];
        }

        public Coordinate[][] AllPositionsInEachDirection(int x, int y)
        {
            int yCoordinate;
            int xCoordinate;
            var seatsInDirections = new List<Coordinate>[8];

            //north
            seatsInDirections[0] = new List<Coordinate>();
            yCoordinate = y - 1;
            while (yCoordinate >= 0)
                seatsInDirections[0].Add(new Coordinate { x = x, y = yCoordinate-- });

            //north east
            seatsInDirections[1] = new List<Coordinate>();
            xCoordinate = x + 1;
            yCoordinate = y - 1;
            while (xCoordinate < _noOfColumns && yCoordinate >= 0)
                seatsInDirections[1].Add(new Coordinate { x = xCoordinate++, y = yCoordinate-- });

            //east
            seatsInDirections[2] = new List<Coordinate>();
            xCoordinate = x + 1;            
            while (xCoordinate < _noOfColumns)
                seatsInDirections[2].Add(new Coordinate { x = xCoordinate++, y = y });

            //south east
            seatsInDirections[3] = new List<Coordinate>();
            xCoordinate = x + 1;
            yCoordinate = y + 1;
            while (xCoordinate < _noOfColumns && yCoordinate < _noOfRows)
                seatsInDirections[3].Add(new Coordinate { x = xCoordinate++, y = yCoordinate++ });

            //south
            seatsInDirections[4] = new List<Coordinate>();            
            yCoordinate = y + 1;
            while (yCoordinate < _noOfRows)
                seatsInDirections[4].Add(new Coordinate { x = x, y = yCoordinate++ });

            //south west
            seatsInDirections[5] = new List<Coordinate>();
            xCoordinate = x - 1;
            yCoordinate = y + 1;
            while (xCoordinate >= 0 && yCoordinate < _noOfRows)
                seatsInDirections[5].Add(new Coordinate { x = xCoordinate--, y = yCoordinate++ });

            //west
            seatsInDirections[6] = new List<Coordinate>();
            xCoordinate = x - 1;            
            while (xCoordinate >= 0)
                seatsInDirections[6].Add(new Coordinate { x = xCoordinate--, y = y });

            //north west
            seatsInDirections[7] = new List<Coordinate>();
            xCoordinate = x - 1;
            yCoordinate = y - 1;
            while (xCoordinate >= 0 && yCoordinate >= 0)
                seatsInDirections[7].Add(new Coordinate { x = xCoordinate--, y = yCoordinate-- });

            return seatsInDirections.Select(l => l.ToArray()).ToArray();
        }
    }
}
