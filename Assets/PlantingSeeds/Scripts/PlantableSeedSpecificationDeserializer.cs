using System;
using Timberborn.Persistence;

namespace PlantingSeeds
{
    public class PlantableSeedSpecificationDeserializer : IObjectSerializer<PlantableSeedSpecification>
    {
        public void Serialize(PlantableSeedSpecification value, IObjectSaver objectSaver) => throw new NotSupportedException();

        public Obsoletable<PlantableSeedSpecification> Deserialize(IObjectLoader objectLoader)
        {
            return new PlantableSeedSpecification(
                objectLoader.Get(new PropertyKey<string>("PlantablePrefabName")),
                objectLoader.Get(new PropertyKey<string>("GoodId")),
                objectLoader.Get(new PropertyKey<int>("GoodAmount")));
        }
    }
}
