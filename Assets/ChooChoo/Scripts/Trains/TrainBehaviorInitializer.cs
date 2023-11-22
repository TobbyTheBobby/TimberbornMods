using Bindito.Core;
using Bindito.Unity;
using Timberborn.BehaviorSystem;
using UnityEngine;

namespace ChooChoo
{
  internal class TrainBehaviorInitializer : MonoBehaviour
  {
    private IInstantiator _instantiator;

    [Inject]
    public void InjectDependencies(IInstantiator instantiator) => _instantiator = instantiator;

    public void Awake()
    {
      InitializeExecutors();
      InitializeBehaviors();
    }

    private void InitializeExecutors()
    {
      AddExecutor<MoveToStationExecutor>();
      AddExecutor<WaitExecutor>();
    }

    private void AddExecutor<T>() where T : MonoBehaviour, IExecutor => _instantiator.AddComponent<T>(gameObject);

    private void InitializeBehaviors()
    {
      BehaviorManager component = GetComponent<BehaviorManager>();
      component.AddRootBehavior<TrainCarryRootBehavior>();
      // component.AddRootBehavior<MovePassengersBehavior>();
      component.AddRootBehavior<BringDistributableGoodTrainBehavior>();
      component.AddRootBehavior<WaitingBehavior>();
    }
  }
}
