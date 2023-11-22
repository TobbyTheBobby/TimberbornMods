namespace ChooChoo
{
    public class TrainYardService
    {
        private readonly TrainDestinationService _trainDestinationService;
        
        public TrainDestination CurrentTrainYard;

        TrainYardService(TrainDestinationService trainDestinationService)
        {
            _trainDestinationService = trainDestinationService; 
        }
        
        public bool ConnectedToTrainYard(TrainDestination trainDestination)
        {
            return _trainDestinationService.TrainDestinationsConnectedBothWays(CurrentTrainYard, trainDestination);
        }
    }
}