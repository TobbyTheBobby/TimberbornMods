using UnityEngine;

namespace GlobalMarket
{
  public class AirPositionDestinationFactory
  {
    private readonly FlightNavigationService _flightNavigationService;

    public AirPositionDestinationFactory(FlightNavigationService flightNavigationService)
    {
      _flightNavigationService = flightNavigationService;
    }

    public AirPositionDestination Create(Vector3 position) => new(_flightNavigationService, position);
  }
}
