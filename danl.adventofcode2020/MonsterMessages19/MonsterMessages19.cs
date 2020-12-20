using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace danl.adventofcode2020.MonsterMessages19
{
    [Puzzle(puzzleNumber: 19, numberOfParts: 2)]
    public class MonsterMessages19
    {
        public const string InputFileResourceName = "danl.adventofcode2020.MonsterMessages19.input.txt";

        private readonly Dictionary<int, string> _unparsedRules;

        private readonly string[] _satelliteMessages;

        public MonsterMessages19(string inputString)
        {
            var inputParts = inputString.Split(InputHelper.LineEnding + InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries);

            _unparsedRules = inputParts[0]
                .Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries)
                .Select(r => r.Split(':'))
                .ToDictionary(rp => int.Parse(rp[0].Trim()), rp => rp[1].Trim());                

            _satelliteMessages = inputParts[1].Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries);
        }

        public static void Run(int part)
        {            
            var input = InputHelper.GetResourceFileAsString(InputFileResourceName);            

            var monsterMessages = new MonsterMessages19(input);

            switch (part)
            {
                case 1:
                    {
                        var numberOfValidMessages = monsterMessages.GetValidMessages().Count();

                        Console.WriteLine($"Number of valid messages: {numberOfValidMessages}");
                        break;
                    }
                case 2:
                    {
                        var numberOfValidMessages = monsterMessages.GetValidMessagesWithLoops().Count();

                        Console.WriteLine($"Number of valid messages: {numberOfValidMessages}");
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        private IEnumerable<string> GetValidMessages()
        {

            var reg = new Regex($"^{GetRegexStringForRule(0)}$");
            return _satelliteMessages.Where(s => reg.IsMatch(s));
        }

        private IEnumerable<string> GetValidMessagesWithLoops()
        {
            _unparsedRules[8] = "42 | 42 8";
            _unparsedRules[11] = "42 31 | 42 42 31 31 | 42 42 42 31 31 31 | 42 42 42 42 31 31 31 31 | 42 42 42 42 42 31 31 31 31 31";
            var reg = new Regex($"^{GetRegexStringForRule(0)}$");
            return _satelliteMessages.Where(s => {

                var match = reg.Match(s);
                return match.Success;

            });
        }

        private string GetRegexStringForRule(int ruleNumber)
        {
            var ruleExpressedAsString = _unparsedRules[ruleNumber];

            if (Regex.IsMatch(ruleExpressedAsString, "\\b" + ruleNumber.ToString() + "\\b"))
            {
                if (ruleNumber == 8)
                    return $"({GetRegexStringForRule(42)})+";

                throw new InvalidOperationException();
            }

            var parts = ruleExpressedAsString
                .Split('|', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToArray();

            if (parts.Length == 1)
                return GetRegexConcatenationString(parts[0]);

            return $"({string.Join('|', parts.Select(p => GetRegexConcatenationString(p)))})";
        }

        private string GetRegexConcatenationString(string ruleExpressedAsString)
        {
            return string.Concat(ruleExpressedAsString
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(ruleNumberOrCharacter => {

                    if (ruleNumberOrCharacter.StartsWith("\""))
                    {
                        var endIndex = ruleNumberOrCharacter.IndexOf('"', 1);

                        if (endIndex == -1)
                            throw new InvalidOperationException();

                        return ruleNumberOrCharacter.Substring(1, endIndex - 1);
                    }

                    if (!int.TryParse(ruleNumberOrCharacter, out var ruleNumber))
                    {
                        throw new InvalidOperationException();                        
                    }

                    return GetRegexStringForRule(ruleNumber);

                }));
        }
    }
}