// using Bindito.Core;
// using Timberborn.BehaviorSystem;
// using Timberborn.Persistence;
// using UnityEngine;
//
// namespace GlobalMarket
// {
//   public class FlyToPositionExecutor : MonoBehaviour, IExecutor
//   {
//     private AirPositionDestinationFactory _airPositionDestinationFactory;
//     private Pilot _pilot;
//
//     [Inject]
//     public void InjectDependencies(AirPositionDestinationFactory airPositionDestinationFactory)
//     {
//       _airPositionDestinationFactory = airPositionDestinationFactory;
//     }
//
//     public void Awake() => _pilot = GetComponent<Pilot>();
//
//     public ExecutorStatus Launch(Vector3 position) => _pilot.GoTo(_airPositionDestinationFactory.Create(position));
//
//     public ExecutorStatus Tick(float deltaTimeInHours)
//     {
//       if (!_pilot.CurrentDestinationReachable)
//         return ExecutorStatus.Failure;
//       return _pilot.Stopped() ? ExecutorStatus.Success : ExecutorStatus.Running;
//     }
//
//     public void Save(IEntitySaver entitySaver)
//     {
//     }
//
//     public void Load(IEntityLoader entityLoader)
//     {
//     }
//   }
// }
