using System;
using System.Linq;

namespace danl.adventofcode2020.PasswordDatabase02
{
    public interface IPasswordPolicyValidator
    {
        string PasswordPolicyString { get; }
        void LoadPolicy(string passwordPolicyString);
        bool ValidatePassword(string password);
    }

    public class SledRentalPasswordPolicyValidator : IPasswordPolicyValidator
    {
        public string PasswordPolicyString { get; private set; }

        private char _passwordPolicyCharacter;
        private int _minCharOccurrences;
        private int _maxCharOccurrences;

        public void LoadPolicy(string passwordPolicyString)
        {
            if (string.IsNullOrWhiteSpace(passwordPolicyString))
                throw new ArgumentNullException(nameof(passwordPolicyString));

            PasswordPolicyString = passwordPolicyString;
            var policyParts = passwordPolicyString.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

            var rangeParts = policyParts[0].Split('-');

            _minCharOccurrences = int.Parse(rangeParts[0]);
            _maxCharOccurrences = int.Parse(rangeParts[1]);

            _passwordPolicyCharacter = policyParts[1][0];
                        
        }

        public bool ValidatePassword(string password)
        {
            var characterCount = password.Count(x => x == _passwordPolicyCharacter);

            return characterCount >= _minCharOccurrences && characterCount <= _maxCharOccurrences;
        }
    }

    public class TobogganRentalPasswordPolicyValidator : IPasswordPolicyValidator
    {
        public string PasswordPolicyString { get; private set; }

        private char _passwordPolicyCharacter;
        private int[] _characterPositions;

        public void LoadPolicy(string passwordPolicyString)
        {
            if (string.IsNullOrWhiteSpace(passwordPolicyString))
                throw new ArgumentNullException(nameof(passwordPolicyString));

            PasswordPolicyString = passwordPolicyString;
            var policyParts = passwordPolicyString.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

            _characterPositions = policyParts[0]
                                    .Split('-', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(int.Parse)
                                    .Select(x => x - 1)
                                    .ToArray();

            _passwordPolicyCharacter = policyParts[1][0];

        }

        public bool ValidatePassword(string password)
        {
            return _characterPositions.Count(p => p < password.Length && password[p] == _passwordPolicyCharacter) == 1;
        }
    }
}