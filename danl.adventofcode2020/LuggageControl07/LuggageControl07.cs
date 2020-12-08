using System;
using System.Collections.Generic;
using System.Linq;

namespace danl.adventofcode2020.LuggageControl07
{
    [Puzzle(puzzleNumber: 7, numberOfParts: 2)]
    public class LuggageControl07
    {
        public const string InputFileResourceName = "danl.adventofcode2020.LuggageControl07.input.txt";

        private readonly Dictionary<string, Tuple<int, string>[]> _luggageRules;

        public LuggageControl07(string inputString)
        {
            _luggageRules = inputString
                .Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries)
                .Select(r => r.Split(" bags contain "))
                .Select(r =>
                {
                    var bagColor = r[0];
                    var containedBagsList = r[1].Trim('.');

                    var containedBags = containedBagsList.StartsWith("no other bags") 
                        ? new Tuple<int, string>[0]
                        : containedBagsList.Split(", ", StringSplitOptions.RemoveEmptyEntries)
                        .Select(b => b.Trim())
                        .Select(b => {
                            var bagSpec = b.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                            return new Tuple<int, string>(int.Parse(bagSpec[0]), string.Join(' ', bagSpec[1], bagSpec[2]));
                        })
                        .ToArray();                        

                    return new { Color = bagColor, ContainedBags = containedBags };
                })
                .ToDictionary(r => r.Color, r => r.ContainedBags);
        }

        public static void Run(int part)
        {
            var luggageControl = new LuggageControl07(InputHelper.GetResourceFileAsString(InputFileResourceName));            

            switch (part)
            {
                case 1:
                    var numberOfOuterMostBagsContainingShinyGold = luggageControl.GetBagsContainingBag("shiny gold").Count();
                    Console.WriteLine($"{numberOfOuterMostBagsContainingShinyGold} bags contain shiny gold.");
                    break;
                case 2:
                    var numberOfBagsInsideShinyGold = luggageControl.GetNumberOfContainedBagsInBag("shiny gold");
                    Console.WriteLine($"{numberOfBagsInsideShinyGold} bags are inside the  shiny gold bag.");
                    break;
                default:
                    throw new NotImplementedException();
            }            
        }

        public IEnumerable<Bag> GetBagsContainingBag(string color)
        {
            var bags = BuildBagGraph(_luggageRules);

            foreach (var bag in bags.Where(b => !b.Color.Equals(color, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (VisitsBag(bag, color))
                    yield return bag;
            }

            yield break;
        }

        public int GetNumberOfContainedBagsInBag(string color)
        {
            var bags = BuildBagGraph(_luggageRules);

            var bag = bags.First(b => b.Color.Equals(color, StringComparison.InvariantCultureIgnoreCase));

            return GetNumberOfContainedBagsInBag(bag);
        }

        public static int GetNumberOfContainedBagsInBag(Bag bag)
        {
            return bag.ContainedBags
                        .Select(b => b.Quantity * (1 + GetNumberOfContainedBagsInBag(b.Bag)))
                        .Sum();
        }

        public static bool VisitsBag(Bag currentBag, string color)
        {
            if (currentBag.Color.Equals(color, StringComparison.InvariantCultureIgnoreCase))
                return true;

            return currentBag.ContainedBags.Any(b => VisitsBag(b.Bag, color));
        }

        public IList<Bag> BuildBagGraph(Dictionary<string, Tuple<int, string>[]> relationships)
        {
            var bags = new List<Bag>();

            foreach (var relationship in relationships)
            {
                var bag = GetBagWithCreation(bags, relationship.Key);

                foreach (var child in relationship.Value)
                {
                    var childBag = GetBagWithCreation(bags, child.Item2);
                    bag.ContainedBags.Add(new BagQuantity { Bag = childBag, Quantity = child.Item1 });
                }
            }

            return bags;
        }

        public static Bag GetBagWithCreation(IList<Bag> source, string color)
        {
            var existingBag = source.FirstOrDefault(b => b.Color.Equals(color, StringComparison.InvariantCultureIgnoreCase));

            if (existingBag == null)
            {
                existingBag = new Bag(color);
                source.Add(existingBag);
            }

            return existingBag;
        }
    }
}
