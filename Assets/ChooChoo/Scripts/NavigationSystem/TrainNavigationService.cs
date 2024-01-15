using System.Collections.Generic;
using System.Linq;
using ChooChoo.Extensions;
using ChooChoo.TrackSystem;
using Timberborn.BlockSystem;
using Timberborn.Coordinates;
using UnityEngine;

namespace ChooChoo.NavigationSystem
{
    public class TrainNavigationService
    {
        private readonly bool _shouldLog = false;

        private readonly TrackRouteWeightsCalculator _trackRouteWeightsCalculator;
        private readonly TrainDestinationService _trainDestinationService;
        private readonly TrackRouteWeightCache _trackRouteWeightCache;
        private readonly BlockService _blockService;

        // private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        private TrainNavigationService(TrackRouteWeightsCalculator trackRouteWeightsCalculator, TrainDestinationService trainDestinationService,
            TrackRouteWeightCache trackRouteWeightCache, BlockService blockService)
        {
            _trackRouteWeightsCalculator = trackRouteWeightsCalculator;
            _trainDestinationService = trainDestinationService;
            _trackRouteWeightCache = trackRouteWeightCache;
            _blockService = blockService;
        }

        public bool FindRailTrackPath(Transform transform, TrainDestination destination, List<TrackRoute> tempPathTrackRoutes)
        {
            // _stopwatch.Restart();
            if (destination == null)
                return false;
            var startTrackPiece = _blockService.GetFloorObjectComponentAt<TrackPiece>(transform.position.ToBlockServicePosition());
            var endTrackPiece = destination.GetComponentFast<TrackPiece>();
            if (startTrackPiece == null || endTrackPiece == null)
                return false;

            if (_shouldLog) Plugin.Log.LogInfo("TrackPieces valid");
            var startTrainDestination = startTrackPiece.GetComponentFast<TrainDestination>();
            if (!_trainDestinationService.TrainDestinationsConnectedOneWay(startTrainDestination, destination))
            {
                if (_shouldLog) Plugin.Log.LogError("Destinations Not Connected");
                if (!_trainDestinationService.DestinationReachableOneWay(startTrackPiece, destination))
                {
                    if (_shouldLog) Plugin.Log.LogError("Destinations Unreachable");
                    return false;
                }
            }

            var facingDirectionRoutes = GetFacingDirections(transform, startTrackPiece);

            foreach (var trackRoute in facingDirectionRoutes)
            {
                if (_shouldLog) Plugin.Log.LogWarning("Looking in direction: " + trackRoute.Exit.Direction);
                if (_shouldLog) Plugin.Log.LogWarning("startTrackPiece == endTrackPiece: " + (startTrackPiece == endTrackPiece));
                if (startTrackPiece == endTrackPiece)
                {
                    tempPathTrackRoutes.Add(trackRoute);
                    tempPathTrackRoutes.Add(trackRoute);
                    return true;
                }

                int? maxDistance = null;
                var distance = 0;
                var trackRouteWeights = new Dictionary<TrackRoute, int?>(_trackRouteWeightCache.TrackRouteWeights);
                _trackRouteWeightsCalculator.CalculateTrackRouteWeight(trackRoute, endTrackPiece, trackRouteWeights, distance, ref maxDistance);
                if (_shouldLog) Plugin.Log.LogError("Weights calculated");
                if (maxDistance == null)
                    continue;
                var trackRoutes = new List<TrackRoute>();
                if (!FindPath(trackRoute, trackRouteWeights, endTrackPiece, trackRoutes, (int)maxDistance))
                    continue;

                tempPathTrackRoutes.AddRange(trackRoutes);
                return true;
            }

            if (_shouldLog) Plugin.Log.LogInfo("Couldnt find path");
            // _stopwatch.Stop();
            // var secondPart = _stopwatch.ElapsedTicks;
            // Plugin.Log.LogWarning("First: " + firstPart + " Second: " + secondPart + " Total: " + (firstPart + secondPart) + " (10.000 Ticks = 1ms)");
            return false;
        }

