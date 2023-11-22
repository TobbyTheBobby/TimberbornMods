using Bindito.Core;
using System;
using Timberborn.BehaviorSystem;
using Timberborn.BlockSystem;

namespace ChooChoo
{
  public class ReturnToTrainYardBehavior : RootBehavior
  {
    private BlockService _blockService;
    private MoveToStationExecutor _moveToStationExecutor;
    private WaitExecutor _waitExecutor;
    private TrainYardSubject _trinYardSubject;

    [Inject]
    public void InjectDependencies(BlockService blockService)
    {
      _blockService = blockService;
    }
    
    public void Awake()
    {
      _moveToStationExecutor = GetComponentFast<MoveToStationExecutor>();
      _waitExecutor = GetComponentFast<WaitExecutor>();
      _trinYardSubject = GetComponentFast<TrainYardSubject>();
    }

    public override Decision Decide(BehaviorAgent agent)
    {
      var currentTrainDestination = _blockService.GetFloorObjectComponentAt<TrainDestination>(TransformFast.position.ToBlockServicePosition());
      if (currentTrainDestination == _trinYardSubject.HomeTrainYard)
      {
        _waitExecutor.LaunchForSpecifiedTime(1);
        return Decision.ReleaseWhenFinished(_waitExecutor);
      }
    
      return ReturnToOriginalTrainYard();
    }

    private Decision ReturnToOriginalTrainYard()
    {
     // Plugin.Log.LogWarning( "Returning Home");
      switch (_moveToStationExecutor.Launch(_trinYardSubject.HomeTrainYard))
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
}
