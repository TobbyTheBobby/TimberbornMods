using System.Collections.Generic;
using ChooChoo;

public class TrackRouteWeightCache
{
    private readonly Dictionary<TrackRoute, int?> _trackRouteWeights = new();

    public Dictionary<TrackRoute, int?> TrackRouteWeights => _trackRouteWeights;

    public void Add(TrackRoute trackRoute)
    {
        _trackRouteWeights.Add(trackRoute, null);
    }
    
    public void Remove(TrackRoute[] trackRoutes)
    {
        foreach (var trackRoute in trackRoutes)
            _trackRouteWeights.Remove(trackRoute);
    }
}
