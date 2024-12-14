using ChooChoo.NavigationSystem;

namespace ChooChoo.TrainYards
{
    public class TrainYardService
    {
        private readonly TrainDestinationService _trainDestinationService;

        public TrainDestination CurrentTrainYard;

        private TrainYardService(TrainDestinationService trainDestinationService)
        {
            _trainDestinationService = trainDestinationService;
        }

        // public bool ConnectedToTrainYard(TrainDestination trainDestination)
        // {
        //     return _trainDestinationService.TrainDestinationsConnectedBothWays(CurrentTrainYard, trainDestination);
        // }
    }
}