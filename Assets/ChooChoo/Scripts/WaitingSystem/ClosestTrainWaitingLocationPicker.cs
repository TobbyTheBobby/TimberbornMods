using System.Linq;
using ChooChoo.NavigationSystem;
using TobbyTools.BuildingRegistrySystem;
using UnityEngine;

namespace ChooChoo.WaitingSystem
{
    public class ClosestTrainWaitingLocationPicker
    {
        private readonly BuildingRegistry<TrainWaitingLocation> _trainWaitingLocationRegistry;
        private readonly TrainDestinationService _trainDestinationService;

        private ClosestTrainWaitingLocationPicker(
            BuildingRegistry<TrainWaitingLocation> trainWaitingLocationRegistry,
            TrainDestinationService trainDestinationService)
        {
            _trainWaitingLocationRegistry = trainWaitingLocationRegistry;
            _trainDestinationService = trainDestinationService;
        }

        public TrainWaitingLocation ClosestWaitingLocation(Vector3 position)
        {
            var list = _trainWaitingLocationRegistry.Finished
                .Where(location =>
                    !location.Occupied && _trainDestinationService.DestinationReachableOneWay(position, location.GetComponentFast<TrainDestination>()))
                .OrderBy(location => Vector3.Distance(position, location.TransformFast.position));
            if (!list.Any())
                return null;
            var closestReachableTrainWaitingLocation = list.First();
            return closestReachableTrainWaitingLocation;
        }
    }
}