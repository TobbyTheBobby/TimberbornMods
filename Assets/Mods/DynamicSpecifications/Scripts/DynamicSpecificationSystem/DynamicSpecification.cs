using Newtonsoft.Json.Linq;

namespace DynamicSpecifications
{
    public class DynamicSpecification
    {
        public string PrefabName { get; }
        public JObject SerializedObject { get; }

        public DynamicSpecification(string prefabName, JObject serializedObject)
        {
            PrefabName = prefabName;
            SerializedObject = serializedObject;
        }
    }
}