using Timberborn.Coordinates;
using UnityEngine;

namespace ChooChoo
{
    public class TrackConnection
    {
        public TrackConnection(Vector3Int coordinate, Direction2D direction)
        {
            Coordinates = coordinate;
            Direction = direction;
            ConnectedTrackPiece = null;
            ConnectedTrackRoutes = null;
        }

        public Vector3Int Coordinates { get; }

        public Direction2D Direction { get; }

        public TrackPiece ConnectedTrackPiece { get; set; }
        
        public TrackRoute[] ConnectedTrackRoutes { get; set; }
    }
}