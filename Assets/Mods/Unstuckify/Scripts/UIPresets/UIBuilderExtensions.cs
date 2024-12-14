using System;
using TimberApi.UIBuilderSystem;
using UnityEngine.UIElements;

namespace Unstuckify.UIPresets
{
    public static class UIBuilderExtensions
    {
        public static VisualElement BuildAndInitialize(this UIBuilder uiBuilder, Type type)
        {
            var visualElement = uiBuilder.Build(type);
            uiBuilder.Initialize(visualElement);
            return visualElement;
        }
        
        public static VisualElement BuildAndInitialize<T>(this UIBuilder uiBuilder) where T : BaseBuilder
        {
            var visualElement = uiBuilder.Build<T>();
            uiBuilder.Initialize(visualElement);
            return visualElement;
        }
    }
}