using TimberApi.UiBuilderSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace DynamicSpecifications
{
    public class DynamicSpecificationSystemUI
    {
        private readonly DynamicSpecificationGenerator _dynamicSpecificationGenerator;
        private readonly UIBuilder _builder;

        DynamicSpecificationSystemUI(DynamicSpecificationGenerator dynamicSpecificationGenerator, UIBuilder uiBuilder)
        {
            _dynamicSpecificationGenerator = dynamicSpecificationGenerator;
            _builder = uiBuilder;
        }

        public void InitializeSelectorSettings(ref VisualElement root)
        {
            var container = _builder.CreateComponentBuilder().CreateVisualElement()
                .SetWidth(new Length(100, LengthUnit.Percent))
                .SetJustifyContent(Justify.Center)
                .SetAlignContent(Align.Center)
                .SetAlignItems(Align.Center)
                .BuildAndInitialize();
            
            var customPathsHeader = _builder.CreateComponentBuilder()
                .CreateVisualElement()
                .AddPreset(factory =>
                {
                    var test = factory.Labels().DefaultHeader();
                    test.TextLocKey = "Tobbert.DynamicSpecifications.SettingsHeader";
                    test.style.fontSize = new Length(16, LengthUnit.Pixel);
                    test.style.unityFontStyleAndWeight = FontStyle.Bold;
                    return test;
                })
                .BuildAndInitialize();

            var generateSpecificationsButtonContainer = _builder.CreateComponentBuilder()
                .CreateVisualElement()
                .SetFlexDirection(FlexDirection.Column)
                .SetWidth(new Length(100, LengthUnit.Percent))
                .SetJustifyContent(Justify.Center)
                .SetAlignItems(Align.Center)
                .AddPreset(builder =>
                {
                    var label = builder.Labels().DefaultText();
                    label.TextLocKey = "Tobbert.DynamicSpecifications.GenerateSpecificationsDescription";
                    label.style.width = new Length(100, LengthUnit.Percent);
                    return label;
                })
                .AddPreset(builder =>
                {
                    var button = builder.Buttons().Button();
                    button.name = "GenerateSpecificationsButton";
                    button.TextLocKey = "Tobbert.DynamicSpecifications.GenerateSpecificationsButton";
                    return button;
                })
                .BuildAndInitialize();

            var generateSpecificationsButton = generateSpecificationsButtonContainer.Q<Button>("GenerateSpecificationsButton");

            generateSpecificationsButton.clicked += _dynamicSpecificationGenerator.GenerateAllSpecifications;
            
            container.Add(customPathsHeader);
            container.Add(generateSpecificationsButtonContainer);

            var toggle = root.Q<Toggle>("AutoSavingOn");
            toggle.parent.Add(container);
        }
    }
}