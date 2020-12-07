using System;
using System.Collections.Generic;
using System.Text;

namespace danl.adventofcode2020.SeatFinder05
{
    public class Seat
    {
        public int Id { get; private set; }

        public int Row => Id / 8;

        public int Column => Id % 8;

        public Seat(string seatRepresentationString)
        {
            var binaryString = seatRepresentationString
                .Replace("F", "0")
                .Replace("B", "1")
                .Replace("R", "1")
                .Replace("L", "0");

            Id = Convert.ToInt32(binaryString, 2);
        }
    }
}
