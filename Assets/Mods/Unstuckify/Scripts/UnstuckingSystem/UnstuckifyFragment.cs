using TimberApi.UIBuilderSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.GameDistricts;
using UnityEngine.UIElements;
using Unstuckify.UIPresets;
using Object = UnityEngine.Object;

namespace Unstuckify.UnstuckingSystem
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
            _root = _uiBuilder.Create<UnstuckifyPanel>().BuildAndInitialize();
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