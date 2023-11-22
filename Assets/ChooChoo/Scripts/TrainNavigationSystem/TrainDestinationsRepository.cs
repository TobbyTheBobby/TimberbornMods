using System.Collections.Generic;

namespace ChooChoo
{
    public class TrainDestinationsRepository
    {
        private readonly List<TrainDestination> _trainDestinations = new();

        public List<TrainDestination> TrainDestinations => _trainDestinations;

        public void Register(TrainDestination trainDestination)
        {
            _trainDestinations.Add(trainDestination);
        }

        public void UnRegister(TrainDestination trainDestination)
        {
            _trainDestinations.Remove(trainDestination);
        }
    }
}