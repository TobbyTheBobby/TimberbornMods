using Timberborn.Persistence;

namespace ChooChoo.Wagons
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
            var trainWagon = objectLoader.Get(WagonKey);
            return trainWagon;
        }
    }
}