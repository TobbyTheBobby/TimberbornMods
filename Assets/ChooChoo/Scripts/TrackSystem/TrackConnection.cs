using Timberborn.Coordinates;
using UnityEngine;

namespace ChooChoo.TrackSystem
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

        public Vector3Int Coordinates { get; set; }

        public Direction2D Direction { get; }

        public TrackPiece ConnectedTrackPiece { get; set; }

        public TrackRoute[] ConnectedTrackRoutes { get; set; }

        public TrackConnection CreateCopy()
        {
            return new TrackConnection(Coordinates, Direction);
        }
    }
}