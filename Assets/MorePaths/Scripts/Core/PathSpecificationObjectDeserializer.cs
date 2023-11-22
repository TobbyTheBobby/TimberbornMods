using TimberApi.Common.Extensions;
using Timberborn.Persistence;

namespace MorePaths
{
    public class PathSpecificationObjectDeserializer : IObjectSerializer<PathSpecification>
    {
        private static readonly PropertyKey<bool> EnabledKey = new("Enabled");
        
        public void Serialize(PathSpecification value, IObjectSaver objectSaver) { }

        public Obsoletable<PathSpecification> Deserialize(IObjectLoader objectLoader)
        {
            return (Obsoletable<PathSpecification>)new PathSpecification(
                objectLoader.GetValueOrDefault(EnabledKey, true),
                objectLoader.Get(new PropertyKey<string>("Name")),
                objectLoader.GetValueOrNull(new PropertyKey<string>("PathTexture")),
                objectLoader.GetValueOrNull(new PropertyKey<string>("RailingTexture")),
                objectLoader.GetValueOrNull(new PropertyKey<string>("ToolGroup")),
                objectLoader.GetValueOrDefault(new PropertyKey<int>("ToolOrder"), 0),
                objectLoader.GetValueOrNull(new PropertyKey<string>("PathIcon")),
                objectLoader.GetValueOrNull(new PropertyKey<string>("DisplayNameLocKey")),
                objectLoader.GetValueOrNull(new PropertyKey<string>("DescriptionLocKey")),
                objectLoader.GetValueOrNull(new PropertyKey<string>("FlavorDescriptionLocKey")),

                objectLoader.GetValueOrDefault(new PropertyKey<float>("MainTextureScale"), 1f),
                objectLoader.GetValueOrDefault(new PropertyKey<float>("NoiseTexScale"), 1f),
                objectLoader.GetValueOrDefault(new PropertyKey<float>("MainColorRed"), 1f),
                objectLoader.GetValueOrDefault(new PropertyKey<float>("MainColorGreen"), 1f),
                objectLoader.GetValueOrDefault(new PropertyKey<float>("MainColorBlue"), 1f),

                objectLoader.GetValueOrDefault(new PropertyKey<bool>("RailingEnabled"), true));
        }
    }
}
