using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace DifficultySettingsChanger.GameValueChangerSystemUI
{
    public class ScrollBarInitializationService
    {
        private static readonly string DraggerClass = "scroll-view__nine-slice-dragger";
        private static readonly string TrackerClass = "scroll-view__nine-slice-tracker";

        public void InitializeVisualElement(VisualElement visualElement)
        {
            if (!(visualElement is ScrollView scrollView))
                return;
            AddNineSliceElements(scrollView.horizontalScroller.slider);
        }

        public void InitializeTextField(TextField textField)
        {
            textField.verticalScrollerVisibility = ScrollerVisibility.Auto;
            AddNineSliceElements(textField.Q<ScrollView>().verticalScroller.slider);
        }

        private void AddNineSliceElements(VisualElement root)
        {
            VisualElement visualElement1 = root.Q<VisualElement>("unity-dragger", (string) null);
            NineSliceVisualElement sliceVisualElement1 = new NineSliceVisualElement();
            sliceVisualElement1.AddToClassList(DraggerClass);
            NineSliceVisualElement child1 = sliceVisualElement1;
            visualElement1.Add(child1);
            VisualElement visualElement2 = root.Q<VisualElement>("unity-tracker", (string) null);
            NineSliceVisualElement sliceVisualElement2 = new NineSliceVisualElement();
            sliceVisualElement2.AddToClassList(TrackerClass);
            NineSliceVisualElement child2 = sliceVisualElement2;
            visualElement2.Add(child2);
        }
    }
}