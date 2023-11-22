using Timberborn.Meshy;

namespace BeaverHats
{
    public class TestingModelPostProcessor : IModelPostprocessor
    {
        public void Postprocess(ImportDetails details)
        {
            Plugin.Log.LogWarning(details.Root.name);
            foreach (var pair in details.CreatedObjectsMap)
            {
                Plugin.Log.LogInfo(pair.Key.Name);
                Plugin.Log.LogInfo(pair.Value.name);
            }
        }
    }
}