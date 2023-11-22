using UnityEngine.UIElements;

namespace DifficultySettingsChanger
{
    public class HierarchicalLayer
    {
        public readonly int Index;

        public readonly VisualElement VisualElement;

        public HierarchicalLayer(int index, VisualElement visualElement)
        {
            Index = index;
            VisualElement = visualElement;
        }
    }
}