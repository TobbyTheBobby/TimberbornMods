using TimberApi.UiBuilderSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unstuckify
{
    public class UnstuckifySettingsUI
    {
        private readonly UnstuckifySetting _unstuckifySetting;
        private readonly UIBuilder _builder;

        UnstuckifySettingsUI(UnstuckifySetting unstuckifySetting, UIBuilder uiBuilder)
        {
            _unstuckifySetting = unstuckifySetting;
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
            
            var headerContainer = _builder.CreateComponentBuilder()
                .CreateVisualElement()
                .AddPreset(factory =>
                {
                    var test = factory.Labels().DefaultHeader();
                    test.TextLocKey = "Tobbert.Unstuckify.Setting.Header";
                    test.style.fontSize = new Length(16, LengthUnit.Pixel);
                    test.style.unityFontStyleAndWeight = FontStyle.Bold;
                    return test;
                })
                .BuildAndInitialize();

            var toggleContainer = _builder.CreateComponentBuilder()
                .CreateVisualElement()
                .SetFlexDirection(FlexDirection.Row)
                .SetWidth(new Length(100, LengthUnit.Percent))
                .SetHeight(new Length(60, LengthUnit.Pixel))
                .SetJustifyContent(Justify.Center)
                .SetAlignItems(Align.Center)
                .AddPreset(builder =>
                {
                    var label = builder.Labels().DefaultText();
                    label.TextLocKey = "Tobbert.Unstuckify.Setting.Label";
                    return label;
                })
                .AddPreset(builder =>
                {
                    var toggle = builder.Toggles().Checkbox();
                    toggle.SetValueWithoutNotify(_unstuckifySetting.AutomaticUnstukcifySetting.AutomaticUnstuckifyEnabled);
                    toggle.RegisterValueChangedCallback(OnToggleValueChanged);
                    return toggle;
                })
                .BuildAndInitialize();

            container.Add(headerContainer);
            container.Add(toggleContainer);

            root.Q<Toggle>("AutoSavingOn").parent.Add(container);
        }

        private void OnToggleValueChanged(ChangeEvent<bool> evt)
        {
            _unstuckifySetting.ChangeTrainModelSetting(evt.newValue);
        }
    }
}