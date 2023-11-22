using System;
using Timberborn.Coordinates;

namespace ChooChoo
{
    public static class FloatExtensions
    {
        public static Direction2D ToDirection2D(this float @float)
        {
            if (@float is <= 45 or > 315)
                return Direction2D.Down;
            if (@float > 225) 
                return Direction2D.Right;
            if (@float > 135 ) 
                return Direction2D.Up;
            if (@float > 45) 
                return Direction2D.Left;
            
            throw new ArgumentOutOfRangeException(nameof(@float), @float, null);
        }
    }
}