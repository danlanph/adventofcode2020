using System;
using System.Linq;

namespace danl.adventofcode2020.CrabCups23
{
    [Puzzle(puzzleNumber: 23, numberOfParts: 2)]
    public class CrabCups23
    {
        public const string Input = "158937462";

        private readonly int[] _cards;

        public CrabCups23(string inputString)
        {
            _cards = inputString.Select(i => int.Parse(i.ToString())).ToArray();
        }

        public static void Run(int part)
        {
            var crabCups = new CrabCups23(Input);

            switch (part)
            {
                case 1:
                    {
                        var order = crabCups.GetOrderAfterMoves(100);
                        Console.WriteLine($"Order after 100 moves : {order}.");

                        break;
                    }
                case 2:
                    {
                        var productOfCupsNextToCupOne = crabCups.ProductOfCupsNextToCupOne(10000000);
                        Console.WriteLine($"Prodict of cups nex to cup 1: {productOfCupsNextToCupOne}.");

                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        private long GetOrderAfterMoves(int numberOfIterations)
        {
            var crabCupGame = new CrabCupGame(_cards, false);

            while (numberOfIterations-- > 0)
            {
                crabCupGame.Move();
            }   

            var order = crabCupGame.Order().Aggregate(0L, (a, d) => 10 * a + d);

            return order;
        }

        private long ProductOfCupsNextToCupOne(int numberOfIterations)
        {
            var crabCupGame = new CrabCupGame(_cards, true);

            while (numberOfIterations-- > 0)
            {
                crabCupGame.Move();
            }

            var cupOne = crabCupGame.Items[1];

            var a = (long)cupOne.Next.Item;
            var b = (long)cupOne.Next.Next.Item;

            return a * b;
        }
    }
}