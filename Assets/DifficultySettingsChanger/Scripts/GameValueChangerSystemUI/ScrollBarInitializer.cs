using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace DifficultySettingsChanger
{
    public class ScrollBarInitializer : IVisualElementInitializer
    {
        public readonly ScrollBarInitializationService _scrollBarInitializationService;

        public ScrollBarInitializer(
            ScrollBarInitializationService scrollBarInitializationService)
        {
            this._scrollBarInitializationService = scrollBarInitializationService;
        }

        public void InitializeVisualElement(VisualElement visualElement) => this._scrollBarInitializationService.InitializeVisualElement(visualElement);
    }
}