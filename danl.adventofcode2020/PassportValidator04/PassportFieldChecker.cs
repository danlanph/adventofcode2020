using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace danl.adventofcode2020.PassportValidator04
{
    public class PassportFieldChecker
    {
        private readonly string[] _requiredFields;

        private readonly Dictionary<string, IFieldValidator[]> _fieldValidators;

        public PassportFieldChecker(string[] requiredFields, Dictionary<string, IFieldValidator[]> fieldValidators)
        {
            _requiredFields = requiredFields;
            _fieldValidators = fieldValidators;
        }

        public bool HasRequiredFields(string passportString)
        {
            var passportFields = passportString
                                    .Split(InputHelper.LineEnding)
                                    .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                                    .SelectMany(x => x)
                                    .Select(keyValue => keyValue.Split(':', 2, StringSplitOptions.RemoveEmptyEntries)[0])
                                    .ToArray();

            return _requiredFields.All(field => passportFields.Contains(field));                
        }

        public bool Validate(string passportString)
        {
            var passportData = passportString
                                    .Split(InputHelper.LineEnding)
                                    .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                                    .SelectMany(x => x)
                                    .Select(kvpString => kvpString.Split(':', 2, StringSplitOptions.RemoveEmptyEntries))
                                    .ToDictionary(x => x[0], x => x[1]);

            if (_requiredFields.Any(field => !passportData.Keys.Contains(field)))
                return false;

            if (_fieldValidators == null)
                return true;

            foreach (var fieldValidations in _fieldValidators)
            {
                var fieldValue = passportData[fieldValidations.Key];
                var validators = fieldValidations.Value;

                if (validators.Any(validator => !validator.Validate(fieldValue)))
                    return false;
            }

            return true;
        }
    }
}
