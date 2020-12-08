using System.Collections.Generic;

namespace danl.adventofcode2020.LuggageControl07
{
    public class Bag
    {        
        public string Color { get; private set; }

        public IList<BagQuantity> ContainedBags { get; private set; }

        public Bag(string color)
        {
            Color = color;
            ContainedBags = new List<BagQuantity>();
        }
    }

    public class BagQuantity
    {
        public int Quantity { get; set; }

        public Bag Bag { get; set; }
    }
}
