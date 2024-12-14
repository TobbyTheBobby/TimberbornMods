using ChooChoo.UIPresets;
using TimberApi.UIBuilderSystem;
using TimberApi.UIBuilderSystem.ElementBuilders;
using TimberApi.UIBuilderSystem.StylingElements;
using TimberApi.UIPresets.Labels;
using TimberApi.UIPresets.Toggles;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace ChooChoo.TrackSystemUI
{
    public class TrackSectionDividerFragmentPreset : TrackSectionDividerFragmentPreset<TrackSectionDividerFragmentPreset>
    {
        protected override TrackSectionDividerFragmentPreset BuilderInstance => this;
    }

    public abstract class TrackSectionDividerFragmentPreset<TBuilder> : BaseBuilder<TBuilder, NineSliceVisualElement> where TBuilder : BaseBuilder<TBuilder, NineSliceVisualElement>
    {
        protected override NineSliceVisualElement InitializeRoot()
        {
            var panelFragment = UIBuilder.Create<PanelFragment>()
                .AddComponent<VisualElementBuilder>(builder => builder
                    .SetFlexDirection(FlexDirection.Row)
                    .SetWidth(new Length(100, LengthUnit.Percent))
                    .SetJustifyContent(Justify.Center)
                    .AddComponent<VisualElementBuilder>(builder => builder
                        .SetFlexDirection(FlexDirection.Row)
                        .SetAlignContent(Align.Center)
                        .SetAlignItems(Align.Center)
                        .SetMargin(new Margin(new Length(5, LengthUnit.Pixel)))
                        .AddComponent<GameLabel>(builder =>
                        {
                            builder.SetLocKey("Tobbert.TrackSystem.ToggleSectionDivider");
                            return builder;
                        })
                        .AddComponent<GameToggle>(builder => builder.SetName("SectionDividerToggle"))))
                .AddComponent<VisualElementBuilder>(builder => builder
                    .SetFlexDirection(FlexDirection.Row)
                    .SetWidth(new Length(100, LengthUnit.Percent))
                    .SetJustifyContent(Justify.Center)
                    .AddComponent<GameLabel>(builder =>
                    {
                        builder.SetName("CollidingSectionDividerWarningLabel");
                        builder.SetLocKey("Tobbert.TrackSystem.CollidingSectionDividers");
                        return builder;
                    }))
                .AddComponent<VisualElementBuilder>(builder => builder
                    .SetFlexDirection(FlexDirection.Row)
                    .SetWidth(new Length(100, LengthUnit.Percent))
                    .SetJustifyContent(Justify.Center)
                    .AddComponent<TimberApi.UIPresets.Buttons.GameButton>(builder =>
                    {
                        builder.SetName("ChangeDirectionButton");
                        builder.SetLocKey("Tobbert.TrackSystem.ButtonChangeDirection");
                        return builder;
                    }));

            return panelFragment.Build();
        }
    }
}