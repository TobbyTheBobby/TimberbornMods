namespace ChooChoo.NavigationSystem
{
    public class TrainPositionDestinationFactory
    {
        private readonly TrainNavigationService _trainNavigationService;

        public TrainPositionDestinationFactory(TrainNavigationService trainNavigationService)
        {
            _trainNavigationService = trainNavigationService;
        }

        public TrainPositionDestination Create(TrainDestination trainDestination)
        {
            return new TrainPositionDestination(_trainNavigationService, trainDestination);
        }
    }
}