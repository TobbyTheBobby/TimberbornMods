using TimberApi.UIBuilderSystem;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace Unstuckify.UIPresets
{
    public class UnstuckifyPanel : UnstuckifyPanel<UnstuckifyPanel>
    {
        protected override UnstuckifyPanel BuilderInstance => this;
    }

    public abstract class UnstuckifyPanel<TBuilder> : BaseBuilder<TBuilder, NineSliceVisualElement> where TBuilder : BaseBuilder<TBuilder, NineSliceVisualElement>
    {
        private PanelFragment _visualElementBuilder = null!;

        protected override NineSliceVisualElement InitializeRoot()
        {
            _visualElementBuilder = UIBuilder.Create<PanelFragment>();
            _visualElementBuilder.AddComponent(typeof(UnstuckifyButton));
            _visualElementBuilder.SetFlexDirection(FlexDirection.Row);
            _visualElementBuilder.SetWidth(new Length(100, LengthUnit.Percent));
            _visualElementBuilder.SetJustifyContent(Justify.Center);
            return _visualElementBuilder.Build();
        }
    }
}