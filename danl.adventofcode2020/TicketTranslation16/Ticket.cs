using System;
using System.Collections.Generic;

namespace danl.adventofcode2020.TicketTranslation16
{
    public class Ticket
    {
        private readonly Dictionary<string, int> FieldNameToIndexMapping;
        private readonly int[] _fieldValues;

        public Ticket(Dictionary<string, int> ColumnNameMappings, int[] fieldValues)
        {
            if (ColumnNameMappings == null)
                throw new ArgumentNullException(nameof(ColumnNameMappings));

            if (fieldValues == null)
                throw new ArgumentNullException(nameof(fieldValues));

            FieldNameToIndexMapping = ColumnNameMappings;
            _fieldValues = fieldValues;
        }

        public int this[string field] {
            get
            {
                if (!FieldNameToIndexMapping.ContainsKey(field) || FieldNameToIndexMapping[field] == -1)
                    throw new ArgumentOutOfRangeException();

                return _fieldValues[FieldNameToIndexMapping[field]];
            }
        }

        public int this[int index]
        {
            get
            {
                return _fieldValues[index];
            }
        }

        public IEnumerable<string> FieldNames
        {
            get
            {
                foreach (var kvp in FieldNameToIndexMapping)
                {
                    if (kvp.Value != -1)
                        yield return kvp.Key;
                }
            }
        }

        public IEnumerable<int> Values
        {
            get
            {
                foreach (var value in _fieldValues)
                    yield return value;
            }
        }
    }
}
