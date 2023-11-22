using System.Collections.Generic;

namespace ChooChoo
{
    public class TrainWaitingLocationsRepository
    {
        private readonly List<TrainWaitingLocation> _waitingLocations = new();

        public List<TrainWaitingLocation> WaitingLocations => _waitingLocations;

        public void Register(TrainWaitingLocation trainDestination)
        {
            _waitingLocations.Add(trainDestination);
        }

        public void UnRegister(TrainWaitingLocation trainDestination)
        {
            _waitingLocations.Remove(trainDestination);
        }
    }
}