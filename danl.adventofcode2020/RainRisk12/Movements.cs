using System;
using System.Collections.Generic;

namespace danl.adventofcode2020.RainRisk12
{
    public class Movement
    {
        public static readonly Dictionary<char, MovementType> CharToMovementType = new Dictionary<char, MovementType>()
        {
            {'N', MovementType.North }, {'E', MovementType.East }, {'S', MovementType.South }, {'W', MovementType.West },
            {'L', MovementType.Left }, {'R', MovementType.Right }, {'F', MovementType.Forward }
        };

        public Movement(string movementString)
        {
            if (string.IsNullOrWhiteSpace(movementString))
                throw new ArgumentNullException(nameof(movementString));

            if (!CharToMovementType.ContainsKey(movementString[0]))
                throw new ArgumentException();

            MovementType = CharToMovementType[movementString[0]];

            Value = int.Parse(movementString.Substring(1));
        }

        public Movement() { }

        public int Value { get; set; }
        public MovementType MovementType{ get; set; }
    }

    public enum MovementType
    {
        North,
        East,
        South,
        West,
        Left,
        Right,
        Forward
    }
}
