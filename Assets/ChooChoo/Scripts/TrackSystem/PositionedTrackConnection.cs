using Timberborn.BlockSystem;
using Timberborn.Coordinates;
using UnityEngine;

namespace ChooChoo
{
    public class PositionedTrackConnection
    {
        public Vector3Int Coordinates { get; }

        public Direction2D Direction2D { get; }
        
        public PositionedTrackConnection(Vector3Int coordinate, Direction2D direction2D)
        {
            Coordinates = coordinate;
            Direction2D = direction2D;
        }

        public static PositionedTrackConnection From(
            TrackConnection specification,
            Vector3Int coordinates,
            Orientation orientation)
        {
            Vector3Int coordinates1 = specification.Coordinates;
            Direction2D direction2D = orientation.Transform(specification.Direction.ToOppositeDirection());
            Vector3Int coordinates2 = coordinates;
            int num = (int) orientation;
            return new PositionedTrackConnection(BlockCalculations.Transform(coordinates1, coordinates2, (Orientation) num), direction2D);
        }
    }
}