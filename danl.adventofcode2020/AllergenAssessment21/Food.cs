using System;

namespace danl.adventofcode2020.AllergenAssessment21
{
    public class Food
    {
        public Food(string food)
        {
            if (string.IsNullOrWhiteSpace(food))
                throw new ArgumentNullException(nameof(food));

            var parts = food.Split(" (contains ", StringSplitOptions.RemoveEmptyEntries);

            Ingredients = parts[0]
                            .Trim()
                            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 1)
            {
                Allergens = new string[0];
                return;
            }

            Allergens = parts[1]
                            .TrimEnd(')')
                            .Split(", ", StringSplitOptions.RemoveEmptyEntries);
        }

        public string[] Ingredients { get; private set; }

        public string[] Allergens { get; private set; }
    }
}
