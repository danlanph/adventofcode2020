using System;
using System.Collections.Generic;
using System.Linq;

namespace danl.adventofcode2020.TicketTranslation16
{
    [Puzzle(puzzleNumber: 16, numberOfParts: 2)]
    public class TicketTranslation16
    {
        public const string InputFileResourceName = "danl.adventofcode2020.TicketTranslation16.input.txt";

        private readonly Dictionary<string, Tuple<int, int>[]> _fieldRules = new Dictionary<string, Tuple<int, int>[]>();

        private readonly Dictionary<string, int> _columnMapping = new Dictionary<string, int>();

        private Ticket _myTicket;

        private readonly IList<Ticket> _nearbyTickets = new List<Ticket>();

        public TicketTranslation16(string inputString)
        {
            var lines = inputString
                .Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length == 0)
                return;            

            var index = 0;
            var line = default(string);

            // Rules
            while ((index < lines.Length) && !(line = lines[index++]).StartsWith("your ticket:"))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var ruleParts = line.Split(':', StringSplitOptions.RemoveEmptyEntries);

                var fieldName = ruleParts[0].Trim();
                var rules = ruleParts[1].Trim()
                                .Split(" or ", StringSplitOptions.RemoveEmptyEntries)
                                .Select(range =>
                                {
                                    var bounds = range.Split('-', StringSplitOptions.RemoveEmptyEntries);
                                    var lowerBound = int.Parse(bounds[0]);
                                    var upperBound = int.Parse(bounds[1]);

                                    return new Tuple<int,int>(lowerBound, upperBound);
                                })
                                .ToArray();

                _fieldRules.Add(fieldName, rules);
                _columnMapping.Add(fieldName, -1);
            }

            // Your ticket
            while ((index < lines.Length) && !(line = lines[index++]).StartsWith("nearby tickets:"))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                _myTicket = new Ticket(_columnMapping, line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());
            }

            // Nearby tickets
            while (index < lines.Length)
            {
                line = lines[index++];
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                _nearbyTickets.Add(new Ticket(_columnMapping, line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()));
            }
        }

        public static void Run(int part)
        {            
            var input = InputHelper.GetResourceFileAsString(InputFileResourceName);            

            var ticketTranslation = new TicketTranslation16(input);

            switch (part)
            {
                case 1:
                    {
                        var errorRate = ticketTranslation.GetScanningErrorRate();

                        Console.WriteLine($"The scanning error rate is {errorRate}");
                        break;
                    }
                case 2:
                    {
                        var validTickets = ticketTranslation.ValidTickets().ToArray();
                        ticketTranslation.InferColumnsFromValues(validTickets);

                        var departureValueProduct = ticketTranslation._myTicket.FieldNames
                            .Where(fieldName => fieldName.StartsWith("departure"))
                            .Select(fieldName => ticketTranslation._myTicket[fieldName])
                            .Aggregate(1L, (a, v) => a * v);

                        Console.WriteLine($"The product of all departure values is {departureValueProduct}");
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        private void InferColumnsFromValues(Ticket[] validTickets)
        {
            var columnIndexesToMap = Enumerable.Range(0, _columnMapping.Keys.Count).ToList();
            var numberOfInferredColumnsInCurrentPass = int.MaxValue;

            while (columnIndexesToMap.Count > 0 && numberOfInferredColumnsInCurrentPass > 0)
            {
                var inferredColumnIndexes = new List<int>();
                foreach (var i in columnIndexesToMap)
                {
                    if (InferColumnFromValues(i, validTickets.Select(t => t[i])))
                        inferredColumnIndexes.Add(i);
                }

                foreach (var i in inferredColumnIndexes)
                    columnIndexesToMap.Remove(i);

                numberOfInferredColumnsInCurrentPass = inferredColumnIndexes.Count;
            }

            if (columnIndexesToMap.Count != 0)
                throw new Exception("Unable to infer all columns");
        }

        private bool InferColumnFromValues(int index, IEnumerable<int> values)
        {
            var rules = _fieldRules.Where(r => _columnMapping[r.Key] == -1).ToList();

            using (var valueEnumerator = values.GetEnumerator())
            {
                while (rules.Count > 1 && valueEnumerator.MoveNext())
                {
                    var failingRules = rules.Where(r => r.Value.All(range => valueEnumerator.Current < range.Item1 || valueEnumerator.Current > range.Item2)).ToArray();
                    foreach (var rule in failingRules)
                        rules.Remove(rule);
                }
            }

            if (rules.Count != 1)
                return false;

            var inferredColumn = rules.First();
            _columnMapping[inferredColumn.Key] = index;
            return true;
        }

        private int GetScanningErrorRate()
        {
            return GetInvalidValues().Sum();
        }

        private IEnumerable<int> GetInvalidValues()
        {
            foreach (var ticket in _nearbyTickets)
            {
                foreach (var fieldValue in ticket.Values)
                {
                    if (!IsValidValue(fieldValue))
                        yield return fieldValue;
                }
            }
        }

        private IEnumerable<Ticket> ValidTickets()
        {
            foreach (var ticket in _nearbyTickets)
            {
                if (ticket.Values.All(IsValidValue))
                    yield return ticket;
            }
        }

        private bool IsValidValue(int value)
        {
            return _fieldRules.Values.Any(r => r.Any(range => value >= range.Item1 && value <= range.Item2));
        }
    }
}