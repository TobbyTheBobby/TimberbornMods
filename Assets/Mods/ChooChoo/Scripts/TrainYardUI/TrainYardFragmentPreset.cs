using ChooChoo.UIPresets;
using TimberApi.UIBuilderSystem;
using TimberApi.UIBuilderSystem.ElementBuilders;
using TimberApi.UIPresets.Buttons;
using Timberborn.CoreUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace ChooChoo.TrainYardUI
{
    public class TrainYardFragmentPreset : TrainYardPanel<TrainYardFragmentPreset>
    {
        protected override TrainYardFragmentPreset BuilderInstance => this;
    }

    public abstract class TrainYardPanel<TBuilder> : BaseBuilder<TBuilder, NineSliceVisualElement> where TBuilder : BaseBuilder<TBuilder, NineSliceVisualElement>
    {
        private const string CreateTrainLocKey = "Tobbert.TrainYard.CreateTrain";

        protected override NineSliceVisualElement InitializeRoot()
        {
            var panelFragment = UIBuilder.Create<PanelFragment>();
            panelFragment.SetFlexDirection(FlexDirection.Column);
            panelFragment.SetWidth(new Length(100, LengthUnit.Percent));
            panelFragment.SetName("PanelFragment");
            panelFragment.SetJustifyContent(Justify.Center);

            var labelBuilder = UIBuilder.Create<LabelBuilder>();
            labelBuilder.SetWidth(new Length(100, LengthUnit.Percent));
            labelBuilder.SetJustifyContent(Justify.Center);
            labelBuilder.SetColor(Color.white);
            labelBuilder.SetName("CostLabel");
            panelFragment.AddComponent(labelBuilder.Build());
            
            var gameButton = UIBuilder.Create<GameButton>();
            gameButton.SetLocKey(CreateTrainLocKey);
            gameButton.SetName("CreateButton");
            panelFragment.AddComponent(gameButton.Build());
            
            return panelFragment.Build();
        }
    }
}