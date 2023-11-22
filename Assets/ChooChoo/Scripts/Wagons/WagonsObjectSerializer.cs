using Timberborn.Persistence;

namespace ChooChoo
{
  public class WagonsObjectSerializer : IObjectSerializer<TrainWagon>
  {
    private static readonly PropertyKey<TrainWagon> WagonKey = new("Wagon");

    public void Serialize(TrainWagon value, IObjectSaver objectSaver)
    {
      objectSaver.Set(WagonKey, value);
    }

    public Obsoletable<TrainWagon> Deserialize(IObjectLoader objectLoader)
    {
      TrainWagon trainWagon = objectLoader.Get(WagonKey);
      return trainWagon;
    }
  }
}
