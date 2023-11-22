using System.Linq;
using ThunderKit.Core.Pipelines;
using UnityEngine.UIElements;

namespace ThunderkitModioUpload
{
    public static class Extensions
    {
        public static T GetSingle<T>(this Pipeline pipeline)
        {
            var manifestIdentities = pipeline.Manifest.Data.OfType<T>().ToArray();
            if (!manifestIdentities.Any())
            {
                pipeline.Log(LogLevel.Warning, "No " + nameof(T) + " found on manifest, skipping.");
                return default;
            }
            
            return manifestIdentities.First();
        }
        
        public static void ToggleDisplayStyle(this VisualElement visualElement, bool visible) => visualElement.style.display = (StyleEnum<DisplayStyle>) (visible ? DisplayStyle.Flex : DisplayStyle.None);
    }
}