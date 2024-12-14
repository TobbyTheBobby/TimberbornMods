using System.Collections.Generic;
using ChooChoo.BuildingRegistrySystem;
using ChooChoo.TrackSystem;
using Timberborn.SingletonSystem;

namespace ChooChoo.NavigationSystem
{
    public class TrackRouteWeightCache : IPostLoadableSingleton
    {
        private readonly BuildingRegistry<TrackPiece> _trackPieceRegistry;
        private readonly EventBus _eventBus;

        public TrackRouteWeightCache(BuildingRegistry<TrackPiece> trackPieceRegistry, EventBus eventBus)
        {
            _trackPieceRegistry = trackPieceRegistry;
            _eventBus = eventBus;
        }

        public Dictionary<TrackRoute, int?> TrackRouteWeights { get; } = new();

        public void PostLoad()
        {
            _eventBus.Register(this);
            RefreshCache();
        }

        [OnEvent]
        public void OnTracksRecalculated(TracksRecalculatedEvent tracksRecalculatedEvent)
        {
            RefreshCache();
        }
        
        private void RefreshCache()
        {
            TrackRouteWeights.Clear();
            foreach (var trackPiece in _trackPieceRegistry.Finished)
            {
                foreach (var trackRoute in trackPiece.TrackRoutes)
                {
                    TrackRouteWeights.Add(trackRoute, null);
                }
            }
        }
    }
}