using System;
using System.Collections.Generic;
using System.Linq;

namespace danl.adventofcode2020.PasswordDatabase02
{
    [Puzzle(puzzleNumber: 2, numberOfParts: 1)]
    public class PasswordDatabase02
    {
        public const string InputFileResourceName = "danl.adventofcode2020.PasswordDatabase02.input.txt";

        public readonly PasswordDatabaseEntry[] PasswordDatabase;

        public PasswordDatabase02(string inputString)
        {
            PasswordDatabase = inputString
                .Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries)
                .Select(row =>
                {
                    var rowFields = row.Split(':', 2);
                    var passwordPolicy = rowFields[0].Trim();
                    var password = rowFields[1].Trim();

                    return new PasswordDatabaseEntry
                    {
                        PasswordPolicyString = passwordPolicy,
                        Password = password
                    };
                })
                .ToArray();
        }

        public static void Run(int part)
        {
            var passwordDatabase = new PasswordDatabase02(InputHelper.GetResourceFileAsString(InputFileResourceName));
            var passwordPolicyValidator = default(IPasswordPolicyValidator);

            switch (part)
            {
                case 1:
                    passwordPolicyValidator = new SledRentalPasswordPolicyValidator();
                    break;
                case 2:
                    passwordPolicyValidator = new TobogganRentalPasswordPolicyValidator();
                    break;
                default:
                    throw new NotImplementedException();
            }

            var numberOfValidPasswords = passwordDatabase.GetInvalidPasswordCount(passwordPolicyValidator);
            Console.WriteLine($"There are {numberOfValidPasswords} valid passwords.");
        }

        public int GetInvalidPasswordCount(IPasswordPolicyValidator passwordPolicyValidator)
        {
            return PasswordDatabase.Count(x => IsPasswordEntryValid(x, passwordPolicyValidator));
        }

        public static bool IsPasswordEntryValid(PasswordDatabaseEntry passwordDatabaseEntry, IPasswordPolicyValidator passwordPolicyValidator)        
        {
            passwordPolicyValidator.LoadPolicy(passwordDatabaseEntry.PasswordPolicyString);
            return passwordPolicyValidator.ValidatePassword(passwordDatabaseEntry.Password);
        }
    }
}
