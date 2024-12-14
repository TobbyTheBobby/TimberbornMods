using ChooChoo.Extensions;
using Timberborn.Coordinates;
using UnityEngine;

namespace ChooChoo.TrackSystem
{
    public class PositionedTrackConnection
    {
        public Vector3Int Coordinates { get; }

        public Direction2D Direction2D { get; }

        private PositionedTrackConnection(Vector3Int coordinate, Direction2D direction2D)
        {
            Coordinates = coordinate;
            Direction2D = direction2D;
        }

        public static PositionedTrackConnection From(
            TrackConnection specification,
            Vector3Int coordinates,
            Orientation orientation)
        {
            var coordinates1 = specification.Coordinates;
            var direction2D = orientation.Transform(specification.Direction.ToOppositeDirection());
            var num = (int)orientation;
            return new PositionedTrackConnection(BlockCalculations.Transform(coordinates1, coordinates, (Orientation)num), direction2D);
        }
    }
}