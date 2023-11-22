using System.Collections.Generic;
using Timberborn.BlockSystem;
using UnityEngine;

namespace ChooChoo
{
    public class TrainDestinationService
    {
        private readonly TrainDestinationConnectedRepository _trainDestinationConnectedRepository;
        private readonly BlockService _blockService;

        public TrainDestinationService(TrainDestinationConnectedRepository trainDestinationConnectedRepository, BlockService blockService)
        {
            _trainDestinationConnectedRepository = trainDestinationConnectedRepository;
            _blockService = blockService;
        }

        public List<TrainDestination> GetConnectedTrainDestinations(TrainDestination trainDestination)
        {
            _trainDestinationConnectedRepository.TrainDestinations.TryGetValue(trainDestination, out var connectedTrainDestinations);
            return connectedTrainDestinations ?? new List<TrainDestination>();
        }

        public bool TrainDestinationsConnectedBothWays(TrainDestination a, TrainDestination b)
        {
            return TrainDestinationsConnectedOneWay(a, b) && TrainDestinationsConnectedOneWay(b, a);
        }
        
        public bool TrainDestinationsConnectedOneWay(TrainDestination origin, TrainDestination end)
        {
            if (origin == null)
                return false;

            var trainDestinations = _trainDestinationConnectedRepository.TrainDestinations;
            
            return trainDestinations.ContainsKey(origin) && trainDestinations[origin].Contains(end);
        }

        public bool DestinationReachableOneWay(TrackPiece start, TrainDestination end)
        {
            if (start == null || end == null)
                return false;
            var checkedTrackPieces = new List<TrackPiece>();
            var connectedDestination = FindTrainDestination(start, checkedTrackPieces);
            return TrainDestinationsConnectedOneWay(connectedDestination, end);
        }
        
        public bool DestinationReachable(Vector3 startPosition, TrainDestination end)
        {
            var startTrackPiece = _blockService.GetFloorObjectComponentAt<TrackPiece>(startPosition.ToBlockServicePosition());
            if (startTrackPiece == null || end == null)
                return false;
            var checkedTrackPieces = new List<TrackPiece>();
            var connectedDestination = FindTrainDestination(startTrackPiece, checkedTrackPieces);
            return TrainDestinationsConnectedBothWays(connectedDestination, end);
        }
        
        public bool DestinationReachableOneWay(Vector3 startPosition, TrainDestination end)
        {
            var startTrackPiece = _blockService.GetFloorObjectComponentAt<TrackPiece>(startPosition.ToBlockServicePosition());
            if (startTrackPiece == null || end == null)
                return false;
            var checkedTrackPieces = new List<TrackPiece>();
            var connectedDestination = FindTrainDestination(startTrackPiece, checkedTrackPieces);
            return TrainDestinationsConnectedOneWay(connectedDestination, end);
        }

        private TrainDestination FindTrainDestination(TrackPiece checkingTrackPiece, List<TrackPiece> checkedTrackPieces)
        {
            checkedTrackPieces.Add(checkingTrackPiece);

            if (checkingTrackPiece.TryGetComponentFast(out TrainDestination trainDestination))
                return trainDestination;
            
            foreach (var trackConnection in checkingTrackPiece.TrackRoutes)
            {
                if (trackConnection.Exit.ConnectedTrackPiece == null)
                    continue;

                if (checkedTrackPieces.Contains(trackConnection.Exit.ConnectedTrackPiece))
                    continue;

                var destination = FindTrainDestination(trackConnection.Exit.ConnectedTrackPiece, checkedTrackPieces);

                if (destination != null)
                    return destination;
            }

            checkedTrackPieces.Remove(checkingTrackPiece);
            return null;
        }
    }
}