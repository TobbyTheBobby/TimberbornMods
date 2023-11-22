using Timberborn.Persistence;
using UnityEngine;

namespace ChooChoo
{
  public class TrackRouteObjectSerializer : IObjectSerializer<TrackRoute>
  {
    private static readonly PropertyKey<TrackConnection> EntranceKey = new("Entrance");
    private static readonly PropertyKey<TrackConnection> ExitKey = new("Exit");
    private static readonly ListKey<Vector3> RouteCornersKey = new("RouteCorners");

    private readonly TrackConnectionObjectSerializer _trackConnectionObjectSerializer;

    public TrackRouteObjectSerializer(TrackConnectionObjectSerializer trackConnectionObjectSerializer)
    {
      _trackConnectionObjectSerializer = trackConnectionObjectSerializer;
    }

    public void Serialize(TrackRoute value, IObjectSaver objectSaver)
    {
      objectSaver.Set(EntranceKey, value.Entrance, _trackConnectionObjectSerializer);
      objectSaver.Set(ExitKey, value.Exit, _trackConnectionObjectSerializer);
      objectSaver.Set(RouteCornersKey, value.RouteCorners);
    }

    public Obsoletable<TrackRoute> Deserialize(IObjectLoader objectLoader)
    {
      var trackConnection = new TrackRoute(
        objectLoader.Get(EntranceKey, _trackConnectionObjectSerializer),
        objectLoader.Get(ExitKey, _trackConnectionObjectSerializer),
        objectLoader.Get(RouteCornersKey).ToArray());
      return trackConnection;
    }
  }
}
