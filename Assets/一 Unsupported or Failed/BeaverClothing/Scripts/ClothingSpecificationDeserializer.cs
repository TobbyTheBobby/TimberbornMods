using System;
using Timberborn.Persistence;

namespace BeaverClothing
{
    public class ClothingSpecificationDeserializer : IObjectSerializer<ClothingSpecification>
    {
        public void Serialize(ClothingSpecification value, IObjectSaver objectSaver) => throw new NotSupportedException();

        public Obsoletable<ClothingSpecification> Deserialize(IObjectLoader objectLoader)
        {
            return (Obsoletable<ClothingSpecification>) new ClothingSpecification(
                objectLoader.Get(new PropertyKey<bool>("Enabled")),
                objectLoader.Get(new PropertyKey<string>("PrefabPath")),
                objectLoader.Get(new PropertyKey<string>("CharacterType")), 
                objectLoader.Get(new PropertyKey<string>("BodyPartName")), 
                objectLoader.Get(new ListKey<string>("WorkPlaces")),
                objectLoader.Get(new PropertyKey<int>("WearChance")),
                objectLoader.Get(new PropertyKey<float>("PositionX")),
                objectLoader.Get(new PropertyKey<float>("PositionY")),
                objectLoader.Get(new PropertyKey<float>("PositionZ")),
                objectLoader.Get(new PropertyKey<float>("RotationX")),
                objectLoader.Get(new PropertyKey<float>("RotationY")),
                objectLoader.Get(new PropertyKey<float>("RotationZ")),
                objectLoader.Get(new PropertyKey<float>("ScaleX")),
                objectLoader.Get(new PropertyKey<float>("ScaleY")),
                objectLoader.Get(new PropertyKey<float>("ScaleZ"))
            );
        }
    }
}
