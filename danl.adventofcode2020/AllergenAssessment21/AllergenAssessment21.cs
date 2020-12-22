using System;
using System.Collections.Generic;
using System.Linq;

namespace danl.adventofcode2020.AllergenAssessment21
{
    [Puzzle(puzzleNumber: 21, numberOfParts: 2)]
    public class AllergenAssessment21
    {
        public const string InputFileResourceName = "danl.adventofcode2020.AllergenAssessment21.input.txt";

        private readonly Food[] _foods;

        public AllergenAssessment21(string inputString)
        {
            _foods = inputString
                        .Split(InputHelper.LineEnding, StringSplitOptions.RemoveEmptyEntries)
                        .Select(t => new Food(t))
                        .ToArray();
        }

        public static void Run(int part)
        {
            var input = InputHelper.GetResourceFileAsString(InputFileResourceName);            

            var allergenAssessment = new AllergenAssessment21(input);

            switch (part)
            {
                case 1:
                    {
                        var nonAllergenIngredientInstances = allergenAssessment.GetNumberOfNonAllergenIngredientInstances();
                        Console.WriteLine($"Number of instances of ingredients that cannot be allergens is: {nonAllergenIngredientInstances}.");
                        
                        break;
                    }
                case 2:
                    {
                        var dangerousIngredients = allergenAssessment.GetCanonicalDangerousIngredientsList();
                        Console.WriteLine($"Dangerous ingredients: {dangerousIngredients}.");

                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        private string GetCanonicalDangerousIngredientsList()
        {
            var allIngredients = GetAllIngredients();
            var allergenToIngredientMappings = GetAllergenToIngredientMappings(allIngredients);
            InferAllergenIngredientMapping(allergenToIngredientMappings);

            return string.Join(',', allergenToIngredientMappings
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => kvp.Value.First()));
        }

        private int GetNumberOfNonAllergenIngredientInstances()
        {
            var allIngredients = GetAllIngredients();
            var allergenToIngredientMappings = GetAllergenToIngredientMappings(allIngredients);
            InferAllergenIngredientMapping(allergenToIngredientMappings);
            var nonAllergenIngredients = GetNonAllergens(allIngredients, allergenToIngredientMappings);

            return _foods
                .Select(f => f.Ingredients)
                .SelectMany(f => f)
                .Where(i => nonAllergenIngredients.Any(nai => nai.Equals(i, StringComparison.InvariantCultureIgnoreCase)))
                .Count();
        }

        private Dictionary<string,IList<string>> GetAllergenToIngredientMappings(string[] allIngredients)
        {
            return _foods
                .Select(f => f.Allergens)
                .SelectMany(a => a)
                .Distinct()
                .Select(x => new
                {
                    Allergen = x,
                    Foods = _foods.Where(f => f.Allergens.Contains(x))
                })
                .Select(m => new
                {
                    Allergen = m.Allergen,
                    PossibleIngredients = (IList<string>)m.Foods.Aggregate(allIngredients.AsEnumerable(), (s,igl) => s.Intersect(igl.Ingredients)).ToList()
                })
                .ToDictionary(m => m.Allergen, m => m.PossibleIngredients);                
        }

        private string[] GetNonAllergens(string[] allIngredients, Dictionary<string, IList<string>> allergenIngredientMapping)
        {
            return allIngredients.Except(allergenIngredientMapping.Select(kvp => kvp.Value.First())).ToArray();
        }

        private string[] GetAllIngredients()
        {
            return _foods
                    .Select(igl => igl.Ingredients)
                    .SelectMany(igl => igl)
                    .Distinct()
                    .ToArray();
        }

        private Dictionary<string, IList<string>> InferAllergenIngredientMapping(Dictionary<string, IList<string>> allergenToIngredientMapping)
        {
            bool changed = true;
            var allergensToSolve = allergenToIngredientMapping.Keys.ToList();

            while (changed && allergensToSolve.Count > 1)
            {
                changed = false;
                foreach (var allergen in allergensToSolve.ToArray())
                {
                    if (allergenToIngredientMapping[allergen].Count == 1)
                    {
                        var mappedIngredient = allergenToIngredientMapping[allergen].First();
                        foreach (var kvp in allergenToIngredientMapping)
                        {
                            if (kvp.Key == allergen)
                                continue;

                            if (kvp.Value.Contains(mappedIngredient))
                            {
                                kvp.Value.Remove(mappedIngredient);
                                changed = true;
                            }
                                
                        }

                        allergensToSolve.Remove(allergen);
                    }
                }
            }

            if (allergensToSolve.Count > 0)
                throw new Exception("Unable to infer unique mapping");

            return allergenToIngredientMapping;
        }

        /*private IList<IngredientList> GetIngredientLists()
        {
            return _foods
                .Select(f => new IngredientList
                {
                    Ingredients = f.Ingredients.ToList(),
                    Allergens = f.Allergens.ToList()
                })
                .ToList();
        }*/
    }
}