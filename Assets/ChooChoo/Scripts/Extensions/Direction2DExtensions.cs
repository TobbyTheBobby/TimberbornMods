using System;
using Timberborn.Coordinates;

namespace ChooChoo
{
    public static class Direction2DExtensions
    {
        public static Direction2D ToOppositeDirection(this Direction2D direction2D)
        {
            switch (direction2D)
            {
                case Direction2D.Down:
                    return Direction2D.Up;
                case Direction2D.Left:
                    return Direction2D.Right;
                case Direction2D.Up:
                    return Direction2D.Down;
                case Direction2D.Right:
                    return Direction2D.Left;
                default:
                    throw new ArgumentOutOfRangeException(nameof (direction2D), direction2D, null);
            }
        }
        
        public static Direction2D CorrectedNext(this Direction2D direction2D)
        {
            switch (direction2D)
            {
                case Direction2D.Down:
                    return Direction2D.Right;
                case Direction2D.Right:
                    return Direction2D.Up;
                case Direction2D.Up:
                    return Direction2D.Left;
                case Direction2D.Left:
                    return Direction2D.Down;
                default:
                    throw new ArgumentOutOfRangeException(nameof (direction2D), (object) direction2D, (string) null);
            }
        }
    }
}
