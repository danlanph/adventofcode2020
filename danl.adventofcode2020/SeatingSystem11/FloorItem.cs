using System;
using System.Collections.Generic;
using System.Text;

namespace danl.adventofcode2020.SeatingSystem11
{
    public abstract class Position
    {
        public abstract bool Occupied { get; }

        public abstract bool CanOccupy { get; }
    }

    public class Floor : Position
    {
        public override bool Occupied => false;

        public override bool CanOccupy => false;

        public override string ToString()
        {
            return ".";
        }
    }

    public class Seat : Position
    {
        private bool _occupied = false;
        public override bool Occupied => _occupied;

        public override bool CanOccupy => true;

        public void Toggle()
        {
            _occupied = !_occupied;
        }

        public override string ToString()
        {
            return _occupied ? "#" : "L";
        }
    }
}
