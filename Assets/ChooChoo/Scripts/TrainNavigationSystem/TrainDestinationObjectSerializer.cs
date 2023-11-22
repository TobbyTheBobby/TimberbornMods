using Timberborn.Persistence;

namespace ChooChoo
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
      TrainPositionDestination positionDestination = value as TrainPositionDestination;
      ConvertPositionDestination(positionDestination, objectSaver);
    }

    public Obsoletable<ITrainDestination> Deserialize(IObjectLoader objectLoader)
    {
      return _trainPositionDestinationFactory.Create(objectLoader.Get(DestinationKey));
    }

    private static void ConvertPositionDestination(TrainPositionDestination positionDestination, IObjectSaver objectSaver)
    {
      objectSaver.Set(DestinationKey, positionDestination.Destination);
    }
  }
}
