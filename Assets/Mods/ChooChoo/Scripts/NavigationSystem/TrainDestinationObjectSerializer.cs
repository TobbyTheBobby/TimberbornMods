using Timberborn.Persistence;

namespace ChooChoo.NavigationSystem
{
    public class TrainDestinationObjectSerializer : IObjectSerializer<ITrainDestination>
    {
        private static readonly PropertyKey<TrainDestination> DestinationKey = new("Station");

        private readonly TrainPositionDestinationFactory _trainPositionDestinationFactory;

        public TrainDestinationObjectSerializer(TrainPositionDestinationFactory trainTrainPositionDestinationFactory)
        {
            _trainPositionDestinationFactory = trainTrainPositionDestinationFactory;
        }

        public void Serialize(ITrainDestination value, IObjectSaver objectSaver)
        {
            var positionDestination = (TrainPositionDestination)value;
            objectSaver.Set(DestinationKey, positionDestination.Destination);
        }

        public Obsoletable<ITrainDestination> Deserialize(IObjectLoader objectLoader)
        {
            return _trainPositionDestinationFactory.Create(objectLoader.Get(DestinationKey));
        }
    }
}