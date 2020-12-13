using System;
using System.Collections.Generic;
using System.Text;

namespace danl.adventofcode2020.RainRisk12
{
    public interface IShip
    {
        public int X { get; }
        public int Y { get; }
        public void Move(Movement movement);
    }

    public class Ship : IShip
    {
        public int X { get; private set; }

        public int Y { get; private set; }

        public Direction CurrentDirection { get; private set; }

        public Ship()
        {
            X = 0;
            Y = 0;
            CurrentDirection = Direction.East;
        }

        public void Move(Movement movement)
        {
            switch (movement.MovementType)
            {
                case MovementType.North:
                    Y = Y + movement.Value;
                    break;
                case MovementType.East:
                    X = X + movement.Value;
                    break;
                case MovementType.South:
                    Y = Y - movement.Value;
                    break;
                case MovementType.West:
                    X = X - movement.Value;
                    break;
                case MovementType.Left:
                    Rotate(360 - movement.Value);
                    break;
                case MovementType.Right:
                    Rotate(movement.Value);
                    break;
                case MovementType.Forward:
                    Move(new Movement { MovementType = DirectionToMovementType[CurrentDirection], Value = movement.Value });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Rotate(int degrees)
        {
            var steps = degrees / 90;

            while (steps-- > 0)
                CurrentDirection = DirectionStep[CurrentDirection];
        }
        

        private static readonly Dictionary<Direction, MovementType> DirectionToMovementType = new Dictionary<Direction, MovementType>() {
            { Direction.North, MovementType.North },
            { Direction.East, MovementType.East },
            { Direction.South, MovementType.South },
            { Direction.West, MovementType.West },
        };

        private static readonly Dictionary<Direction, Direction> DirectionStep = new Dictionary<Direction, Direction>()
        {
            { Direction.East, Direction.South },
            { Direction.South, Direction.West },
            { Direction.West, Direction.North },
            { Direction.North, Direction.East },
        };

        public enum Direction
        {
            North,
            East,
            South,
            West
        }
    }

    public class WaypointShip : IShip
    {
        public int X { get; private set; }

        public int Y { get; private set; }

        public int WayPointX { get; private set; }

        public int WayPointY { get; private set; }

        public WaypointShip()
        {
            X = 0;
            Y = 0;
            WayPointX = 10;
            WayPointY = 1;
        }

        public void Move(Movement movement)
        {
            switch (movement.MovementType)
            {
                case MovementType.North:
                    WayPointY = WayPointY + movement.Value;
                    break;
                case MovementType.East:
                    WayPointX = WayPointX + movement.Value;
                    break;
                case MovementType.South:
                    WayPointY = WayPointY - movement.Value;
                    break;
                case MovementType.West:
                    WayPointX = WayPointX - movement.Value;
                    break;
                case MovementType.Left:
                    Rotate(360 - movement.Value);
                    break;
                case MovementType.Right:
                    Rotate(movement.Value);
                    break;
                case MovementType.Forward:
                    var steps = movement.Value;
                    while (steps-- > 0)
                    {
                        X = X + WayPointX;
                        Y = Y + WayPointY;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Rotate(int degrees)
        {
            var steps = degrees / 90;

            while (steps-- > 0)
            {
                var tmp = -1 * WayPointX;
                WayPointX = WayPointY;
                WayPointY = tmp;
            }   
        }
    }
}
