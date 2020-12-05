using System.Linq;

namespace danl.adventofcode2020.PassportValidator04
{
    public class HexColorValidator : IFieldValidator
    {
        private readonly char[] ValidHexDigits = new[] {
            'a', 'b', 'c', 'd', 'e', 'f',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        public bool Validate(string fieldValue)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
                return false;

            if (fieldValue.Length != 7)
                return false;

            if (fieldValue[0] != '#')
                return false;

            foreach (var hexDigit in fieldValue.Substring(1))
            {
                if (!ValidHexDigits.Contains(hexDigit))
                    return false;
            }

            return true;
        }
    }
}