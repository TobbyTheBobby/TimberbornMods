using UnityEngine;
using UnityEngine.UIElements;

namespace TobbyTools.Extensions
{
    public static class VisualElementExtensions
    {
        public static void LogChildren(this VisualElement visualElement, int layer = 0)
        {
            foreach (var child in visualElement.Children())
            {
                Debug.Log($"Layer: {layer}, name: {child.name}, type: {child.GetType()}");
                child.LogChildren(0 + 1);
            }
        }
    }
}