using System;
using Timberborn.Persistence;

namespace TobbyTools.TextureReplacementTool
{
    public class ReplaceTextureSpecificationDeserializer : IObjectSerializer<ReplaceTextureSpecification>
    {
        public void Serialize(ReplaceTextureSpecification value, IObjectSaver objectSaver) => throw new NotSupportedException();

        public Obsoletable<ReplaceTextureSpecification> Deserialize(IObjectLoader objectLoader)
        {
            return (Obsoletable<ReplaceTextureSpecification>) new ReplaceTextureSpecification(
                objectLoader.GetValueOrNull(new PropertyKey<string>("BuildingName")),
                objectLoader.Get(new PropertyKey<string>("MaterialName")), 
                objectLoader.Get(new PropertyKey<string>("ReplacementTextureName")));
        }
    }
}
