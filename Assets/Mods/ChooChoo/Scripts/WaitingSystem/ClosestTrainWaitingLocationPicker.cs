using System.Collections.Generic;
using System.Linq;
using ChooChoo.NavigationSystem;
using ChooChoo.TrackSystem;
using TobbyTools.BuildingRegistrySystem;
using UnityEngine;

namespace ChooChoo.WaitingSystem
{
    public class ClosestTrainWaitingLocationPicker
    {
        private readonly BuildingRegistry<TrainWaitingLocation> _trainWaitingLocationRegistry;
        private readonly TrainDestinationService _trainDestinationService;
        private readonly TrainNavigationService _trainNavigationService;

        private ClosestTrainWaitingLocationPicker(
            BuildingRegistry<TrainWaitingLocation> trainWaitingLocationRegistry,
            TrainDestinationService trainDestinationService,
            TrainNavigationService trainNavigationService)
        {
            _trainWaitingLocationRegistry = trainWaitingLocationRegistry;
            _trainDestinationService = trainDestinationService;
            _trainNavigationService = trainNavigationService;
        }

        public TrainWaitingLocation ClosestWaitingLocation(Transform transform)
        {
            // return _trainWaitingLocationRegistry.Finished
            //     .Where(location =>
            //         !location.Occupied && _trainDestinationService.DestinationReachableOneWay(position, location.GetComponentFast<TrainDestination>()))
            //     .OrderBy(location => Vector3.Distance(position, location.TransformFast.position)).FirstOrDefault();
            
            return _trainWaitingLocationRegistry.Finished
                .Where(location =>
                    !location.Occupied && _trainNavigationService.FindTrackPath(transform, location.GetComponentFast<TrainDestination>(), new List<TrackRoute>()))
                .OrderBy(location => Vector3.Distance(transform.position, location.TransformFast.position)).FirstOrDefault();
        }
    }
}