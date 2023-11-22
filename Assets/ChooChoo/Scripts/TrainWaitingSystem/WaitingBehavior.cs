using Bindito.Core;
using System;
using Timberborn.BehaviorSystem;
using Timberborn.BlockSystem;
using Timberborn.EntitySystem;

namespace ChooChoo
{
  public class WaitingBehavior : RootBehavior, IDeletableEntity
  {
    private BlockService _blockService;
    private ClosestTrainWaitingLocationPicker _closestTrainWaitingLocationPicker;
    private MoveToStationExecutor _moveToStationExecutor;
    private WaitExecutor _waitExecutor;
    private TrainWaitingLocation _currentWaitingLocation;

    [Inject]
    public void InjectDependencies(BlockService blockService, ClosestTrainWaitingLocationPicker closestTrainWaitingLocationPicker)
    {
      _blockService = blockService;
      _closestTrainWaitingLocationPicker = closestTrainWaitingLocationPicker;
    }
    
    public void Awake()
    {
      _moveToStationExecutor = GetComponentFast<MoveToStationExecutor>();
      _waitExecutor = GetComponentFast<WaitExecutor>();
    }

    public override Decision Decide(BehaviorAgent agent)
    {
      var trainWaitingLocation = _blockService.GetFloorObjectComponentAt<TrainWaitingLocation>(TransformFast.position.ToBlockServicePosition());
      if (_currentWaitingLocation != null && _currentWaitingLocation == trainWaitingLocation)
      {
        _waitExecutor.LaunchForIdleTime();
        return Decision.ReleaseWhenFinished(_waitExecutor);
      }
      if (trainWaitingLocation != null && !trainWaitingLocation.Occupied)
        return OccupyWaitingLocation(trainWaitingLocation);
      return GoToClosestWaitingLocation();
    }
    
    public void DeleteEntity()
    {
      if (_currentWaitingLocation != null)
        _currentWaitingLocation.UnOccupy();
    }

    private Decision OccupyWaitingLocation(TrainWaitingLocation trainWaitingLocation)
    {
      if (_currentWaitingLocation != null)
        _currentWaitingLocation.UnOccupy();
      _currentWaitingLocation = trainWaitingLocation;
      if (_currentWaitingLocation == null)
        return Decision.ReleaseNow();
      _currentWaitingLocation.Occupy(GameObjectFast);
      return GoToWaitingLocation(_currentWaitingLocation.TrainDestinationComponent);
    }
    
    private Decision GoToWaitingLocation(TrainDestination trainDestination)
    {
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

    private Decision GoToClosestWaitingLocation()
    {
      var closestWaitingLocation = _closestTrainWaitingLocationPicker.ClosestWaitingLocation(TransformFast.position);
      return OccupyWaitingLocation(closestWaitingLocation);
    }
  }
}
