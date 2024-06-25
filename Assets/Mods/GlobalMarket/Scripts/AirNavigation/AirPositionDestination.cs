using System;
using System.Collections.Generic;
using UnityEngine;

namespace GlobalMarket
{
  public class AirPositionDestination : IAirDestination, IEquatable<AirPositionDestination>
  {
    private readonly FlightNavigationService _flightNavigationService;

    public Vector3 Destination { get; }

    public AirPositionDestination(FlightNavigationService flightNavigationService, Vector3 destination)
    {
      _flightNavigationService = flightNavigationService;
      Destination = destination;
    }

    public bool GeneratePath(Vector3 start, List<Vector3> pathCorners)
    {
      return _flightNavigationService.GenerateRandomFlyingPath(start, Destination, pathCorners);
    }

    public bool Equals(AirPositionDestination other)
    {
      if ((object) other == null)
        return false;
      if ((object) this == (object) other)
        return true;
      return Destination.Equals(other.Destination);
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if ((object) this == obj)
        return true;
      return !(obj.GetType() != GetType()) && Equals((AirPositionDestination) obj);
    }

    public static bool operator ==(AirPositionDestination left, AirPositionDestination right) => object.Equals((object) left, (object) right);

    public static bool operator !=(AirPositionDestination left, AirPositionDestination right) => !object.Equals((object) left, (object) right);
  }
}
