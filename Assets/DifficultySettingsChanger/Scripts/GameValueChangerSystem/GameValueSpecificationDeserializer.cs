using System;
using Timberborn.Persistence;

namespace DifficultySettingsChanger
{
    public class GameValueSpecificationDeserializer : IObjectSerializer<GameValueSpecification>
    {
        public void Serialize(GameValueSpecification value, IObjectSaver objectSaver)
        {
            // // Plugin.Log.LogInfo("Serializing");
            //
            // var fileName = "GameValueSpecification." + value.ClassName + "." + value.FieldName + ".original.json";
            //
            // var path = Path.Combine(Plugin.Mod.DirectoryPath, "Specifications", fileName);
            //
            // File.WriteAllText(path, JsonConvert.SerializeObject(value, Formatting.Indented));
            //
            // // Plugin.Log.LogInfo("Written to: " + path );

            throw new NotImplementedException();
        }

        public Obsoletable<GameValueSpecification> Deserialize(IObjectLoader objectLoader)
        {
            var objectLoaderInstance = (ObjectLoader)objectLoader;

            // return (Obsoletable<GameValueSpecification>)new GameValueSpecification(
            //     objectLoader.Get(new PropertyKey<string>("ClassName")),
            //     objectLoader.Get(new PropertyKey<string>("FieldName")),
            //     objectLoaderInstance._objectSave.GetSerialized("Value"));

            try
            {
                return (Obsoletable<GameValueSpecification>)new GameValueSpecification(
                    objectLoader.Get(new PropertyKey<string>("ClassName")),
                    objectLoader.Get(new PropertyKey<string>("FieldName")),
                    objectLoader.Get(new ListKey<GameValueSpecification>("Value"), this));
            }
            catch (InvalidCastException)
            {
                return (Obsoletable<GameValueSpecification>)new GameValueSpecification(
                    objectLoader.Get(new PropertyKey<string>("ClassName")),
                    objectLoader.Get(new PropertyKey<string>("FieldName")),
                    objectLoaderInstance._objectSave.GetSerialized("Value"));
            }
            
        }
    }
}
