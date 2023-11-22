using Bindito.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Timberborn.BehaviorSystem;
using Timberborn.BlockSystem;
using Timberborn.EntitySystem;

namespace ChooChoo
{
  public class MovePassengersBehavior : RootBehavior, IDeletableEntity
  {
    private TrainDestinationsRepository _trainDestinationsRepository;
    private BlockService _blockService;
    private MoveToStationExecutor _moveToStationExecutor;
    private TrainWaitingLocation _currentWaitingLocation;

    private readonly List<Passenger> _passengers = new();
    private readonly List<Passenger> _reservedPassengers = new();

    public List<Passenger> Passengers => _passengers;

    [Inject]
    public void InjectDependencies(TrainDestinationsRepository trainDestinationsRepository, BlockService blockService)
    {
      _trainDestinationsRepository = trainDestinationsRepository;
      _blockService = blockService;
    }
    
    public void Awake()
    {
      _moveToStationExecutor = GetComponentFast<MoveToStationExecutor>();
    }

    public override Decision Decide(BehaviorAgent agent)
    {
      // Plugin.Log.LogInfo("Trying to move passengers");
      var passengerStations = _trainDestinationsRepository.TrainDestinations.Where(destionation => destionation.TryGetComponentFast(out PassengerStation _));

      var currentPassengerStation = _blockService.GetFloorObjectComponentAt<PassengerStation>(TransformFast.position.ToBlockServicePosition());

      if (_passengers.Any())
      {
        var firstPassenger = _passengers.First();
        if (firstPassenger.IsWaiting && firstPassenger.PassengerStationLink.ValidLink())
        {
          ForcedPassengerDropOff();
          return Decision.ReleaseNow();
        }
        var destination = _passengers.First().PassengerStationLink.EndLinkPoint;
        if (currentPassengerStation == destination)
        {
          DropPassengersOff(currentPassengerStation);
          return Decision.ReleaseNow();
        }

        switch (_moveToStationExecutor.Launch(destination.GetComponentFast<TrainDestination>()))
        {
          case ExecutorStatus.Success:
            return Decision.ReleaseNow();
          case ExecutorStatus.Failure:
            ForcedPassengerDropOff();
            return Decision.ReleaseNow();
          case ExecutorStatus.Running:
            return Decision.ReturnWhenFinished(_moveToStationExecutor);
          default:
            throw new ArgumentOutOfRangeException();
        }
      }

      if (_reservedPassengers.Any())
      {
        var passenger = _reservedPassengers.First();
        if (passenger.PassengerStationLink == null || !passenger.PassengerStationLink.ValidLink())
        {
          ForcedPassengerDropOff();
          return Decision.ReleaseNow();
        }
        if (passenger.PassengerStationLink.StartLinkPoint == currentPassengerStation)
        {
          LoadPassengers(currentPassengerStation);
          return Decision.ReleaseNow();
        }
      }

      foreach (var trainDestination in passengerStations)
      {
        var passengerStation = trainDestination.GetComponentFast<PassengerStation>();
        
        if (passengerStation.UnreservedPassengerQueue.Any())
        {
          ReservePassengers(passengerStation);
          switch (_moveToStationExecutor.Launch(trainDestination))
          {
            case ExecutorStatus.Success:
              return Decision.ReleaseNow();
            case ExecutorStatus.Failure:
              return Decision.ReleaseNow();
            case ExecutorStatus.Running:
              return Decision.ReturnWhenFinished(_moveToStationExecutor);
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
      }
      
      return Decision.ReleaseNow();
    }
    
    public void DeleteEntity()
    {
      UnreservePassenger();
      ForcedPassengerDropOff();
    }
    
    private void DropPassengersOff(PassengerStation passengerStation)
    {
      // Plugin.Log.LogInfo("Dropping off passengers");
      foreach (var passenger in _passengers.ToList())
      {
        if (passenger.PassengerStationLink.EndLinkPoint == passengerStation)
        {
          passenger.ArrivedAtDestination();
          _passengers.Remove(passenger);
        }
      }
    }
    
    private void ForcedPassengerDropOff()
    {
      // Plugin.Log.LogInfo("Dropping off passengers");
      foreach (var passenger in _passengers.ToList())
      {
        passenger.ArrivedAtDestination();
        _passengers.Remove(passenger);
      }
    }
    
    private void ReservePassengers(PassengerStation passengerStation)
    {      
      // Plugin.Log.LogInfo("Reserving Up passengers");
      var unreservedPassengers = passengerStation.UnreservedPassengerQueue;
      passengerStation.ReservedPassengerQueue.AddRange(unreservedPassengers);
      _reservedPassengers.AddRange(unreservedPassengers);
    }
    
    private void UnreservePassenger()
    {      
      // Plugin.Log.LogInfo("Unreserving passengers");
      foreach (var passenger in _reservedPassengers)
      {
        if (passenger.PassengerStationLink == null)
          continue;
        var passengerStation = passenger.PassengerStationLink.StartLinkPoint;
        passengerStation.ReservedPassengerQueue.Remove(passenger);
      }
      _reservedPassengers.Clear();
    }

    private void LoadPassengers(PassengerStation passengerStation)
    {
      // Plugin.Log.LogInfo("Picking Up passengers");
      foreach (var passenger in _reservedPassengers)
      {
        passengerStation.PassengerQueue.Remove(passenger);
        passengerStation.ReservedPassengerQueue.Remove(passenger);
        _passengers.Add(passenger);
      }
      _reservedPassengers.Clear();
      foreach (var passenger in passengerStation.UnreservedPassengerQueue)
      {
        passengerStation.PassengerQueue.Remove(passenger);
        _passengers.Add(passenger);
      }
    }
  }
}
