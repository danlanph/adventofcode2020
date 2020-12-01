using System;
namespace danl.adventofcode2020
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class PuzzleAttribute : Attribute
    {
        public PuzzleAttribute(int puzzleNumber, int numberOfParts)
        {
            if (puzzleNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(puzzleNumber), "Must be greater than or equal to 1");

            if (numberOfParts < 1)
                throw new ArgumentOutOfRangeException(nameof(numberOfParts), "Must be greater than or equal to 1");

            PuzzleNumber = puzzleNumber;
            NumberOfParts = numberOfParts;
        }

        public int PuzzleNumber { get; private set; }
        public int NumberOfParts { get; private set; }
    }
}
