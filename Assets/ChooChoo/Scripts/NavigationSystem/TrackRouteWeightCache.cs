using System.Collections.Generic;
using ChooChoo.TrackSystem;

namespace ChooChoo.NavigationSystem
{
    public class TrackRouteWeightCache
    {
        public Dictionary<TrackRoute, int?> TrackRouteWeights { get; } = new();

        public void Add(TrackRoute trackRoute)
        {
            TrackRouteWeights.Add(trackRoute, null);
        }

        public void Remove(TrackRoute[] trackRoutes)
        {
            foreach (var trackRoute in trackRoutes)
                TrackRouteWeights.Remove(trackRoute);
        }
    }
}