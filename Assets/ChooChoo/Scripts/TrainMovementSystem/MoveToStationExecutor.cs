using Bindito.Core;
using Timberborn.BehaviorSystem;
using Timberborn.Persistence;
using UnityEngine;

namespace ChooChoo
{
  public class MoveToStationExecutor : MonoBehaviour, IExecutor
  {
    private TrainPositionDestinationFactory _trainPositionDestinationFactory;
    
    private Machinist _machinist;

    [Inject]
    public void InjectDependencies(TrainPositionDestinationFactory trainPositionDestinationFactory)
    {
      _trainPositionDestinationFactory = trainPositionDestinationFactory;
    }

    public void Awake() => _machinist = GetComponent<Machinist>();

    public ExecutorStatus Launch(TrainDestination trainDestination) => _machinist.GoTo(_trainPositionDestinationFactory.Create(trainDestination));

    public ExecutorStatus Tick(float deltaTimeInHours)
    {
      if (!_machinist.CurrentDestinationReachable)
        return ExecutorStatus.Failure;
      return _machinist.Stopped() ? ExecutorStatus.Success : ExecutorStatus.Running;
    }

    public void Save(IEntitySaver entitySaver)
    {
    }

    public void Load(IEntityLoader entityLoader)
    {
    }
  }
}
