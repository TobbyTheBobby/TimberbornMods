using Timberborn.Persistence;
using UnityEngine;

namespace ChooChoo
{
  public class TrackRouteObjectDeserializer : IObjectSerializer<TrackRoute>
  {
    private static readonly PropertyKey<TrackConnection> EntranceKey = new("Entrance");
    private static readonly PropertyKey<TrackConnection> ExitKey = new("Exit");
    private static readonly ListKey<Vector3> RouteCornersKey = new("RouteCorners");

    private readonly TrackConnectionObjectDeserializer _trackConnectionObjectDeserializer;

    public TrackRouteObjectDeserializer(TrackConnectionObjectDeserializer trackConnectionObjectDeserializer)
    {
      _trackConnectionObjectDeserializer = trackConnectionObjectDeserializer;
    }

    public void Serialize(TrackRoute value, IObjectSaver objectSaver)
    {
      objectSaver.Set(EntranceKey, value.Entrance, _trackConnectionObjectDeserializer);
      objectSaver.Set(ExitKey, value.Exit, _trackConnectionObjectDeserializer);
      objectSaver.Set(RouteCornersKey, value.RouteCorners);
    }

    public Obsoletable<TrackRoute> Deserialize(IObjectLoader objectLoader)
    {
      var trackConnection = new TrackRoute(
        objectLoader.Get(EntranceKey, _trackConnectionObjectDeserializer),
        objectLoader.Get(ExitKey, _trackConnectionObjectDeserializer),
        objectLoader.Get(RouteCornersKey).ToArray());
      return trackConnection;
    }
  }
}
