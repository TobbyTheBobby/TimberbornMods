using Timberborn.Coordinates;
using Timberborn.Persistence;
using UnityEngine;

namespace ChooChoo
{
  public class TrackConnectionObjectSerializer : IObjectSerializer<TrackConnection>
  {
    private static readonly PropertyKey<Vector3Int> CoordinatesKey = new("Coordinates");
    private static readonly PropertyKey<Direction2D> DirectionKey = new("Direction");
    private static readonly PropertyKey<TrackPiece> ConnectedTrackPieceKey = new("ConnectedTrackPiece");

    private readonly EnumObjectSerializer<Direction2D> _direction2DSerializer;

    public TrackConnectionObjectSerializer(EnumObjectSerializer<Direction2D> direction2DSerializer)
    {
      _direction2DSerializer = direction2DSerializer;
    }

    public void Serialize(TrackConnection value, IObjectSaver objectSaver)
    {
      objectSaver.Set(CoordinatesKey, value.Coordinates);
      objectSaver.Set(DirectionKey, value.Direction, _direction2DSerializer);
      if (value.ConnectedTrackPiece != null)
        objectSaver.Set(ConnectedTrackPieceKey, value.ConnectedTrackPiece);
    }

    public Obsoletable<TrackConnection> Deserialize(IObjectLoader objectLoader)
    {
      var trackConnection = new TrackConnection(objectLoader.Get(CoordinatesKey), objectLoader.Get(DirectionKey, _direction2DSerializer));
      if (objectLoader.Has(ConnectedTrackPieceKey))
        trackConnection.ConnectedTrackPiece = objectLoader.Get(ConnectedTrackPieceKey);
      return trackConnection;
    }
  }
}
