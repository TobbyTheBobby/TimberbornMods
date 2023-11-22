// using Bindito.Core;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Timberborn.BehaviorSystem;
// using Timberborn.CharacterMovementSystem;
// using Timberborn.CharacterNavigation;
// using Timberborn.Common;
// using Timberborn.Navigation;
// using Timberborn.Persistence;
// using Timberborn.TickSystem;
// using Timberborn.TimeSystem;
// using Timberborn.WalkingSystem;
// using UnityEngine;
//
// namespace GlobalMarket
// {
//   public class Pilot : TickableComponent, IPersistentEntity
//   {
//     private static readonly ComponentKey PilotKey = new(nameof (Pilot));
//     private static readonly PropertyKey<IAirDestination> CurrentDestinationKey = new("CurrentDestination");
//     private static readonly float SecondsPerDistanceUnit = 1f;
//     private FlightNavigationService _flightNavigationService;
//     private IDayNightCycle _dayNightCycle;
//     private PathFollowerFactory _pathFollowerFactory;
//     private AirDestinationObjectSerializer _airDestinationObjectSerializer;
//     private WalkerSpeedManager _walkerSpeedManager;
//     private PathFollower _pathFollower;
//     private readonly List<Vector3> _pathCorners = new(100);
//     private readonly List<Vector3> _tempPathCorners = new(100);
//     private IAirDestination _currentAirDestination;
//     private IAirDestination _previousAirDestination;
//
//     public event EventHandler<StartedNewPathEventArgs> StartedNewPath;
//
//     public bool CurrentDestinationReachable { get; private set; }
//
//     public IReadOnlyList<Vector3> PathCorners { get; private set; }
//
//     public BoundingBox CurrentPathBounds { get; private set; }
//
//     [Inject]
//     public void InjectDependencies(
//       FlightNavigationService flightNavigationService,
//       IDayNightCycle dayNightCycle,
//       PathFollowerFactory pathFollowerFactory,
//       AirDestinationObjectSerializer airDestinationObjectSerializer)
//     {
//       _flightNavigationService = flightNavigationService;
//       _dayNightCycle = dayNightCycle;
//       _pathFollowerFactory = pathFollowerFactory;
//       _airDestinationObjectSerializer = airDestinationObjectSerializer;
//     }
//
//     public void Awake()
//     {
//       _walkerSpeedManager = GetComponent<WalkerSpeedManager>();
//       _pathFollower = _pathFollowerFactory.Create(gameObject);
//       PathCorners = _pathCorners.AsReadOnly();
//     }
//
//     public override void Tick()
//     {
//       if (Stopped())
//         return;
//       if (_pathFollower.ReachedLastPathCorner())
//         Stop();
//       else
//         Move();
//     }
//
//     public ExecutorStatus GoTo(IAirDestination airDestination)
//     {
//       _previousAirDestination = _currentAirDestination;
//       int path = (int) FindPath(airDestination);
//       if (path != 2)
//         return (ExecutorStatus) path;
//       return (ExecutorStatus) path;
//     }
//
//     public void Stop()
//     {
//       _previousAirDestination = _currentAirDestination;
//       _currentAirDestination = null;
//       _pathFollower.StopMoving();
//     }
//
//     public bool Stopped() => _currentAirDestination == null;
//
//     public void Save(IEntitySaver entitySaver)
//     {
//       IObjectSaver component = entitySaver.GetComponent(PilotKey);
//       if (_currentAirDestination != null)
//         component.Set(CurrentDestinationKey, _currentAirDestination, _airDestinationObjectSerializer);
//     }
//
//     public void Load(IEntityLoader entityLoader)
//     {
//       IObjectLoader component = entityLoader.GetComponent(PilotKey);
//       if (component.Has(CurrentDestinationKey))
//       {
//         _currentAirDestination = component.Get(CurrentDestinationKey, _airDestinationObjectSerializer);
//         FindPath(_currentAirDestination);
//       }
//     }
//
//     private ExecutorStatus FindPath(IAirDestination airDestination)
//     {
//       if (!HasSavedPathToDestination(airDestination))
//       {
//         Vector3 start = transform.position;
//         _pathCorners.Clear();
//         CurrentDestinationReachable = airDestination.GeneratePath(start, _tempPathCorners);
//         if (CurrentDestinationReachable)
//         {
//           if (!_pathCorners.IsEmpty())
//             _pathCorners.RemoveLast();
//           _pathCorners.AddRange(_tempPathCorners);
//           _tempPathCorners.Clear();
//         }
//         else
//           _pathCorners.Clear();
//         _pathFollower.StartMovingAlongPath(_pathCorners);
//         EventHandler<StartedNewPathEventArgs> startedNewPath = StartedNewPath;
//         if (startedNewPath != null)
//           startedNewPath(this, new StartedNewPathEventArgs(100));
//       }
//       if (CurrentDestinationReachable)
//       {
//         _currentAirDestination = airDestination;
//         RecalculatePathBounds();
//         return !_pathFollower.ReachedLastPathCorner() ? ExecutorStatus.Running : ExecutorStatus.Success;
//       }
//       Stop();
//       return ExecutorStatus.Failure;
//     }
//
//     private void RecalculatePathBounds()
//     {
//       BoundingBox.Builder builder = new BoundingBox.Builder();
//       for (int index = 0; index < _pathCorners.Count; ++index)
//         builder.Expand(NavigationCoordinateSystem.WorldToGridInt(_pathCorners[index]));
//       CurrentPathBounds = builder.Build();
//     }
//
//     private bool HasSavedPathToDestination(IAirDestination airDestination) => Equals(_previousAirDestination, airDestination);
//
//     private void Move()
//     {
//       var speed = _walkerSpeedManager.Speed * CalculateSpeedReductionAtStartAndEnd();
//       _pathFollower.MoveAlongPath(Time.fixedDeltaTime, "Walking", speed);
//     }
//
//     private float CalculateSpeedReductionAtStartAndEnd()
//     {
//       var start = SlowdownStart();
//       var end = SlowdownEnd();
//
//       return start * end;
//     }
//
//     private float SlowdownStart()
//     {
//       var distanceFromStart = Vector3.Distance(_pathCorners[0], transform.position);
//       if (distanceFromStart > 3)
//       {
//         return 1;
//       }
//
//       return distanceFromStart / 3 + 0.2f;
//     }
//     
//     private float SlowdownEnd()
//     {
//       var distanceToEnd = Vector3.Distance(transform.position, _pathCorners.Last());
//       if (distanceToEnd > 3)
//       {
//         return 1;
//       }
//
//       return distanceToEnd / 3 + 0.2f;
//     }
//   }
// }
