using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace danl.adventofcode2020.PassportValidator04
{
    [Puzzle(puzzleNumber: 4, numberOfParts: 2)]
    public class PassportValidator04
    {
        public const string InputFileResourceName = "danl.adventofcode2020.PassportValidator04.input.txt";

        private readonly string[] _passportsAsStrings;

        public PassportValidator04(string inputString)
        {
            _passportsAsStrings = inputString
                            .Split(InputHelper.LineEnding + InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries);            
        }

        public static void Run(int part)
        {
            var passportValidator = new PassportValidator04(InputHelper.GetResourceFileAsString(InputFileResourceName));            

            switch (part)
            {
                case 1:
                    {
                        var numberOfValidPassports = passportValidator.GetNumberOfValidPassports(onlyRequiredFields: true);
                        Console.WriteLine($"{numberOfValidPassports} passports are valid.");
                        break;
                    }
                case 2:
                    {
                        var numberOfValidPassports = passportValidator.GetNumberOfValidPassports(onlyRequiredFields: false);
                        Console.WriteLine($"{numberOfValidPassports} passports are valid.");
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        public long GetNumberOfValidPassports(bool onlyRequiredFields)
        {
            var requiredFieldsList = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
            var fieldValidations = new Dictionary<string, IFieldValidator[]> {
                { "byr", new IFieldValidator[] { new IntegerValidator(1920, 2002) } },
                { "iyr", new IFieldValidator[] { new IntegerValidator(2010, 2020) } },
                { "eyr", new IFieldValidator[] { new IntegerValidator(2020, 2030) } },
                { "hgt", new IFieldValidator[] { new HeightValidator() } },
                { "hcl", new IFieldValidator[] { new HexColorValidator() } },
                { "ecl", new IFieldValidator[] { new EnumValueValidator(new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }) } },
                { "pid", new IFieldValidator[] { new RegexValidator("^[0-9]{9}$") } },
            };
            
            var fieldChecker = new PassportFieldChecker(requiredFieldsList, onlyRequiredFields ? null : fieldValidations);

            return _passportsAsStrings.Count(fieldChecker.Validate);
        }
    }
}
