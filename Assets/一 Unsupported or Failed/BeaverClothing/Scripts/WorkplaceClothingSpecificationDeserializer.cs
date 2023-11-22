using System;
using Timberborn.Persistence;

namespace BeaverHats
{
    public class WorkplaceClothingSpecificationDeserializer : IObjectSerializer<WorkplaceClothingSpecification>
    {
        public void Serialize(WorkplaceClothingSpecification value, IObjectSaver objectSaver) => throw new NotSupportedException();

        public Obsoletable<WorkplaceClothingSpecification> Deserialize(IObjectLoader objectLoader)
        {
            return (Obsoletable<WorkplaceClothingSpecification>) new WorkplaceClothingSpecification(
                objectLoader.Get(new PropertyKey<bool>("Enabled")),
                objectLoader.Get(new PropertyKey<string>("WorkPlace")),
                objectLoader.Get(new PropertyKey<int>("WearChance"))
            );
        }
    }
}
