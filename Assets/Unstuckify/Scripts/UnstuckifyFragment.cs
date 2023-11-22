using TimberApi.UiBuilderSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.GameDistricts;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Unstuckify
{
  internal class UnstuckifyFragment : IEntityPanelFragment
  {
    private readonly UnstuckifyService _unstuckifyService;
    private readonly UIBuilder _uiBuilder;
    private Citizen _citizen;
    private VisualElement _root;
    private Button _button;

    public UnstuckifyFragment(UnstuckifyService unstuckifyService, UIBuilder uiBuilder)
    {
      _unstuckifyService = unstuckifyService;
      _uiBuilder = uiBuilder;
    }

    public VisualElement InitializeFragment()
    {
      _root = _uiBuilder.CreateFragmentBuilder()
        .AddComponent(builder => builder
          .SetFlexDirection(FlexDirection.Row)
          .SetWidth(new Length(100, LengthUnit.Percent))
          .SetJustifyContent(Justify.Center)

          .AddPreset(builder =>
          {
            var button = builder.Buttons().ButtonGame("Tobbert.Unstuckify.UnstuckifyButton");
            button.name = "button";
            return button;
          }))
        .BuildAndInitialize();

      _root.Q<Button>("button").clicked += OnClick;
      _root.ToggleDisplayStyle(false);
      return _root;
    }

    public void ShowFragment(BaseComponent entity)
    {
      _citizen = entity.GetComponentFast<Citizen>();
    }

    public void ClearFragment()
    {
      _root.ToggleDisplayStyle(false);
    }

    public void UpdateFragment()
    {
      if (!(bool)(Object)_citizen) 
        return;
      _root.ToggleDisplayStyle(!_citizen.HasAssignedDistrict);
    }

    private void OnClick()
    {
      if (!(bool)(Object)_citizen) 
        return;
      _unstuckifyService.Unstuckify(_citizen);
    }
  }
}
