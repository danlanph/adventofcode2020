namespace danl.adventofcode2020.PassportValidator04
{
    public class IntegerValidator : IFieldValidator
    {
        private readonly bool checkRange;
        private int minValue;
        private int maxValue;

        public IntegerValidator(int? minimum, int? maximum)
        {
            checkRange = minimum.HasValue && maximum.HasValue;

            if (checkRange)
            {
                minValue = minimum.Value;
                maxValue = maximum.Value;
            }
        }

        public bool Validate(string fieldValue)
        {
            if (!int.TryParse(fieldValue, out var value))
                return false;

            if (checkRange)
            {
                if (value < minValue || value > maxValue)
                    return false;
            }

            return true;
        }
    }
}
