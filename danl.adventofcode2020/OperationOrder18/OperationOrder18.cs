using System;
using System.Collections.Generic;
using System.Linq;

namespace danl.adventofcode2020.OperationOrder18
{
    [Puzzle(puzzleNumber: 18, numberOfParts: 2)]
    public class OperationOrder18
    {
        public const string InputFileResourceName = "danl.adventofcode2020.OperationOrder18.input.txt";

        private readonly string[] _expressions;

        public OperationOrder18(string inputString)
        {
            _expressions = inputString
                .Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries);
        }

        public static void Run(int part)
        {
            var input = InputHelper.GetResourceFileAsString(InputFileResourceName);            

            var operationOrder = new OperationOrder18(input);

            switch (part)
            {
                case 1:
                    {
                        var sumOfExpressionValues = operationOrder.GetExpressionValues((p) => p.ParseWithEqualPrecedence()).Aggregate(0UL, (a,v) => a + v);
                        
                        Console.WriteLine($"Sum of expressions {sumOfExpressionValues}");
                        break;
                    }
                case 2:
                    {
                        var sumOfExpressionValues = operationOrder.GetExpressionValues((p) => p.ParseWithAdditionHigherPrecedence()).Aggregate(0UL, (a, v) => a + v);

                        Console.WriteLine($"Sum of expressions {sumOfExpressionValues}");
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        public IEnumerable<ulong> GetExpressionValues(Func<Parser,Expression> parseFunc)
        {
            foreach (var expression in _expressions)
            {
                var scanner = new Scanner(expression);
                var parser = new Parser(scanner.GetTokens());
                var expressionTree = parseFunc(parser);

                var value = expressionTree.Evaluate();

                yield return value;
            }
        }
    }
}