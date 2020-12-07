using System;
using System.Linq;

namespace danl.adventofcode2020.CustomsDeclaration06
{
    [Puzzle(puzzleNumber: 6, numberOfParts: 2)]
    public class CustomsDeclaration06
    {
        public const string InputFileResourceName = "danl.adventofcode2020.CustomsDeclaration06.input.txt";

        private readonly string[][] _groupCustomDeclarations;

        public CustomsDeclaration06(string inputString)
        {
            _groupCustomDeclarations = inputString
                .Split(InputHelper.LineEnding + InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries)
                .Select(g => g.Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries))
                .ToArray();
        }

        public static void Run(int part)
        {
            var customsDeclaration = new CustomsDeclaration06(InputHelper.GetResourceFileAsString(InputFileResourceName));            

            switch (part)
            {
                case 1:
                    var totalAnyYesQuestionsAcrossGroups = customsDeclaration.GetGroupAnyAnsweredYesTotals();
                    Console.WriteLine($"The total questions any answered yes per group is {totalAnyYesQuestionsAcrossGroups}.");
                    break;
                case 2:
                    var totalAllYesQuestionsAcrossGroups = customsDeclaration.GetGroupAllAnsweredYesTotals();
                    Console.WriteLine($"The total questions all answered yes per group is {totalAllYesQuestionsAcrossGroups}.");
                    break;
                default:
                    throw new NotImplementedException();
            }            
        }

        public int GetGroupAllAnsweredYesTotals()
        {
            return _groupCustomDeclarations.Select(GetGroupAllTotal).Sum();
        }

        public int GetGroupAnyAnsweredYesTotals()
        {
            return _groupCustomDeclarations.Select(GetGroupAnyTotal).Sum();
        }

        public static int GetGroupAnyTotal(string[] customsDeclarations)
        {
            return customsDeclarations
                .Select(s => s.ToCharArray())
                .SelectMany(x => x)
                .Distinct()
                .Count();
        }

        public static int GetGroupAllTotal(string[] customsDeclarations)
        {
            var AtoZ = Enumerable.Range('a', 26).Select(x => (char)x);

            return customsDeclarations
                .Select(s => s.ToCharArray())
                .Aggregate(AtoZ, (all, current) => all.Intersect(current))
                .Count();
        }
    }
}
