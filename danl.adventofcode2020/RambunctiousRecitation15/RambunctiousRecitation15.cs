using System;
using System.Collections.Generic;
using System.Linq;

namespace danl.adventofcode2020.RambunctiousRecitation15
{
    [Puzzle(puzzleNumber: 15, numberOfParts: 2)]
    public class RambunctiousRecitation15
    {
        public const string Input = "8,11,0,19,1,2";

        private int[] _startSequence;

        public RambunctiousRecitation15(string inputString)
        {
            _startSequence = inputString
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(i => int.Parse(i))
                .ToArray();
        }

        public static void Run(int part)
        {
            var rambunctiousRecitation = new RambunctiousRecitation15(Input);

            switch (part)
            {                
                case 1:
                    {
                        var sequenceTerm = rambunctiousRecitation.GetTerm(2020);

                        Console.WriteLine($"The 2020th number spoken is {sequenceTerm}");
                        break;
                    }
                case 2:
                    {
                        var sequenceTerm = rambunctiousRecitation.GetTerm(30000000);

                        Console.WriteLine($"The 30000000th number spoken is {sequenceTerm}");
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        private int GetTerm(int index)
        {
            var sequence = new Sequence(_startSequence);

            var currentIndex = sequence.CurrentIndex;
            while (currentIndex < index)
                currentIndex = sequence.GenerateNextTerm();

            return sequence.CurrentTerm;
        }
    }
}