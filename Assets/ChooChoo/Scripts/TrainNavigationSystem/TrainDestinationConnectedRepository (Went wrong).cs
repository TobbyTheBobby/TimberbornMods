// using System.Collections.Generic;
// using System.Security.Cryptography;
// using Timberborn.SingletonSystem;
//
// namespace ChooChoo
// {
//     public class TrainDestinationConnectedRepository : IPostLoadableSingleton
//     {
//         private readonly EventBus _eventBus;
//
//         private readonly TrainDestinationsRepository _trainDestinationsRepository;
//
//         private readonly Dictionary<TrainDestination, List<TrainDestination>> _trainDestinationConnections = new();
//
//         private bool _tracksUpdated = true;
//
//         public Dictionary<TrainDestination, List<TrainDestination>> TrainDestinations
//         {
//             get
//             {
//                 if (_tracksUpdated)
//                     Update();
//                 return _trainDestinationConnections;
//             }
//         }
//
//         TrainDestinationConnectedRepository(EventBus eventBus, TrainDestinationsRepository trainDestinationsRepository)
//         {
//             _eventBus = eventBus;
//             _trainDestinationsRepository = trainDestinationsRepository;
//         }
//
//         public void PostLoad()
//         {
//             _eventBus.Register(this);
//             Update();
//         }
//
//         [OnEvent]
//         public void OnTrackUpdate(OnTracksUpdatedEvent onTracksUpdatedEvent)
//         {
//             _tracksUpdated = true;
//         }
//
//         private void Update()
//         {
//             FindDestinationConnections();
//             _tracksUpdated = false;
//         }
//
//         private void FindDestinationConnections()
//         {
//             _trainDestinationConnections.Clear();
//             foreach (var checkingDestination in _trainDestinationsRepository.TrainDestinations)
//             {
//                 var thisTrackPiece = checkingDestination.GetComponentFast<TrackPiece>();
//                 // Plugin.Log.LogError(thisTrackPiece.name);
//                 var checkedTrackRoutes = new List<TrackRoute>();
//                 
//                 var firstConnectedTrackRoute = thisTrackPiece.TrackRoutes[0];
//                 var list1 = new List<TrainDestination>();
//                 CheckNextTrackPiece(firstConnectedTrackRoute, checkedTrackRoutes, list1);
//                 checkedTrackRoutes.Clear();
//
//                 if (thisTrackPiece.TrackRoutes.Length == 1)
//                 {
//                     _trainDestinationConnections.Add(checkingDestination, list1);
//                     return;
//                 }
//                 
//                 var secondConnectedTrackRoute = thisTrackPiece.TrackRoutes[1];
//                 var list2 = new List<TrainDestination>();
//                 CheckNextTrackPiece(secondConnectedTrackRoute, checkedTrackRoutes, list2);
//
//                 var trainDestinationsConnected = new List<TrainDestination>();
//                 
//                 // Plugin.Log.LogInfo("list1 count: " + list1.Count + "list2 count: " + list2.Count);
//                 foreach (var trainDestination in list1)
//                 {
//                     if (trainDestination == checkingDestination || trainDestination == null)
//                         continue;
//
//                     if (list2.Contains(trainDestination))
//                         trainDestinationsConnected.Add(trainDestination);
//                 }
//
//                 _trainDestinationConnections.Add(checkingDestination, trainDestinationsConnected);
//             }
//             // Plugin.Log.LogWarning(list.Count + "");
//             // foreach (var l in list)
//             // {
//             //     Plugin.Log.LogInfo(l.Count + "");
//             // }
//         }
//
//         private void CheckNextTrackPiece(TrackRoute checkingTrackRoute, List<TrackRoute> checkedTrackRoutes, List<TrainDestination> trainDestinationsConnected)
//         {
//             if (checkedTrackRoutes.Contains(checkingTrackRoute))
//                 return;
//
//             var checkingTrackPiece = checkingTrackRoute.Exit.ConnectedTrackPiece;
//
//             if (checkingTrackPiece == null) 
//                 return;
//             
//             // Plugin.Log.LogError(checkingTrackPiece.CenterCoordinates + "");
//             checkedTrackRoutes.Add(checkingTrackRoute);
//
//             if (checkingTrackPiece.TryGetComponentFast(out TrainDestination trainDestination) && !trainDestinationsConnected.Contains(trainDestination))
//                 trainDestinationsConnected.Add(trainDestination);
//
//             foreach (var trackRoute in checkingTrackRoute.Exit.ConnectedTrackRoutes)
//             {
//                 CheckNextTrackPiece(trackRoute, checkedTrackRoutes, trainDestinationsConnected);
//             }
//         }
//     }
// }