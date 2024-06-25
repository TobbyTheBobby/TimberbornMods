// using Bindito.Core;
// using ChooChoo.MovementSystem;
// using ChooChoo.NavigationSystem;
// using Timberborn.BehaviorSystem;
// using Timberborn.Localization;
// using Timberborn.StatusSystem;
// using Timberborn.TickSystem;
//
// namespace ChooChoo.WaitingSystemUI
// {
//     public class NoWaitingStationStatus : TickableComponent
//     {
//         private static readonly string UnconnectedLocKey = "Tobbert.TrainNavigation.NoWaitingStation";
//         private static readonly string UnconnectedShortLocKey = "Tobbert.TrainNavigation.NoWaitingStationShort";
//         private TrainDestinationService _trainDestinationService;
//         private ILoc _loc;
//         private BehaviorManager _behaviorManager;
//         private StatusToggle _unconnectedTrainDestinationStatusToggle;
//         private bool _isUnconnectedToAnyTrainDestination;
//         
//         [Inject]
//         public void InjectDependencies(TrainDestinationService trainDestinationService, ILoc loc)
//         {
//             _trainDestinationService = trainDestinationService;
//             _loc = loc;
//         }
//
//         public void Awake()
//         {
//             _behaviorManager = GetComponentFast<BehaviorManager>();
//             _unconnectedTrainDestinationStatusToggle = StatusToggle.CreateNormalStatusWithAlertAndFloatingIcon(
//                 "tobbert.choochoo/tobbert_choochoo/NoWaitingStationStatusIcon", _loc.T(UnconnectedLocKey), _loc.T(UnconnectedShortLocKey));
//         }
//
//         public override void StartTickable()
//         {
//             GetComponentFast<StatusSubject>().RegisterStatus(_unconnectedTrainDestinationStatusToggle);
//             CheckIfConnectedToTrainDestination();
//             UpdateStatusToggle();
//         }
//
//         public override void Tick()
//         {
//             CheckIfConnectedToTrainDestination();
//             UpdateStatusToggle();
//         }
//
//         private void UpdateStatusToggle()
//         {
//             if (_behaviorManager.RunningExecutor.Name == nameof(WaitExecutor) || _behaviorManager.RunningExecutor.Name == nameof(MoveToStationExecutor))
//                 _unconnectedTrainDestinationStatusToggle.Deactivate();
//             else
//                 _unconnectedTrainDestinationStatusToggle.Activate();
//         }
//
//         private void CheckIfConnectedToTrainDestination()
//         {
//             var connectedTrainDestinations = _trainDestinationService.GetConnectedTrainDestinations(_behaviorManager);
//             _isUnconnectedToAnyTrainDestination = connectedTrainDestinations.Count <= 1;
//         }
//     }
// }