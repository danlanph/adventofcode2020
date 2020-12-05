using System.Linq;

namespace danl.adventofcode2020.PassportValidator04
{
    internal class EnumValueValidator : IFieldValidator
    {
        private readonly string[] _validValues;

        public EnumValueValidator(string[] validValues)
        {
            _validValues = validValues;
        }

        public bool Validate(string fieldValue)
        {
            return _validValues.Contains(fieldValue);
        }
    }
}