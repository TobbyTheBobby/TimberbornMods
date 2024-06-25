using System.Reflection;
using HarmonyLib;
using Newtonsoft.Json.Linq;
using Timberborn.Persistence;
using Timberborn.SerializationSystem;

namespace DifficultySettingsChanger
{
    public class DynamicSpecificationDeserializer : IObjectSerializer<DynamicSpecification>
    {
        private static readonly PropertyKey<string> PrefabNameKey = new("PrefabName");

        private readonly ObjectSaveReaderWriter _objectSaveReaderWriter;

        private static readonly BindingFlags BindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
        private readonly FieldInfo _fieldInfo = AccessTools.TypeByName("ObjectLoader").GetField("_objectSave", BindingFlags);
        private readonly MethodInfo _methodInfo = typeof(ObjectSaveReaderWriter).GetMethod("SerializeObject", BindingFlags);

        DynamicSpecificationDeserializer(ObjectSaveReaderWriter objectSaveReaderWriter)
        {
            _objectSaveReaderWriter = objectSaveReaderWriter;
        }
        
        public void Serialize(DynamicSpecification value, IObjectSaver objectSaver)
        {
            // not used
        }

        public Obsoletable<DynamicSpecification> Deserialize(IObjectLoader objectLoader)
        {
            var prefabName = objectLoader.Get(PrefabNameKey);

            var objectSave = _fieldInfo.GetValue(objectLoader);

            var serializedObject = (JObject)_methodInfo.Invoke(_objectSaveReaderWriter, new[] { objectSave });
            
            return new DynamicSpecification(prefabName, serializedObject);
        }
    }
}
