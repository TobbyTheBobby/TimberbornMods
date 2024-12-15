using System.Collections.Generic;
using System.Linq;
using ChooChoo.TrackSystem;
using Timberborn.SingletonSystem;
using TobbyTools.BuildingRegistrySystem;

namespace ChooChoo.NavigationSystem
{
    public class TrainDestinationConnectedRepository : IPostLoadableSingleton
    {
        private readonly BuildingRegistry<TrainDestination> _trainDestinationRegistry;
        private readonly TrainNavigationService _trainNavigationService;
        private readonly EventBus _eventBus;

        private readonly Dictionary<TrainDestination, IEnumerable<TrainDestination>> _trainDestinationConnections = new();

        private bool _tracksUpdated = true;

        public Dictionary<TrainDestination, IEnumerable<TrainDestination>> TrainDestinations
        {
            get
            {
                if (_tracksUpdated)
                    Update();
                return _trainDestinationConnections;
            }
        }

        public TrainDestinationConnectedRepository(
            BuildingRegistry<TrainDestination> trainDestinationRegistry,
            TrainNavigationService trainNavigationService,
            EventBus eventBus)
        {
            _trainDestinationRegistry = trainDestinationRegistry;
            _trainNavigationService = trainNavigationService;
            _eventBus = eventBus;
        }

        public void PostLoad()
        {
            _eventBus.Register(this);
            _tracksUpdated = true;
        }

        [OnEvent]
        public void OnTracksRecalculated(TracksRecalculatedEvent tracksRecalculatedEvent)
        {
            _tracksUpdated = true;
        }

        private void Update()
        {
            FindDestinationConnections();
            _tracksUpdated = false;
        }

        private void FindDestinationConnections()
        {
            _trainDestinationConnections.Clear();
            foreach (var checkingDestination in _trainDestinationRegistry.Finished)
            {
                var trainDestinationsConnected = new List<TrainDestination>();
                // var checkedTrackPieces = new List<TrackPiece>();   
                foreach (var otherDestination in _trainDestinationRegistry.Finished)
                {
                    if (checkingDestination == otherDestination)
                        continue;
                    
                    foreach (var trackConnection in checkingDestination.TrackPiece.TrackRoutes.Select(route => route.Exit))
                    {
                        if (_trainNavigationService.FindPath(trackConnection.Direction, checkingDestination.TrackPiece, otherDestination.TrackPiece, new List<TrackRoute>()))
                        {
                            trainDestinationsConnected.Add(otherDestination);
                        }
                    }
                }
                
                // CheckNextTrackPiece(checkingDestination.GetComponentFast<TrackPiece>(), checkedTrackPieces, trainDestinationsConnected);
                _trainDestinationConnections.Add(checkingDestination, trainDestinationsConnected.Distinct());
            }
            
            // Debug.LogWarning(_trainDestinationConnections.Count + "");
            // foreach (var l in _trainDestinationConnections)
            // {
            //     Debug.Log(l.Value.Count() + "");
            // }
        }
    }
}