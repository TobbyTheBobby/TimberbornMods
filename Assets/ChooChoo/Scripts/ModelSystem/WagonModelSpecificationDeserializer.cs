using System;
using Timberborn.Persistence;

namespace ChooChoo
{
    public class WagonModelSpecificationDeserializer : IObjectSerializer<WagonModelSpecification>
    {
        public void Serialize(WagonModelSpecification value, IObjectSaver objectSaver) => throw new NotSupportedException();

        public Obsoletable<WagonModelSpecification> Deserialize(IObjectLoader objectLoader)
        {
            return (Obsoletable<WagonModelSpecification>) new WagonModelSpecification(
                objectLoader.Get(new PropertyKey<string>("Faction")),
                objectLoader.Get(new PropertyKey<string>("NameLocKey")),
                objectLoader.Get(new PropertyKey<string>("ModelLocation")),
                objectLoader.GetValueOrNull(new PropertyKey<string>("DependentModel")),
                objectLoader.Get(new PropertyKey<float>("Length")));
        }
    }
}
