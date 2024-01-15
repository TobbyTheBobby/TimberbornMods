using ChooChoo.PassengerSystem;
using TimberApi.UiBuilderSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using UnityEngine.UIElements;

namespace ChooChoo.PassengerSystemUI
{
    internal class PassengerDistrictObstacleFragment : IEntityPanelFragment
    {
        private readonly UIBuilder _uiBuilder;
        private PassengerStationDistrictObject _passengerStationDistrictObject;
        private VisualElement _root;
        private Toggle _sectionDividerToggle;

        public PassengerDistrictObstacleFragment(UIBuilder uiBuilder)
        {
            _uiBuilder = uiBuilder;
        }

        public VisualElement InitializeFragment()
        {
            _root = _uiBuilder.CreateFragmentBuilder()
                .AddComponent(builder => builder
                    .SetFlexDirection(FlexDirection.Row)
                    .SetWidth(new Length(100, LengthUnit.Percent))
                    .SetJustifyContent(Justify.Center)
                    .AddComponent(builder => builder
                        .SetFlexDirection(FlexDirection.Row)
                        .SetAlignContent(Align.Center)
                        .SetAlignItems(Align.Center)
                        .SetMargin(new Margin(new Length(5, LengthUnit.Pixel)))
                        .AddPreset(builder =>
                        {
                            var label = builder.Labels().GameText("Tobbert.PassengerStation.DistrictObjectToggle");
                            label.style.marginRight = new Length(5, LengthUnit.Pixel);
                            return label;
                        })
                        .AddPreset(builder => builder.Toggles().Checkbox(name: "DistrictObstacleToggle"))))
                .AddComponent(builder => builder
                    .SetFlexDirection(FlexDirection.Row)
                    .SetWidth(new Length(100, LengthUnit.Percent))
                    .SetJustifyContent(Justify.Center)
                    .AddPreset(builder =>
                        builder.Labels().GameText(name: "CollidingSectionDividerWarningLabel", locKey: "Tobbert.PassengerStation.DistrictObject")))
                .BuildAndInitialize();

            _sectionDividerToggle = _root.Q<Toggle>("DistrictObstacleToggle");
            _sectionDividerToggle.RegisterValueChangedCallback(v => OnValueChanged(v.newValue));


            _root.ToggleDisplayStyle(false);
            return _root;
        }

        public void ShowFragment(BaseComponent entity)
        {
            _passengerStationDistrictObject = entity.GetComponentFast<PassengerStationDistrictObject>();
            if (!_passengerStationDistrictObject || !_passengerStationDistrictObject.enabled)
                return;
            _root.ToggleDisplayStyle(true);
            _sectionDividerToggle.SetValueWithoutNotify(_passengerStationDistrictObject.GoesAcrossDistrict);
        }

        public void ClearFragment()
        {
            _root.ToggleDisplayStyle(false);
            _passengerStationDistrictObject = null;
        }

        public void UpdateFragment()
        {
        }

        private void OnValueChanged(bool newValue)
        {
            _passengerStationDistrictObject.UpdateDistrictObject(newValue);
        }
    }
}