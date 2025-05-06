using TimberApi.UIBuilderSystem.ElementBuilders;
using TimberApi.UIBuilderSystem.StyleSheetSystem;
using TimberApi.UIBuilderSystem.StylingElements;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace Unstuckify.UIPresets
{
    public class PanelFragment : VisualElementBuilder
    {
        private const string BackgroundClass = "PanelFragment";

        private VisualElementBuilder _visualElementBuilder = null!;

        protected override NineSliceVisualElement InitializeRoot()
        {
            _visualElementBuilder = UIBuilder.Create<VisualElementBuilder>();
            _visualElementBuilder.AddClass(BackgroundClass);
            _visualElementBuilder.SetPadding(new Padding(new Length(12, LengthUnit.Pixel), new Length(8, LengthUnit.Pixel)));
            return _visualElementBuilder.Build();
        }

        protected override void InitializeStyleSheet(StyleSheetBuilder styleSheetBuilder)
        {
            styleSheetBuilder.AddNineSlicedBackgroundClass(BackgroundClass, "ui/images/backgrounds/bg-3", 9, 0.5f);
        }
    }
}