namespace danl.adventofcode2020.PassportValidator04
{
    public class HeightValidator : IFieldValidator
    {
        public bool Validate(string fieldValue)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
                return false;

            if (fieldValue.Length < 3)
                return false;

            if (!int.TryParse(fieldValue.Substring(0, fieldValue.Length - 2), out var heightValue))
                return false;

            var heightUnits = fieldValue.Substring(fieldValue.Length - 2).ToLowerInvariant();

            switch (heightUnits)
            {
                case "in":
                    return heightValue >= 59 && heightValue <= 76;
                case "cm":
                    return heightValue >= 150 && heightValue <= 193;
                default:
                    return false;
            }
        }
    }
}