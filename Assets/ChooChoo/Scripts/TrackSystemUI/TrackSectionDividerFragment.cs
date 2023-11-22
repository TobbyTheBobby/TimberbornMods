using TimberApi.UiBuilderSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace ChooChoo
{
  internal class TrackSectionDividerFragment : IEntityPanelFragment
  {
    private readonly UIBuilder _uiBuilder;
    private OneWayTrack _oneWayTrack;
    private VisualElement _root;
    private Button _changeDirectionButton;
    private Label _collidingSectionDividerWarningLabel;
    private Toggle _sectionDividerToggle;

    public TrackSectionDividerFragment(UIBuilder uiBuilder)
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
              var label = builder.Labels().GameText("Tobbert.TrackSystem.ToggleSectionDivider");
              label.style.marginRight = new Length(5, LengthUnit.Pixel);
              return label;
            })

            .AddPreset(builder => builder.Toggles().Checkbox(name: "SectionDividerToggle"))))
        .AddComponent(builder => builder
          .SetFlexDirection(FlexDirection.Row)
          .SetWidth(new Length(100, LengthUnit.Percent))
          .SetJustifyContent(Justify.Center)
          .AddPreset(builder =>
            {
              var label = builder.Labels().GameText(name: "CollidingSectionDividerWarningLabel", locKey: "Tobbert.TrackSystem.CollidingSectionDividers");
              label.style.color = new StyleColor(Color.red);
              return label;
            }))
        .AddComponent(builder => builder
          .SetFlexDirection(FlexDirection.Row)
          .SetWidth(new Length(100, LengthUnit.Percent))
          .SetJustifyContent(Justify.Center)
          .AddPreset(builder => builder.Buttons().ButtonGame(name: "ChangeDirectionButton", locKey: "Tobbert.TrackSystem.ButtonChangeDirection")))
        .BuildAndInitialize();

      _changeDirectionButton = _root.Q<Button>("ChangeDirectionButton");
      _changeDirectionButton.clicked += ChangeDirection;
      _changeDirectionButton.ToggleDisplayStyle(false);

      _collidingSectionDividerWarningLabel = _root.Q<Label>("CollidingSectionDividerWarningLabel");
      _collidingSectionDividerWarningLabel.ToggleDisplayStyle(false);
      
      _sectionDividerToggle = _root.Q<Toggle>("SectionDividerToggle");
      _sectionDividerToggle.RegisterValueChangedCallback(v => OnValueChanged(v.newValue));


      _root.ToggleDisplayStyle(false);
      return _root;
    }

    public void ShowFragment(BaseComponent entity)
    {
      _oneWayTrack = entity.GetComponentFast<OneWayTrack>();
      if (!(bool)(Object)_oneWayTrack || !_oneWayTrack.enabled) 
        return;
      _root.ToggleDisplayStyle(true);
      _sectionDividerToggle.SetValueWithoutNotify(_oneWayTrack.DividesSection);
      var collidesWithAnotherSectionDivider = _oneWayTrack.WouldCollideWithAnotherSectionDivider();
      // Plugin.Log.LogError(collidesWithAnotherSectionDivider + "");
      _sectionDividerToggle.SetEnabled(!collidesWithAnotherSectionDivider);
      _collidingSectionDividerWarningLabel.ToggleDisplayStyle(collidesWithAnotherSectionDivider);
      _changeDirectionButton.ToggleDisplayStyle(_oneWayTrack.DividesSection);
    }

    public void ClearFragment()
    {
      _root.ToggleDisplayStyle(false);
      _oneWayTrack = null;
    }

    public void UpdateFragment() { }

    private void OnValueChanged(bool newValue)
    {
      _oneWayTrack.ToggleDividesSection(newValue);
      _changeDirectionButton.ToggleDisplayStyle(newValue);
    }

    private void ChangeDirection()
    {
      _oneWayTrack.ChangeDirection();
    }
  }
}
