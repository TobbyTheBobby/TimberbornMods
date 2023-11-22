using System.Linq;
using TimberApi.UiBuilderSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.CoreUI;
using Timberborn.DropdownSystem;
using Timberborn.EntityPanelSystem;
using Timberborn.Localization;
using UnityEngine.UIElements;

namespace ChooChoo
{
  internal class TrainTypeSelectorFragment : IEntityPanelFragment
  {
    private readonly UIBuilder _uiBuilder;
    private ILoc _loc;
    private readonly VisualElementLoader _visualElementLoader;
    private readonly DropdownItemsSetter _dropdownItemsSetter;
    private TrainTypeDropdownProvider _trainTypeDropdownProvider;
    private Dropdown _dropdown;
    private VisualElement _root;
    private readonly int _numberOfWagons = 4;

    public TrainTypeSelectorFragment(
      UIBuilder uiBuilder,
      ILoc loc,
      VisualElementLoader visualElementLoader,
      DropdownItemsSetter dropdownItemsSetter)
    {
      _uiBuilder = uiBuilder;
      _loc = loc;
      _visualElementLoader = visualElementLoader;
      _dropdownItemsSetter = dropdownItemsSetter;
    }

    public VisualElement InitializeFragment()
    {
      _root = _uiBuilder.CreateFragmentBuilder().BuildAndInitialize();
      
      var fragment = _visualElementLoader.LoadVisualElement("Game/EntityPanel/PlantablePrioritizerFragment");
      var container = new VisualElement();
      foreach (var element in fragment.Children().ToList())
      {
        container.Add(element);
        container.Q<Label>().text = _loc.T("Tobbert.TrainModel.TrainModel");
      }
      _root.Add(container);
      _dropdown = container.Q<Dropdown>("Priorities");

      _root.ToggleDisplayStyle(false);
      return _root;
    }

    public void ShowFragment(BaseComponent entity)
    {
      _trainTypeDropdownProvider = entity.GetComponentFast<TrainTypeDropdownProvider>();
      if (!_trainTypeDropdownProvider)
        return;
      _dropdownItemsSetter.SetLocalizableItems(_dropdown, _trainTypeDropdownProvider);
    }

    public void ClearFragment()
    {
      _dropdown.ClearItems();
      _trainTypeDropdownProvider = null;
      UpdateFragment();
    }

    public void UpdateFragment() => _root.ToggleDisplayStyle(_trainTypeDropdownProvider);
  }
}
