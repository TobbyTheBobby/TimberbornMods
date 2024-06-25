using System.Collections.Generic;
using ChooChoo.TrackSystem;
using Timberborn.SingletonSystem;
using TobbyTools.BuildingRegistrySystem;

namespace ChooChoo.NavigationSystem
{
    public class TrainDestinationConnectedRepository : IPostLoadableSingleton
    {
        private readonly BuildingRegistry<TrainDestination> _trainDestinationRegistry;
        private readonly EventBus _eventBus;

        private readonly Dictionary<TrainDestination, List<TrainDestination>> _trainDestinationConnections = new();

        private bool _tracksUpdated = true;

        public Dictionary<TrainDestination, List<TrainDestination>> TrainDestinations
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
            EventBus eventBus)
        {
            _trainDestinationRegistry = trainDestinationRegistry;
            _eventBus = eventBus;
        }

        public void PostLoad()
        {
            _eventBus.Register(this);
            Update();
        }

        [OnEvent]
        public void OnTrackUpdate(OnTracksUpdatedEvent onTracksUpdatedEvent)
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
                var checkedTrackPieces = new List<TrackPiece>();
                CheckNextTrackPiece(checkingDestination.GetComponentFast<TrackPiece>(), checkedTrackPieces, trainDestinationsConnected);
                _trainDestinationConnections.Add(checkingDestination, trainDestinationsConnected);
            }
            // Plugin.Log.LogWarning(list.Count + "");
            // foreach (var l in list)
            // {
            //     Plugin.Log.LogInfo(l.Count + "");
            // }
        }

        private void CheckNextTrackPiece(
            TrackPiece checkingTrackPiece,
            List<TrackPiece> checkedTrackPieces,
            List<TrainDestination> trainDestinationsConnected)
        {
            // Plugin.Log.LogError(checkingTrackPiece.CenterCoordinates + "");
            checkedTrackPieces.Add(checkingTrackPiece);

            if (checkingTrackPiece.TryGetComponentFast(out TrainDestination trainDestination))
                trainDestinationsConnected.Add(trainDestination);

            foreach (var trackRoute in checkingTrackPiece.TrackRoutes)
            {
                if (trackRoute.Exit.ConnectedTrackPiece == null)
                    continue;

                if (checkedTrackPieces.Contains(trackRoute.Exit.ConnectedTrackPiece))
                    continue;

                CheckNextTrackPiece(trackRoute.Exit.ConnectedTrackPiece, checkedTrackPieces, trainDestinationsConnected);
            }
        }
    }
}