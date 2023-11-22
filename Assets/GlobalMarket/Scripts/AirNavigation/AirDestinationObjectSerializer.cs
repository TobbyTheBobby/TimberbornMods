using System;
using Timberborn.Persistence;
using UnityEngine;

namespace GlobalMarket
{
  public class AirDestinationObjectSerializer : IObjectSerializer<IAirDestination>
  {
    private static readonly PropertyKey<Vector3> PositionKey = new("Position");
    private readonly AirPositionDestinationFactory _airPositionDestinationFactory;

    public AirDestinationObjectSerializer(AirPositionDestinationFactory airAirPositionDestinationFactory)
    {
      _airPositionDestinationFactory = airAirPositionDestinationFactory;
    }

    public void Serialize(IAirDestination value, IObjectSaver objectSaver)
    {
      AirPositionDestination positionDestination = value as AirPositionDestination;
      ConvertPositionDestination(positionDestination, objectSaver);
    }

    public Obsoletable<IAirDestination> Deserialize(IObjectLoader objectLoader)
    {
      return _airPositionDestinationFactory.Create(objectLoader.Get(PositionKey));
    }

    private static void ConvertPositionDestination(AirPositionDestination positionDestination, IObjectSaver objectSaver)
    {
      objectSaver.Set(PositionKey, positionDestination.Destination);
    }
  }
}
