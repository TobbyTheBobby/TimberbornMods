using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ChooChoo
{
    public class TrackRouteWeightsCalculator
    {
        public void CalculateTrackRouteWeight(TrackRoute previousTrackRoute, TrackPiece destinationTrackPiece, Dictionary<TrackRoute, int?> trackRouteWeights, int previousDistance, ref int? maxDistance)
        {
            if (!trackRouteWeights.ContainsKey(previousTrackRoute) || previousTrackRoute.Exit.ConnectedTrackRoutes == null)
                return;

            var currentDistance = previousDistance + previousTrackRoute.Exit.ConnectedTrackPiece.TrackDistance;

            if (currentDistance > maxDistance)
                return;

            if (!UpdateWeight(previousTrackRoute, currentDistance, trackRouteWeights))
                return;

            // Plugin.Log.LogError("Checking Route");
            foreach (var trackRoute in previousTrackRoute.Exit.ConnectedTrackRoutes
                         .Where(trackRoute => trackRoute.Exit.ConnectedTrackRoutes != null)
                         .OrderBy(trackRoute => Vector3.Distance(trackRoute.Exit.ConnectedTrackPiece.CenterCoordinates, destinationTrackPiece.CenterCoordinates))
                     )
            {
                // Plugin.Log.LogWarning("Checking: " + trackRoute.Exit.ConnectedTrackPiece.CenterCoordinates + " Current weight: " + trackRouteWeights[trackRoute] + " New currentDistance: " + currentDistance + "Max distance: " + maxDistance);

                if (!trackRoute.Exit.ConnectedTrackPiece.CanPathFindOverIt && !(trackRoute.Exit.ConnectedTrackPiece == destinationTrackPiece))
                {
                    // Plugin.Log.LogError("Cannot pathfind over it");
                    continue;
                }

                if (trackRoute.Exit.ConnectedTrackPiece == destinationTrackPiece)
                {
                    // Plugin.Log.LogError("Reached end");
                    foreach (var destinationTrackRoute in destinationTrackPiece.TrackRoutes)
                    {
                        if (destinationTrackRoute.Entrance.ConnectedTrackPiece != previousTrackRoute.Exit.ConnectedTrackPiece) 
                            continue;
                        // Plugin.Log.LogError("Found Destination");

                        if (!trackRouteWeights.ContainsKey(trackRoute))
                            continue;
                        
                        var newNewDistance = currentDistance + trackRoute.Exit.ConnectedTrackPiece.TrackDistance;

                        if (trackRouteWeights[trackRoute] == null)
                            trackRouteWeights[trackRoute] = newNewDistance;
                        else
                            trackRouteWeights[trackRoute] =
                                Math.Min((int)trackRouteWeights[trackRoute], newNewDistance);

                        maxDistance = maxDistance == null ? newNewDistance : Math.Min((int)maxDistance, newNewDistance);
                        break;
                    }
                }

                CalculateTrackRouteWeight(trackRoute, destinationTrackPiece, trackRouteWeights, currentDistance, ref maxDistance);
            }
            // Plugin.Log.LogError("Dead end");
        }

        private bool UpdateWeight(TrackRoute previousTrackRoute, int currentDistance, Dictionary<TrackRoute, int?> trackRouteWeights)
        {
            // Plugin.Log.LogInfo("Updating weight");
            if (trackRouteWeights[previousTrackRoute] == null)
            {
                trackRouteWeights[previousTrackRoute] = currentDistance;
            }
            else
            {
                var previousWeight = (int)trackRouteWeights[previousTrackRoute];
                if (previousWeight <= currentDistance)
                    return false;
                trackRouteWeights[previousTrackRoute] = Math.Min(previousWeight, currentDistance);
            }
            // Plugin.Log.LogWarning("Updated weights: Current weight: " + trackRouteWeights[previousTrackRoute] + " New currentDistance: " + currentDistance);
            return true;
        }
    }
}