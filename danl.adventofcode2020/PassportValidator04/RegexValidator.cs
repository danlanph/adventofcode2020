using System.Text.RegularExpressions;

namespace danl.adventofcode2020.PassportValidator04
{
    internal class RegexValidator : IFieldValidator
    {
        private readonly Regex _regex;

        public RegexValidator(string regex)
        {
            _regex = new Regex(regex);
        }

        public bool Validate(string fieldValue)
        {
            return _regex.IsMatch(fieldValue);
        }
    }
}