        private IEnumerable<TrackRoute> GetFacingDirections(Transform transform, TrackPiece startTrackPiece)
        {
            var directionalTrackRoutes = new List<TrackRoute>();
            var facingDirection = transform.eulerAngles.y.ToDirection2D();
            var correctedFacingDirection = startTrackPiece.GetComponentFast<BlockObject>().Orientation.CorrectedTransform(facingDirection);
            AddFacingTrackRoute(correctedFacingDirection, directionalTrackRoutes, startTrackPiece);
            var rightOfCorrectlyFacingDirection = correctedFacingDirection.Next();
            AddFacingTrackRoute(rightOfCorrectlyFacingDirection, directionalTrackRoutes, startTrackPiece);
            var leftOfCorrectlyFacingDirection = correctedFacingDirection.Next().Next().Next();
            AddFacingTrackRoute(leftOfCorrectlyFacingDirection, directionalTrackRoutes, startTrackPiece);
            var oppositeOfCorrectlyFacingDirection = correctedFacingDirection.Next().Next();
            AddFacingTrackRoute(oppositeOfCorrectlyFacingDirection, directionalTrackRoutes, startTrackPiece);
            // Plugin.Log.LogInfo(transform.eulerAngles + "   " + facingDirection + "      " + correctedFacingDirection + "  " + rightOfCorrectlyFacingDirection + "   " + leftOfCorrectlyFacingDirection + "    " + oppositeOfCorrectlyFacingDirection);
            return directionalTrackRoutes;
        }

        private void AddFacingTrackRoute(Direction2D direction, List<TrackRoute> directionalTrackRoutes, TrackPiece startTrackPiece)
        {
            var trackRoute = startTrackPiece.TrackRoutes.FirstOrDefault(route => route.Exit.Direction == direction);
            if (trackRoute != null)
                directionalTrackRoutes.Add(trackRoute);
        }

        private bool FindPath(TrackRoute previousRoute, Dictionary<TrackRoute, int?> nodes, TrackPiece destinationTrackPiece,
            List<TrackRoute> trackConnections, int maxLength)
        {
            // Plugin.Log.LogInfo(nodes[previousRoute] + "      " + maxLength);
            if (!nodes.ContainsKey(previousRoute) || previousRoute.Exit.ConnectedTrackRoutes == null || nodes[previousRoute] > maxLength)
            {
                // Plugin.Log.LogWarning("Is null   ");
                return false;
            }

            trackConnections.Add(previousRoute);

            foreach (var trackRoute in previousRoute.Exit.ConnectedTrackRoutes
                         .Where(connection => connection.Exit.ConnectedTrackRoutes != null)
                         .OrderBy(connection =>
                             Vector3.Distance(connection.Exit.ConnectedTrackPiece.CenterCoordinates, destinationTrackPiece.CenterCoordinates))
                    )
            {
                // Plugin.Log.LogError("Checking route: " + nodes[trackRoute] + "  " + nodes[previousRoute] + "  " + trackRoute.Exit.ConnectedTrackPiece.TrackDistance);
                if (!nodes.ContainsKey(trackRoute))
                    continue;
                if (nodes[previousRoute] + trackRoute.Exit.ConnectedTrackPiece.TrackDistance == nodes[trackRoute])
                {
                    // var test = "Route found: " + nodes[trackRoute] + " ";
                    // if (trackRoute.Exit.ConnectedTrackPiece != null)
                    //     test += trackRoute.Exit.ConnectedTrackPiece.CenterCoordinates;
                    // Plugin.Log.LogWarning(test);
                    if (trackRoute.Exit.ConnectedTrackPiece == destinationTrackPiece)
                    {
                        foreach (var destinationTrackRoute in destinationTrackPiece.TrackRoutes)
                        {
                            if (destinationTrackRoute.Entrance.ConnectedTrackPiece == previousRoute.Exit.ConnectedTrackPiece)
                            {
                                // Plugin.Log.LogError("Found Destination");
                                trackConnections.Add(trackRoute);
                                if (!trackRoute.Exit.ConnectedTrackRoutes.Any())
                                    return true;

                                trackConnections.Add(trackRoute.Exit.ConnectedTrackRoutes[0]);
                                return true;
                            }
                        }

                        trackConnections.Remove(previousRoute);
                        return false;
                    }

                    if (FindPath(trackRoute, nodes, destinationTrackPiece, trackConnections, maxLength))
                    {
                        return true;
                    }
                }
            }

            trackConnections.Remove(previousRoute);
            return false;
        }
    }
}