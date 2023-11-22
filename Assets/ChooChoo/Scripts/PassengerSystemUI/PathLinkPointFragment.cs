using System;
using TimberApi.UiBuilderSystem;
using TimberApi.UiBuilderSystem.ElementSystem;
using TimberApi.UiBuilderSystem.PresetSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.Localization;
using Timberborn.SingletonSystem;
using UnityEngine.UIElements;

namespace ChooChoo
{
  internal class PathLinkPointFragment : IEntityPanelFragment
  {
    private readonly EventBus _eventBus;
    private readonly UIBuilder _builder;
    private readonly ILoc _loc;
    private readonly ConnectPathLinkPointButton _newStationButton;
    private readonly PassengerStationLinkRepository _passengerStationLinkRepository;
    private VisualElement _root;
    private VisualElement _content;
    private PassengerStation _passengerStation;

    public PathLinkPointFragment(
      EventBus eventBus,
      UIBuilder builder,
      ILoc loc,
      ConnectPathLinkPointButton newStationButton,
      PassengerStationLinkRepository passengerStationLinkRepository)
    {
      _eventBus = eventBus;
      _builder = builder;
      _loc = loc;
      _newStationButton = newStationButton;
      _passengerStationLinkRepository = passengerStationLinkRepository;
    }

    public VisualElement InitializeFragment()
    {
      _eventBus.Register(this);
      _root = _builder.CreateFragmentBuilder()
        .AddComponent(builder => builder
          .SetWidth(new Length(100f, LengthUnit.Percent))
          .SetJustifyContent(Justify.Center)
          .AddPreset(builder =>
          {
            var localizableButton = builder.Buttons().ButtonGame();
            localizableButton.name = "PathLinkPointButton";
            localizableButton.style.width = new Length(60f, LengthUnit.Percent);
            return localizableButton;
          }))
        .AddComponent((Action<VisualElementBuilder>) (builder => builder.SetWidth(new Length(100f, LengthUnit.Percent)).SetJustifyContent(Justify.Center).AddPreset((Func<UiPresetFactory, VisualElement>) (builder =>
      {
        var localizableButton = builder.Buttons().ButtonGame();
        localizableButton.name = "RemovePathLinksButton";
        localizableButton.TextLocKey = "Tobbert.PathLinkPoint.RemoveLinks";
        localizableButton.style.width = new Length(60f, LengthUnit.Percent);
        localizableButton.style.marginTop = new Length(5f, LengthUnit.Pixel);
        return localizableButton;
      })))).BuildAndInitialize();
      _newStationButton.Initialize(_root.Q<Button>("PathLinkPointButton"), () => _passengerStation, RefreshFragment);
      _root.Q<Button>("RemovePathLinksButton").clicked += RemoveLinks;
      _root.ToggleDisplayStyle(false);
      return _root;
    }

    public void ShowFragment(BaseComponent entity)
    {
      PassengerStation component = entity.GetComponentFast<PassengerStation>();
      if (!(component != null) || !component.UIEnabledEnabled)
        return;
      _passengerStation = component;
    }

    public void ClearFragment()
    {
      _root.ToggleDisplayStyle(false);
      _newStationButton.StopRouteAddition();
      _passengerStation = null;
    }

    public void UpdateFragment()
    {
      if ((bool) (UnityEngine.Object) _passengerStation)
        _root.ToggleDisplayStyle(true);
      else
        _root.ToggleDisplayStyle(false);
    }

    private void RefreshFragment()
    {
    }

    private void RemoveLinks()
    {
      _passengerStationLinkRepository.RemoveLinks(_passengerStation);
    }
  }
}
