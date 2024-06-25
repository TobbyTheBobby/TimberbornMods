using System;
using Timberborn.Coordinates;
using UnityEngine;

namespace ChooChoo.Extensions
{
    public static class OrientationExtensions
    {
        public static Direction2D CorrectedTransform(
            this Orientation orientation,
            Direction2D direction2D)
        {
            switch (orientation)
            {
                case Orientation.Cw0:
                    return direction2D;
                case Orientation.Cw90:
                    return direction2D.CorrectedNext();
                case Orientation.Cw180:
                    return direction2D.CorrectedNext().CorrectedNext();
                case Orientation.Cw270:
                    return direction2D.CorrectedNext().CorrectedNext().CorrectedNext();
                default:
                    throw new ArgumentException($"Unexpected {(object)"Orientation"}: {(object)orientation}");
            }
        }

        public static Vector3Int Untransform(this Orientation orientation, Vector3Int vector)
        {
            switch (orientation)
            {
                case Orientation.Cw0:
                    return vector;
                case Orientation.Cw90:
                    return new Vector3Int(-vector.y, vector.x, vector.z);
                case Orientation.Cw180:
                    return new Vector3Int(-vector.x, -vector.y, vector.z);
                case Orientation.Cw270:
                    return new Vector3Int(vector.y, -vector.x, vector.z);
                default:
                    throw new ArgumentException($"Unexpected {(object)"Orientation"}: {(object)orientation}");
            }
        }
    }
}