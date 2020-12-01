using System;
using System.Collections.Generic;
using System.Linq;

namespace danl.adventofcode2020.ExpenseReport01
{
    [Puzzle(puzzleNumber: 1, numberOfParts: 2)]
    public class ExpenseReport01
    {
        public const string InputFileResourceName = "danl.adventofcode2020.ExpenseReport01.input.txt";
        public const int TargetSumForExpenseItems = 2020;

        public readonly int[] ExpenseReportNumbers;

        public ExpenseReport01(string inputString)
        {
            ExpenseReportNumbers = inputString
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Select(int.Parse)
                .ToArray();
        }

        public static void Run(int part)
        {
            var expenseReport = new ExpenseReport01(InputHelper.GetResourceFileAsString(InputFileResourceName));

            switch (part)
            {
                case 1:
                case 2:
                    {
                        var parts = part + 1;
                        var items = expenseReport.GetNExpenseItemsWithSum(expenseReport.ExpenseReportNumbers.ToList(), TargetSumForExpenseItems, parts);

                        if (items == null)
                            throw new Exception($"Unable to locate {parts} items from list with sum equal to {TargetSumForExpenseItems}.");

                        var product = items.Aggregate(1, (p, x) => p * x);
                        Console.WriteLine($"{string.Join(" x ", items)} = {product}");
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }            
        }

        public Tuple<int, int> GetTwoExpenseItemsWithSum(int sum)
        {
            foreach (var expenseItem in ExpenseReportNumbers)
            {
                var remainder = sum - expenseItem;

                if (ExpenseReportNumbers.Contains(remainder))
                    return new Tuple<int, int>(expenseItem, remainder);
            }

            throw new Exception($"Unable to locate a pair from list with sum equal to {sum}.");
        }

        public IList<int> GetNExpenseItemsWithSum(IList<int> items, int sum, int numberOfPartitions)
        {
            if (numberOfPartitions == 1)
            {
                if (items.Contains(sum))
                    return new List<int>() { sum };

                return null;
            }

            foreach (var item in items)
            {
                var listWithoutItem = new List<int>(items);
                listWithoutItem.Remove(item);

                var itemsSummingToTarget = GetNExpenseItemsWithSum(listWithoutItem, sum - item, numberOfPartitions - 1);

                if (itemsSummingToTarget != null)
                {
                    itemsSummingToTarget.Add(item);
                    return itemsSummingToTarget;
                }
            }

            return null;                
        }
    }
}
