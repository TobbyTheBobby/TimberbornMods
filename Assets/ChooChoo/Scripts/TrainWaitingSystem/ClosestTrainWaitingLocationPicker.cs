using System.Linq;
using UnityEngine;

namespace ChooChoo
{
    public class ClosestTrainWaitingLocationPicker
    {
        private readonly TrainWaitingLocationsRepository _trainWaitingLocationsRepository;

        private readonly TrainDestinationService _trainDestinationService;

        ClosestTrainWaitingLocationPicker(TrainWaitingLocationsRepository trainWaitingLocationsRepository, TrainDestinationService trainDestinationService)
        {
            _trainWaitingLocationsRepository = trainWaitingLocationsRepository;
            _trainDestinationService = trainDestinationService;
        }

        public TrainWaitingLocation ClosestWaitingLocation(Vector3 position)
        {
            var list = _trainWaitingLocationsRepository.WaitingLocations.Where(location => !location.Occupied && _trainDestinationService.DestinationReachableOneWay(position, location.TrainDestinationComponent)).OrderBy(location => Vector3.Distance(position, location.TransformFast.position));
            if (!list.Any())
                return null;
            var closestReachableTrainWaitingLocation = list.First();
            return closestReachableTrainWaitingLocation;
        }
    }
}
